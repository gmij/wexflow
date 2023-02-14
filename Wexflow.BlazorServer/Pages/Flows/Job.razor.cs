using Microsoft.AspNetCore.Components;
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


        protected bool loading { get; private set; } = true;

        protected IEnumerable<Entry> entryList { get; set; }

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

    }
}
