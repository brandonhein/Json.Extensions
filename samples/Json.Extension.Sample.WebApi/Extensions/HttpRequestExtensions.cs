using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Json.Extension.Sample.WebApi.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<string> ReadAsStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            request.EnableBuffering();

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var reader = new StreamReader(request.Body, encoding))
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                return await reader.ReadToEndAsync();
            }
        }
    }
}
