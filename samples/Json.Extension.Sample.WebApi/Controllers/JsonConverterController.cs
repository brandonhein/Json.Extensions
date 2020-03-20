using Hein.Swagger.Attributes;
using Json.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Json.Extension.Sample.WebApi.Controllers
{
    [SwaggerController("Json Paths => Json Object/Array")]
    [Swagger]
    [Route("convert")]
    public class JsonConverterController : Controller
    {
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [Summary("Take json paths and it's value and create a json we all know and love")]
        public async Task<IActionResult> ConverToObject([FromBody] Dictionary<string, object> paths)
        {
            var result = JsonSimplifier.Simplify(paths);
            return Ok(result);
        }

        [HttpPost("shell")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), 200)]
        [Summary("Take Json Path List and convert those paths to a json shell object")]
        public async Task<IActionResult> ConverToShellObject([FromBody] List<string> paths)
        {
            var shell = JsonShell.GenerateFromPaths(paths);
            return Ok(shell);
        }
    }
}
