using Microsoft.AspNetCore.Mvc;
using CRUD_API.IRepository;
using System.Threading.Tasks;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APICallfromOtherApplicationController : ControllerBase
    {
        private readonly IAPICallfromOtherApplication _converter;
        public APICallfromOtherApplicationController(IAPICallfromOtherApplication converter)
        {
            _converter = converter;
        }



        [HttpPost]
        [Route("CreateStudent")]
        public async Task<IActionResult> Create()
        {
            var msg = await _converter.Create();
            return Ok(msg);

        }
    }
}
