using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wexflow.BlazorServer.Authentication;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : WexflowControllerBase
    {
        private readonly CustomAuthenticationStateProvider stateProvider;
        private readonly IMapper mapper;

        public UserController(WexflowService service, AuthenticationStateProvider stateProvider, IMapper mapper) : base(service)
        {
            this.stateProvider = (CustomAuthenticationStateProvider)stateProvider;
            this.mapper = mapper;
        }


        private User GetUserByLoginId(string loginId)
        {
            var u = this._service.Engine.GetUser(loginId);
            return mapper.Map<User>(u);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void Login()
        {
            stateProvider.Login();
        }

        [ApiExplorerSettings(IgnoreApi = true)]

        public void Logout() 
        {
            stateProvider.Logout();
        }

    }
}
