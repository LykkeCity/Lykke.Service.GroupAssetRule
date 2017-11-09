using System;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Services;
using Moq;
using Xunit;
using System.Collections.Generic;

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
    }

}
