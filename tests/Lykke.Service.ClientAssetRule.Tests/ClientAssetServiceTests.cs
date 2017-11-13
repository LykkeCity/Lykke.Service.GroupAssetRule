using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Services;
using Microsoft.Rest;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace Lykke.Service.ClientAssetRule.Tests
{
    public class ClientAssetServiceTests
    {
        private readonly Mock<IRuleRepository> _ruleRepositoryMock;
        private readonly ClientAssetService _service;

        private readonly ISetup<IAssetsService, Task<HttpOperationResponse>> _addAssetGroup;
        private readonly ISetup<IAssetsService, Task<HttpOperationResponse>> _removeAssetGroup;

        public ClientAssetServiceTests()
        {
            var assetsServiceMock = new Mock<IAssetsService>();
            _ruleRepositoryMock = new Mock<IRuleRepository>();

            _addAssetGroup = assetsServiceMock.Setup(o =>
                    o.AssetGroupAddOrReplaceClientWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()));
            _addAssetGroup.Returns(Task.FromResult(new HttpOperationResponse()));
            
            _removeAssetGroup = assetsServiceMock.Setup(o =>
                o.AssetGroupRemoveClientWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()));
            _removeAssetGroup.Returns(Task.FromResult(new HttpOperationResponse()));

            assetsServiceMock.Setup(o => o.Dispose());

            _service = new ClientAssetService(
                _ruleRepositoryMock.Object,
                assetsServiceMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_OK()
        {
            // arrange
            string clientId = "me";

            var clientRegulations = new List<IClientRegulation>
            {
                new ClientRegulation("reg1", true, true),
                new ClientRegulation("reg2", true, true)
            };

            var assetRules = new List<IRule>
            {
                new Rule("1", "n1", "reg1", new List<string>{"A1", "A2"}, new List<string>{"D1", "D2"}),
                new Rule("2", "n2", "reg2", new List<string>{"A3", "A4"}, new List<string>{"D3", "D4"})
            };

            var added = new List<string>();
            var removed = new List<string>();

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg1")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg1").ToList());

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg2")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg2").ToList());

            _addAssetGroup.Callback((string a, string b, Dictionary<string, List<string>> c, CancellationToken d) =>
            {
                added.Add(b);
            });

            _removeAssetGroup.Callback((string a, string b, Dictionary<string, List<string>> c, CancellationToken d) =>
            {
                removed.Add(b);
            });
            
            // act
            await _service.UpdateAsync(clientId, clientRegulations);

            // assert
            Assert.Equal(assetRules.Sum(o => o.AllowedAssetGroups.Count), added.Count);
            Assert.Equal(assetRules.Sum(o => o.DeclinedAssetGroups.Count), removed.Count);
        }

        [Fact]
        public async Task UpdateAsync_Declined_And_Allowed_Intersection()
        {
            // arrange
            string clientId = "me";

            var clientRegulations = new List<IClientRegulation>
            {
                new ClientRegulation("reg1", true, true),
                new ClientRegulation("reg2", true, true)
            };

            var assetRules = new List<IRule>
            {
                new Rule("1", "n1", "reg1", new List<string>{"A1", "A2"}, new List<string>{"A3", "A4"}),
                new Rule("2", "n2", "reg2", new List<string>{"A3", "A4", "A5"}, new List<string>{"A1", "A2"})
            };

            var added = new List<string>();
            var removed = new List<string>();

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg1")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg1").ToList());

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg2")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg2").ToList());

            _addAssetGroup.Callback((string a, string b, Dictionary<string, List<string>> c, CancellationToken d) =>
            {
                added.Add(b);
            });

            _removeAssetGroup.Callback((string a, string b, Dictionary<string, List<string>> c, CancellationToken d) =>
            {
                removed.Add(b);
            });

            // act
            await _service.UpdateAsync(clientId, clientRegulations);

            // assert
            Assert.Single(added);
            Assert.Equal(4, removed.Count);
        }
    }
}
