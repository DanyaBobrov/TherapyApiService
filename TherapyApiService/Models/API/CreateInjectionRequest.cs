using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TherapyApiService.Models.API
{
    public class CreateInjectionRequest
    {
        [Required(AllowEmptyStrings = false)]
        [SwaggerSchema("Injection identifier")]
        public string Id { get; set; }
    }
}