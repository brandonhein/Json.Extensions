using Hein.Swagger.Attributes;
using Json.Extension.Sample.WebApi.Extensions;
using Json.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Json.Extension.Sample.WebApi.Controllers
{
    [SwaggerController("Json Object/Array => Json Paths")]
    [Swagger]
    [Route("parse")]
    public class JsonParserController : Controller
    {
        [HttpPost]
        [Consumes("text/plain", "application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        [Summary("Parse thru any json to get paths to specific values")]
        public async Task<IActionResult> GetJsonPathsAndValuesAsync([FromBody] string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                json = await Request.ReadAsStringAsync();
            }

            var result = JsonParser.PathsAndValues(json);
            return Ok(result);
        }

        [HttpPost("paths")]
        [Consumes("text/plain", "application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [Summary("Parse thru any json to get just paths")]
        public async Task<IActionResult> GetJsonPathsAsync([FromBody] string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                json = await Request.ReadAsStringAsync();
            }

            var result = JsonParser.Paths(json);
            return Ok(result);
        }
    }
}
