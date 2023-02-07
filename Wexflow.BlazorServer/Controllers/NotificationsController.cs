using Microsoft.AspNetCore.Mvc;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class NotificationsController : WexflowControllerBase
    {
        public NotificationsController(WexflowService service) : base(service)
        {
        }


        [HttpGet("hasNotifications")]
        public void HasNotifications()
        {
            var context = this.HttpContext;
            var assignedToUsername = context.Request.Query["a"].ToString();
            Core.Db.User assignedTo = _service.Engine.GetUser(assignedToUsername);

            var res = false;
            var auth = context.Items["User"] as Auth;
            if (auth != null)
            {
                var username = auth.Username;
                var password = auth.Password;

                var user = _service.Engine.GetUser(username);
                //  todo:   这个位置的权限判断，后续可以移到外面，通过统一鉴权来处理。
                if (user.Password.Equals(password) && (user.UserProfile == Core.Db.UserProfile.SuperAdministrator || user.UserProfile == Core.Db.UserProfile.Administrator))
                {
                    res = _service.Engine.HasNotifications(assignedTo.GetDbId());
                }
            }
        }
    }
}
