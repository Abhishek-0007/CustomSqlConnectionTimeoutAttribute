using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITestService _test;
        private readonly IRecurringJobManager recurringJobManager;

        public WeatherForecastController(IServiceProvider serviceProvider)
        {
            _config = serviceProvider.GetRequiredService<IConfiguration>();
            _test = serviceProvider.GetRequiredService<ITestService>();
            recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();
        }

        [HttpPost("test")]
        public async Task TestSql()
        {
            recurringJobManager.AddOrUpdate("test job", () => _test.Check(), "* */6 * * *");
        }
    }
}