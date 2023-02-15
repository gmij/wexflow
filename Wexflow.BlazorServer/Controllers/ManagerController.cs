using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Wexflow.BlazorServer.Data;
using Wexflow.Core.Db;

namespace Wexflow.BlazorServer.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ManagerController : WexflowControllerBase
    {
        private readonly ILocalStorageService storageService;
        private readonly AuthenticationStateProvider stateProvider;

        public ManagerController(WexflowService service, ILocalStorageService storageService, AuthenticationStateProvider stateProvider, IServiceProvider sp, IMapper mapper) : base(service, sp, mapper)
        {
            this.storageService = storageService;
            this.stateProvider = stateProvider;
        }

        [HttpGet("search")]

        public async Task<IEnumerable<WorkflowInfo>> Search(string keyword = "")
        {
            string keywordToUpper = keyword.ToUpper();

            IEnumerable<WorkflowInfo> workflows = null;


            var user = await this.CurrentUser();

            //  todo. 当前用户为空，应该要踢回登录页面。
            if (user == null)
                throw new ArgumentNullException(nameof(user), "请登录");

            if (user.UserProfile == Data.UserProfile.SuperAdministrator)
            {
                workflows = _service.Engine.Workflows
                    .ToList()
                    .Where(wf =>
                            wf.Name.ToUpper().Contains(keywordToUpper)
                            || wf.Description.ToUpper().Contains(keywordToUpper)
                            || wf.Id.ToString().Contains(keywordToUpper))
                    .Select(wf => new WorkflowInfo(wf.DbId, wf.Id, wf.InstanceId, wf.Name, wf.FilePath,
                        (Data.LaunchType)wf.LaunchType, wf.IsEnabled, wf.IsApproval, wf.EnableParallelJobs, wf.IsWaitingForApproval, wf.Description, wf.IsRunning, wf.IsPaused,
                        wf.Period.ToString(@"dd\.hh\:mm\:ss"), wf.CronExpression,
                    wf.IsExecutionGraphEmpty
                        , wf.LocalVariables.Select(v => new Variable { Key = v.Key, Value = v.Value }).ToArray()
                        , wf.StartedOn.ToString(_service.Config["DateTimeFormat"]))).ToArray();
                    ;
            }
            else if (user.UserProfile == Data.UserProfile.Administrator)
            {
                //  todo: 要把当前用户的getdbid转换成真实数据，不能使用-1这个常量
                workflows = _service.Engine.GetUserWorkflows(user.GetDbId())
                                        .ToList()
                                        .Where(wf =>
                                            wf.Name.ToUpper().Contains(keywordToUpper)
                                            || wf.Description.ToUpper().Contains(keywordToUpper)
                                            || wf.Id.ToString().Contains(keywordToUpper))
                                        .Select(wf => new WorkflowInfo(wf.DbId, wf.Id, wf.InstanceId, wf.Name, wf.FilePath,
                                            (Data.LaunchType)wf.LaunchType, wf.IsEnabled, wf.IsApproval, wf.EnableParallelJobs, wf.IsWaitingForApproval, wf.Description, wf.IsRunning, wf.IsPaused,
                                            wf.Period.ToString(@"dd\.hh\:mm\:ss"), wf.CronExpression,
                                        wf.IsExecutionGraphEmpty
                                            , wf.LocalVariables.Select(v => new Variable { Key = v.Key, Value = v.Value }).ToArray()
                                            , wf.StartedOn.ToString(_service.Config["DateTimeFormat"]))).ToArray()
                                        ;
            }

            return workflows?.OrderBy(o => o.Id);

        }

        [HttpPost("start")]
        public async Task<Guid> Start([FromQuery]int w)
        {
            var user = await this.CurrentUser();
            var username = user.Username;
            var workflowId = w;

            if (user.UserProfile == Data.UserProfile.SuperAdministrator)
            {
                return _service.Engine.StartWorkflow(username, workflowId);
            }
            else if (user.UserProfile == Data.UserProfile.Administrator)
            {
                var workflowDbId = _service.Engine.Workflows.First(w => w.Id == workflowId).DbId;
                var check = _service.Engine.CheckUserWorkflow(user.GetDbId(), workflowDbId);
                if (check)
                {
                    return _service.Engine.StartWorkflow(username, workflowId);
                }
            }
            return Guid.Empty;
        }
    }
}
