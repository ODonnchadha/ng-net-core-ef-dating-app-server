using app.api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace app.api.Controllers
{
    [ApiController(), Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ILogger<ValuesController> logger;

        public ValuesController(DataContext context, ILogger<ValuesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value = context.Values.FirstOrDefault(
                value => value.Id == id);
            return Ok(value);
        }

        [HttpGet()]
        public IActionResult GetValues()
        {
            var values = context.Values.ToList();
            return Ok(values);
        }
    }
}
