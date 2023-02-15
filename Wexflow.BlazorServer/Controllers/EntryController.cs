using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntryController : WexflowControllerBase
    {
        public EntryController(WexflowService service, IServiceProvider sp, IMapper mapper) : base(service, sp, mapper)
        {
        }

        [HttpGet("jobs")]
        public IEnumerable<Entry> Jobs()
        {
            var data = _service.Engine.GetEntries().Where(e => e.Status == Core.Db.Status.Running || e.Status == Core.Db.Status.Pending);
            return mapper.Map<IEnumerable<Entry>>(data);
        }


        [HttpPost("stop")]
        public async Task<bool> Stop([FromQuery(Name ="w")]int wfId, [FromQuery(Name ="i")]Guid instId)
        {
            var user = await this.CurrentUser();
            var username = user.Username;
            var result = false;
            if (user.UserProfile == UserProfile.SuperAdministrator)
            {
                result = _service.Engine.StopWorkflow(wfId, instId, username);
            }
            else if (user.UserProfile == UserProfile.Administrator)
            {
                var workflowDbId = _service.Engine.Workflows.First(w => w.Id == wfId).DbId;
                var check = _service.Engine.CheckUserWorkflow(user.GetDbId(), workflowDbId);
                if (check)
                {
                    result = _service.Engine.StopWorkflow(wfId, instId, username);
                }
            }
            return result;
        }

        [HttpGet("IsApproval")]
        public bool IsApproval(int wfId)
        {
            if (wfId == 0) return false;
            var wf = _service.Engine.GetWorkflow(wfId);
            return wf.IsApproval;
        }

        [HttpPost("accept")]
        [HttpPost("approve")]
        public async Task<bool> Accept([FromQuery(Name = "w")] int wfId, [FromQuery(Name = "i")] Guid instId)
        {
            return await Approval(wfId, instId, true);
        }

        [HttpPost("reject")]
        public async Task<bool> Reject([FromQuery(Name = "w")] int wfId, [FromQuery(Name = "i")] Guid instId)
        {
            return await Approval(wfId, instId, false);
        }

        private async Task<bool> Approval(int wfId, Guid instId, bool isaccept)
        {
            var user = await this.CurrentUser();
            var username = user.Username;
            bool res = false;
            Func<int, Guid, string, bool> func = isaccept ? _service.Engine.ApproveWorkflow : _service.Engine.RejectWorkflow;

            if (user.UserProfile == UserProfile.SuperAdministrator)
            {
                res = func(wfId, instId, username);
            }
            else if (user.UserProfile == UserProfile.Administrator)
            {
                var workflowDbId = _service.Engine.Workflows.First(w => w.Id == wfId).DbId;
                var check = _service.Engine.CheckUserWorkflow(user.GetDbId(), workflowDbId);
                if (check)
                {
                    res = func(wfId, instId, username);
                }
            }

            return res;
        }

    }
}
