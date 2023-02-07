using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Wexflow.BlazorServer.Controllers;

namespace Wexflow.BlazorServer.Pages
{
    public partial class History
    {
        [Inject]
        HistoryController history { get; set; }

        protected IEnumerable<Data.HistoryEntry> EntryList { get; set; }


        protected bool _visible = false;
        protected string _logDetail = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EntryList = history.SearchHistoryEntriesByPageOrderBy(new ViewModel.EntryQueryModel());
        }
        void ClearContent()
        {
            _visible = false;
            _logDetail = string.Empty;
        }

        void ShowHisLog(RowData<Data.HistoryEntry> row)
        {
            //  OnRowClick的方法，直接变更_visible，就可以
            _visible = true;
            _logDetail = history.GetHistoryEntryLogs(row.Data.Id);
            _logDetail = _logDetail.Replace("\r\n", "<br/>");
        }


    }
}
