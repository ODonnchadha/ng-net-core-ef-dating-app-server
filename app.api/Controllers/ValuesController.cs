using app.api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

        [HttpGet()]
        public async Task<IActionResult> GetValuesAsync()
        {
            var values = await context.Values.ToListAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetValueAsync(int id)
        {
            var value = await context.Values.FirstOrDefaultAsync(
                value => value.Id == id);
            return Ok(value);
        }
    }
}
