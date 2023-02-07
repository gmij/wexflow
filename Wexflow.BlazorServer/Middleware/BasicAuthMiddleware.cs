using System.Text;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _enable;
        private int _failStatusCode = 401;

        public BasicAuthMiddleware(RequestDelegate next, bool enable)
        {
            
            _next = next;
            _enable = enable;
        }

        public async Task Invoke(HttpContext context, WexflowService service)
        {
            try
            {
                if (_enable)
                {
                    var auth = GetAuth(context.Request);
                    var username = auth.Username;
                    var password = auth.Password;

                    var user = service.Engine.GetUser(username);
                    if (user.Password.Equals(password))
                    {
                        context.Items["User"] = user;

                        await _next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = _failStatusCode;
                    }
                }
                else
                {
#if DEBUG
                    var testAdmin = service.Engine.GetUser("admin");
                    context.Items["User"] = testAdmin;
#endif
                    await _next(context);
                }
            }
            catch
            {
                context.Response.StatusCode = _failStatusCode;
            }

        }

        private string DecodeBase64(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        private Auth GetAuth(HttpRequest request)
        {
            var auth = request.Headers["Authorization"].First();
            if (auth == null)
                throw new ArgumentException("Headers.Authorization");
            auth = auth.Replace("Basic ", string.Empty);
            auth = DecodeBase64(auth);
            var authParts = auth.Split(':');
            var username = authParts[0];
            var password = authParts[1];
            return new Auth { Username = username, Password = password };
        }
    }
}
