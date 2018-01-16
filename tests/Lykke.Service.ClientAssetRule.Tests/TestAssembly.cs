using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.ClientAssetRule.Tests
{
    [TestClass]
    public class TestAssembly
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
