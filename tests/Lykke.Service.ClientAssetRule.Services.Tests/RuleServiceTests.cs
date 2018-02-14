using System;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Log;
using Lykke.Service.Assets.Client;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.ClientAssetRule.Services.Tests
{
    [TestClass]
    public class RuleServiceTests
    {
        private readonly Mock<IAssetsService> _assetsServiceMock;
        private readonly Mock<IAssetGroupRuleRepository> _ruleRepositoryMock;
        private readonly AssetGroupRuleService _service;

        public RuleServiceTests()
        {
            var logMock = new Mock<ILog>();
            _assetsServiceMock = new Mock<IAssetsService>();
            _ruleRepositoryMock = new Mock<IAssetGroupRuleRepository>();

            _service = new AssetGroupRuleService(
                _ruleRepositoryMock.Object,
                _assetsServiceMock.Object,
                logMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task InsertAsync_Throw_InvalidOperationException_If_Rule_For_Regulation_Exist()
        {
            // arrange
            var rule = new AssetGroupRule
            {
                RegulationId = "reg"
            };

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<AssetGroupRule>
                {
                    rule
                });
            
            // act
            Task task = _service.InsertAsync(rule);

            // assert
            await task;
        }

        [TestMethod]
        public async Task GetAssetGroupsAsync_OK()
        {
            // arrange
            const string regulation1 = "regulation1";
            const string regulation2 = "regulation2";

            const string clientId = "clientId";

            var regulations = new List<string>
            {
                regulation1,
                regulation2
            };

            var assetRules = new List<IAssetGroupRule>
            {
                new AssetGroupRule("1", "n1", regulation1, new List<string>{ "group_1", "group_2"}, new List<string>{ "group_d_1", "group_d_2"}),
                new AssetGroupRule("2", "n2", regulation2, new List<string>{ "group_3", "group_4"}, new List<string>{ "group_d_3", "group_d_4"})
            };

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string regulationId) => assetRules.Where(o => o.RegulationId == regulationId));

            _assetsServiceMock.Setup(o => o.AssetGroupAddOrReplaceClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse()));

            _assetsServiceMock.Setup(o => o.AssetGroupRemoveClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse()));

            // act
            await _service.SetAsync(clientId, regulations);

            // assert
            _assetsServiceMock.Verify(o => o.AssetGroupAddOrReplaceClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()),
                Times.Exactly(4));
            _assetsServiceMock.Verify(o => o.AssetGroupRemoveClientWithHttpMessagesAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(4));
        }

        [TestMethod]
        public async Task GetAssetGroupsAsync_Declined_And_Allowed_Intersection()
        {
            // arrange
            const string regulation1 = "regulation1";
            const string regulation2 = "regulation2";

            const string clientId = "clientId";

            var regulations = new List<string>
            {
                regulation1,
                regulation2
            };

            var assetRules = new List<IAssetGroupRule>
            {
                new AssetGroupRule("1", "n1", regulation1, new List<string>{"group_1", "group_2"}, new List<string>{"group_3", "group_4"}),
                new AssetGroupRule("2", "n2", regulation2, new List<string>{"group_3", "group_4", "group_5"}, new List<string>{"group_1", "group_2"})
            };

            var addedLayers = new List<string>();
            var removedLayers = new List<string>();

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string regulationId) => assetRules.Where(o => o.RegulationId == regulationId));

            _assetsServiceMock.Setup(o => o.AssetGroupAddOrReplaceClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse()))
                .Callback((string client, string layerId, Dictionary<string, List<string>> customHeaders,
                        CancellationToken cancellationToken) =>
                        addedLayers.Add(layerId));

            _assetsServiceMock.Setup(o => o.AssetGroupRemoveClientWithHttpMessagesAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new HttpOperationResponse()))
                .Callback((string client, string layerId, Dictionary<string, List<string>> customHeaders,
                        CancellationToken cancellationToken) =>
                        removedLayers.Add(layerId));

            // act
            await _service.SetAsync(clientId, regulations);

            // assert
            Assert.IsTrue(new[] { "group_5" }.All(o => addedLayers.Contains(o)) &&
                          new[] { "group_1", "group_2", "group_3", "group_4" }.All(o => removedLayers.Contains(o)));
        }
    }

}
