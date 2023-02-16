using AntDesign;
using Microsoft.AspNetCore.Components;
using OneOf.Types;
using Wexflow.BlazorServer.Controllers;
using Wexflow.BlazorServer.Data;
using Wexflow.BlazorServer.Helper;

namespace Wexflow.BlazorServer.Pages.Flows
{
    public partial class Job
    {

        [Inject]
        EntryController manager { get; set; }

        [Inject]
        NotifyHelper _notice { get; set; }

        ITable? table;


        protected bool loading { get; private set; } = true;

        protected IEnumerable<Entry> entryList { get; set; }

        private Timer _timer;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            entryList = manager.Jobs();
            loading = false;
        }

        async void StopEntry(string instId, int wfId)
        {
            var result = await manager.Stop(wfId, Guid.Parse(instId));
            _notice.NotifySuccessOrFail(result, $"成功停止流程：{instId}", $"停止流程失败：{instId}");
           
            StateHasChanged();
        }

        protected bool IsApproval(int wfId)
        {
            return manager.IsApproval(wfId);
        }

        protected async void Accept(string instId, int wfId)
        {
            var r = await manager.Accept(wfId, Guid.Parse(instId));
            _notice.NotifySuccessOrFail(r, $"流程审批通过：{instId}", $"流程审批未通过：{instId}");
            if (r)
                BindData();
        }

        protected async void Reject(string instId, int wfId)
        {
            var r = await manager.Reject(wfId, Guid.Parse(instId));
            _notice.NotifySuccessOrFail(r, $"流程已拒绝：{instId}", $"流程拒绝失败：{instId}");
            if (r)
                BindData();
        }

        void BindData()
        {
            _timer = new Timer( _ =>
            {
                InvokeAsync( () =>
                {
                    entryList =  manager.Jobs();
                    StateHasChanged();
                });
                
            }, null, 3000, -1);
        }

    }
}
