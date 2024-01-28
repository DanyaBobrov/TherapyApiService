using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TherapyApiService.Models;
using TherapyApiService.Models.API;
using TherapyApiService.Models.API.DTO;

namespace TherapyApiService.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InjectionsController : ControllerBase
    {
        private readonly ILogger<InjectionsController> logger;

        public InjectionsController(
            ILogger<InjectionsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [SwaggerOperation("Create a new injection")]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(InjectionDTO))]
        public async Task<IActionResult> CreateInjectionAsync(CreateInjectionRequest request)
        {
            var injection = new Injection();
            return Created("TODO ???", InjectionDTO.Mapper.Map(injection));
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get the injection")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(InjectionDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetInjectionAsync([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [SwaggerOperation("Get list of injections")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<InjectionDTO>))]
        public async Task<IActionResult> ListInjectionsAsync([FromQuery] ListInjectionsRequest request)//TODO filter
        {
            var response = new ListInjectionsResponse();
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [SwaggerOperation("Update the injection")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(InjectionDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateInjectionAsync([FromRoute] int id, [FromBody] UpdateInjectionRequest updateInjectionRequest)//TODO partial update
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [SwaggerOperation("Replace the injection")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(InjectionDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ReplaceInjectionAsync([FromRoute] int id, [FromBody] ReplaceInjectionRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete the injection")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(InjectionDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteInjectionAsync([FromRoute] int id)
        {
            throw new NotImplementedException();
            //return NoContent(); TODO NoContent ???
        }
    }
}