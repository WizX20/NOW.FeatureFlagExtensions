using FeatureTestApplication.Configurations;
using FeatureTestApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeatureTestApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ITestService _testService;
        private readonly IEnumerable<ITestService> _testServices;
        private readonly ITestConfiguration _testConfiguration;
        private readonly IEnumerable<ITestConfiguration> _testConfigurations;

        public TestController(
            ILogger<TestController> logger,
            ITestService testService,
            IEnumerable<ITestService> testServices,
            ITestConfiguration testConfiguration,
            IEnumerable<ITestConfiguration> testConfigurations)
        {
            _logger = logger;
            _testService = testService;
            _testServices = testServices;
            _testConfiguration = testConfiguration;
            _testConfigurations = testConfigurations;
        }

        [HttpGet, ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult Get()
        {
            var names = new List<string>();

            foreach (var configuration in _testConfigurations)
            {
                names.Add($"configuration: {configuration.Name}");
            }

            foreach (var service in _testServices)
            {
                names.Add($"configuration: {service.Name}");
            }

            return Ok(names);
        }

        [HttpGet("service"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetService()
        {
            return Ok(_testService.Name);
        }

        [HttpGet("config"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetConfiguration()
        {
            return Ok(_testConfiguration.Name);
        }
    }
}