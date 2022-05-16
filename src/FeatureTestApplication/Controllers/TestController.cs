using Microsoft.AspNetCore.Mvc;
using DefaultInjectionTests = FeatureTestApplication.TestFeatures.DefaultInjection;
using InterceptorInjectionTests = FeatureTestApplication.TestFeatures.InterceptorInjection;

namespace FeatureTestApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        private readonly DefaultInjectionTests.ScopedFeature.ITestServiceScoped _defaultTestServiceScoped;
        private readonly IEnumerable<DefaultInjectionTests.ScopedFeature.ITestServiceScoped> _defaultTestServicesScoped;
        private readonly DefaultInjectionTests.TransientFeature.ITestServiceTransient _defaultTestServiceTransient;
        private readonly IEnumerable<DefaultInjectionTests.TransientFeature.ITestServiceTransient> _defaultTestServicesTransient;

        private readonly InterceptorInjectionTests.ScopedFeature.ITestServiceScoped _interceptedTestServiceScoped;
        private readonly IEnumerable<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped> _interceptedTestServicesScoped;
        private readonly InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton _interceptedTestServiceSingleton;
        private readonly IEnumerable<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton> _interceptedTestServicesSingleton;
        private readonly InterceptorInjectionTests.TransientFeature.ITestServiceTransient _interceptedTestServiceTransient;
        private readonly IEnumerable<InterceptorInjectionTests.TransientFeature.ITestServiceTransient> _interceptedTestServicesTransient;

        public TestController(
            ILogger<TestController> logger,
            DefaultInjectionTests.ScopedFeature.ITestServiceScoped defaultTestServiceScoped,
            IEnumerable<DefaultInjectionTests.ScopedFeature.ITestServiceScoped> defaultTestServicesScoped,
            DefaultInjectionTests.TransientFeature.ITestServiceTransient defaultTestServiceTransient,
            IEnumerable<DefaultInjectionTests.TransientFeature.ITestServiceTransient> defaultTestServicesTransient,

            InterceptorInjectionTests.ScopedFeature.ITestServiceScoped interceptedTestServiceScoped,
            IEnumerable<InterceptorInjectionTests.ScopedFeature.ITestServiceScoped> interceptedTestServicesScoped,
            InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton interceptedTestServiceSingleton,
            IEnumerable<InterceptorInjectionTests.SingletonFeature.ITestServiceSingleton> interceptedTestServicesSingleton,
            InterceptorInjectionTests.TransientFeature.ITestServiceTransient interceptedTestServiceTransient,
            IEnumerable<InterceptorInjectionTests.TransientFeature.ITestServiceTransient> interceptedTestServicesTransient)
        {
            _logger = logger;

            _defaultTestServiceScoped = defaultTestServiceScoped;
            _defaultTestServicesScoped = defaultTestServicesScoped;
            _defaultTestServiceTransient = defaultTestServiceTransient;
            _defaultTestServicesTransient = defaultTestServicesTransient;

            _interceptedTestServiceScoped = interceptedTestServiceScoped;
            _interceptedTestServicesScoped = interceptedTestServicesScoped;
            _interceptedTestServiceSingleton = interceptedTestServiceSingleton;
            _interceptedTestServicesSingleton = interceptedTestServicesSingleton;
            _interceptedTestServiceTransient = interceptedTestServiceTransient;
            _interceptedTestServicesTransient = interceptedTestServicesTransient;
        }

        [HttpGet, ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult Get()
        {
            var names = new List<string>();

            foreach (var service in _defaultTestServicesScoped)
            {
                names.Add($"default test service scoped: {service.Name}");
            }

            foreach (var service in _defaultTestServicesTransient)
            {
                names.Add($"default test service transient: {service.Name}");
            }

            foreach (var service in _interceptedTestServicesScoped)
            {
                names.Add($"intercepted test service scoped: {service.Name}");
            }

            foreach (var service in _interceptedTestServicesSingleton)
            {
                names.Add($"intercepted test service singleton: {service.Name}");
            }

            foreach (var service in _interceptedTestServicesTransient)
            {
                names.Add($"intercepted test service transient: {service.Name}");
            }

            return Ok(names);
        }

        [HttpGet("default/scoped"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetDefaultScoped()
        {
            return Ok(_defaultTestServiceScoped.Name);
        }

        [HttpGet("default/transient"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetDefaultTransient()
        {
            return Ok(_defaultTestServiceTransient.Name);
        }

        [HttpGet("intercepted/scoped"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetInterceptedScoped()
        {
            return Ok(_interceptedTestServiceScoped.Name);
        }

        [HttpGet("intercepted/singleton"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetInterceptedSingleton()
        {
            return Ok(_interceptedTestServiceSingleton.Name);
        }

        [HttpGet("intercepted/transient"), ResponseCache(CacheProfileName = Constants.ResponseCache.DefaultCacheProfile)]
        public IActionResult GetInterceptedTransient()
        {
            return Ok(_interceptedTestServiceTransient.Name);
        }
    }
}