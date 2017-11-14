using System;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Services;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.ClientAssetRule.Tests
{
    public class RuleServiceTests
    {
        private readonly Mock<IRuleRepository> _ruleRepositoryMock;
        private readonly RuleService _service;

        public RuleServiceTests()
        {
            _ruleRepositoryMock = new Mock<IRuleRepository>();
            _service = new RuleService(_ruleRepositoryMock.Object);
        }

        [Fact]
        public async Task InsertAsync_Throw_InvalidOperationException_If_Rule_For_Regulation_Exist()
        {
            // arrange
            var rule = new Rule
            {
                RegulationId = "reg"
            };

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Rule>
                {
                    rule
                });
            
            // act
            Task task = _service.InsertAsync(rule);

            // assert
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await task);
            Assert.Equal("Rule for specified regulation already exist", exception.Message);
        }

        [Fact]
        public async Task GetAssetGroupsAsync_OK()
        {
            // arrange
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

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg1")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg1").ToList());

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg2")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg2").ToList());

            // act
            IAssetGroups assetGroups = await _service.GetAssetGroupsAsync(clientRegulations);

            // assert
            Assert.Equal(assetRules.Sum(o => o.AllowedAssetGroups.Count), assetGroups.Allowed.Count);
            Assert.Equal(assetRules.Sum(o => o.DeclinedAssetGroups.Count), assetGroups.Declined.Count);
        }

        [Fact]
        public async Task GetAssetGroupsAsync_Declined_And_Allowed_Intersection()
        {
            // arrange
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

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg1")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg1").ToList());

            _ruleRepositoryMock.Setup(o => o.GetByRegulationIdAsync(It.Is<string>(p => p == "reg2")))
                .ReturnsAsync(assetRules.Where(o => o.RegulationId == "reg2").ToList());

            // act
            IAssetGroups assetGroups = await _service.GetAssetGroupsAsync(clientRegulations);

            // assert
            Assert.Single(assetGroups.Allowed);
            Assert.Equal(4, assetGroups.Declined.Count);
        }
    }

}
