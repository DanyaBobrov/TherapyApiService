using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TherapyApiService.Models.API
{
    public class CreateInjectionRequest
    {
        [Required(AllowEmptyStrings = false)]
        [SwaggerSchema("Injection date")]
        public DateOnly Date { get; set; }
    }
}