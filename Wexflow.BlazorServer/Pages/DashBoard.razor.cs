using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Wexflow.BlazorServer.Controllers;

namespace Wexflow.BlazorServer.Pages
{
    public partial class DashBoard 
    {
        [Inject]
        DashboardController Dashboard { get; set; }

        protected Data.StatusCount FlowExecCodes { get; private set; }

        protected IEnumerable<Data.Entry> EntryList { get; set; }

        protected bool _visible = false;
        protected string _logDetail = string.Empty;



        protected override void OnInitialized()
        {
            base.OnInitialized();
            FlowExecCodes = Dashboard.GetStatusCount();
            EntryList = Dashboard.SearchEntriesByPageOrderBy(new ViewModel.EntryQueryModel());
        }

        void ClearContent()
        {
            _visible = false;
            _logDetail = string.Empty;
        }

        void ShowLog(RowData<Data.Entry> row)
        {
            //  OnRowClick的方法，直接变更_visible，就可以
            _visible = true;
            _logDetail = Dashboard.GetEntryLogs(row.Data.Id);
            _logDetail = _logDetail.Replace("\r\n", "<br/>");
        }


        Dictionary<string, object> SetRowEvent(RowData<Data.Entry> row) {
            return new()
            {
                ["ondblclick"] = () => {
                    //  OnRow的方法，直接变更_visible，不行，需要追加调用StateHasChanged
                    _visible = true;
                    _logDetail = Dashboard.GetEntryLogs(row.Data.Id);
                    _logDetail = _logDetail.Replace("\r\n", "<br/>");
                    Console.WriteLine(row.Data.Id, _logDetail);
                    StateHasChanged();
                }
            };
        }

    }
}
