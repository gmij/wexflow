using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Wexflow.BlazorServer.Controllers;
using Wexflow.BlazorServer.Data;
using Wexflow.BlazorServer.Helper;

namespace Wexflow.BlazorServer.Pages.Flows
{
    public partial class Manager
    {

        [Inject]
        ManagerController manager { get; set; }

        [Inject]
        NotifyHelper _notice { get; set; }

        protected bool loading { get; private set; } = true;

        protected string searchString = string.Empty;

        protected IEnumerable<WorkflowInfo> workflowList { get; set; }

        protected IEnumerable<WorkflowInfo> selectWfItem { get; set; }

        protected ITable? table;


        protected async override void OnInitialized()
        {
            base.OnInitialized();
            workflowList = await manager.Search();
            loading = false;

            StateHasChanged();
        }

        async void OnChange(QueryModel<WorkflowInfo> query)
        {
            loading = true;
            workflowList = await manager.Search(searchString ?? "");
            loading = false;
            StateHasChanged();
        }

        async void StartWf(int wfId)
        {
            var instId = await manager.Start(wfId);
            _notice.Success($"已触发流程:{instId}");
        }

        void OpenDesign(int wfId)
        {
            _notice.Warning($"暂未实现: 参数={wfId}");
        }

    }
}
