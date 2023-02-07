using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Wexflow.BlazorServer.Controllers;
using Wexflow.BlazorServer.Data;

namespace Wexflow.BlazorServer.Pages
{
    public partial class Manager
    {

        [Inject]
        ManagerController manager { get; set; }

        protected bool loading { get; private set; } = true;

        protected IEnumerable<WorkflowInfo>? workflowList { get; set; }

        protected IEnumerable<WorkflowInfo>? selectWfItem { get; set; }

        protected ITable table;


        protected async override void OnInitialized()
        {
            base.OnInitialized();
            workflowList = await manager.Search();
            loading = false;
            
            StateHasChanged();
            if (workflowList.Any())
            {
                table.SetSelection(new[] { workflowList.First().Id.ToString() });
            }
        }


        void SelectRow(RowData<WorkflowInfo> row)
        {
            table.SetSelection(new[] { row.Data.Id.ToString() });
            Console.WriteLine(row.Data.Id);
        }

        async void StartWf()
        {
            if (selectWfItem.Any())
            {
                var wfId = selectWfItem.First().Id;
                await manager.Start(wfId);
            }
        }

    }
}
