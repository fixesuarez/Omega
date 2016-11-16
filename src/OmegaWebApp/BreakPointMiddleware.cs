using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OmegaWebApp
{
    public class BreakPointMiddleware
    {
        readonly RequestDelegate _next;

        public BreakPointMiddleware( RequestDelegate next )
        {
            _next = next;
        }

        public Task Invoke( HttpContext context )
        {
            return _next( context );
        }
    }
}
