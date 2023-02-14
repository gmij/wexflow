using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wexflow.BlazorServer.Authentication;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Controllers
{
    public class WexflowControllerBase : ControllerBase
    {

        protected readonly WexflowService _service;
        protected readonly IServiceProvider sp;
        protected readonly IMapper mapper;

        public WexflowControllerBase(WexflowService service)
        {
            _service = service;
        }

        public WexflowControllerBase(WexflowService service, IServiceProvider sp, IMapper mapper) : this(service)
        {
            this.sp = sp;
            this.mapper = mapper;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<User> CurrentUser()
        {
            var asp = sp.GetRequiredService<AuthenticationStateProvider>();
            if (asp != null)
            {
                var state = await asp.GetAuthenticationStateAsync();
                var username = state.User.Identity?.Name;
                var user = _service.Engine.GetUser(username);
                
                
                return mapper.Map<User>(user);
            }
            else
            {
                return new();
            }
        }

    }
}
