namespace Kaalcharakk.Middleware
{
    public class TokenCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Request.Cookies["accessToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Request.Headers["Authorization"] = $"Bearer {accessToken}";
            }

            await _next(context);
        }
    }

}
