using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.ClientAssetRule.Services.Tests
{
    [TestClass]
    public class AssetConditionLayerRuleServiceTests
    {
        private readonly Mock<ILog> _logMock;
        private readonly Mock<IAssetsService> _assetsServiceMock;
        private readonly Mock<IAssetConditionLayerRuleRepository> _assetConditionLayerRuleRepositoryMock;
        private readonly AssetConditionLayerRuleService _service;

        public AssetConditionLayerRuleServiceTests()
        {
            _logMock = new Mock<ILog>();
            _assetsServiceMock = new Mock<IAssetsService>();
            _assetConditionLayerRuleRepositoryMock = new Mock<IAssetConditionLayerRuleRepository>();

            _service = new AssetConditionLayerRuleService(
                _assetConditionLayerRuleRepositoryMock.Object,
                _assetsServiceMock.Object,
                _logMock.Object);
        }

        [TestMethod]
        public async Task SetAsync_Adds_Two_Layers_Removes_One_Layer()
        {
            // arrange
            const string regulation1 = "regulation1";
            const string regulation2 = "regulation2";
            const string regulation3 = "regulation3";

            const string clientId = "clientId";

            var clientRegulations = new List<string>
            {
                regulation1,
                regulation2
            };

            var assetConditionLayerRules = new List<IAssetConditionLayerRule>
            {
                new AssetConditionLayerRule("rule 1", regulation1, new List<string>{"layer1"}),
                new AssetConditionLayerRule("rule 2", regulation2, new List<string>{"layer2"}),
                new AssetConditionLayerRule("rule 3", regulation3, new List<string>{"layer3"})
            };

            _assetConditionLayerRuleRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IList<IAssetConditionLayerRule>) assetConditionLayerRules));

            _assetsServiceMock.Setup(o => o.AssetConditionAddLayerToClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse<ErrorResponse>()));

            _assetsServiceMock.Setup(o => o.AssetConditionRemoveLayerFromClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse<ErrorResponse>()));

            // act
            await _service.SetAsync(clientId, clientRegulations);

            // assert
            _assetsServiceMock.Verify(o => o.AssetConditionAddLayerToClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()),
                Times.Exactly(2));
            _assetsServiceMock.Verify(o => o.AssetConditionRemoveLayerFromClientWithHttpMessagesAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(1));
        }

        [TestMethod]
        public async Task SetAsync_Intersection()
        {
            // arrange
            const string regulation1 = "regulation1";
            const string regulation2 = "regulation2";
            const string regulation3 = "regulation3";

            const string clientId = "clientId";

            var clientRegulations = new List<string>
            {
                regulation2
            };

            var assetConditionLayerRules = new List<IAssetConditionLayerRule>
            {
                new AssetConditionLayerRule("rule 1", regulation1, new List<string>{"layer1", "layer2", "layer3"}),
                new AssetConditionLayerRule("rule 2", regulation2, new List<string>{"layer1", "layer2"}),
                new AssetConditionLayerRule("rule 3", regulation3, new List<string>{"layer1", "layer2", "layer4"})
            };

            var addedLayers = new List<string>();
            var removedLayers = new List<string>();

            _assetConditionLayerRuleRepositoryMock.Setup(o => o.GetAsync())
                .Returns(Task.FromResult((IList<IAssetConditionLayerRule>)assetConditionLayerRules));

            _assetsServiceMock.Setup(o => o.AssetConditionAddLayerToClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse<ErrorResponse>()))
                .Callback((string client, string layerId, Dictionary<string, List<string>> customHeaders,
                        CancellationToken cancellationToken) =>
                        addedLayers.Add(layerId));

            _assetsServiceMock.Setup(o => o.AssetConditionRemoveLayerFromClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse<ErrorResponse>()))
                .Callback((string client, string layerId, Dictionary<string, List<string>> customHeaders,
                        CancellationToken cancellationToken) =>
                        removedLayers.Add(layerId));

            // act
            await _service.SetAsync(clientId, clientRegulations);

            // assert
            Assert.IsTrue(new[] {"layer1", "layer2"}.All(o => addedLayers.Contains(o)) &&
                          new[] {"layer3", "layer4"}.All(o => removedLayers.Contains(o)));
        }
    }
}
