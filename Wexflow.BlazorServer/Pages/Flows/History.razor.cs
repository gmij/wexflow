using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Wexflow.BlazorServer.Controllers;
using Wexflow.BlazorServer.ViewModel;

namespace Wexflow.BlazorServer.Pages.Flows
{
    public partial class History
    {
        [Inject]
        HistoryController history { get; set; }

        RangePicker<DateTime[]> searchDateRange { get; set; }

        protected IEnumerable<Data.HistoryEntry> EntryList { get; set; }

        protected int _count = 0;

        protected bool _visible = false;
        protected string _logDetail = string.Empty;

        protected DateTime[] rangeTimes { get; set; } = Array.Empty<DateTime>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            rangeTimes = new[] {history.GetHistoryEntryStatusDateMin(), history.GetHistoryEntryStatusDateMax()};

            var queryModel = new EntryQueryModel() { From = rangeTimes[0], To = rangeTimes[1] };
            BindTable(queryModel);
        }
        void ClearContent()
        {
            _visible = false;
            _logDetail = string.Empty;
        }

        void ShowHisLog(string id)
        {
            //  OnRowClick的方法，直接变更_visible，就可以
            _visible = true;
            _logDetail = history.GetHistoryEntryLogs(id);
            _logDetail = _logDetail.Replace("\r\n", "<br/>");
        }

        protected void SearchHistory()
        {
            var queryModel = new EntryQueryModel() { From = rangeTimes[0], To = rangeTimes[1] };
            BindTable(queryModel);
        }

        protected void OnPageIndexChange(PaginationEventArgs args)
        {
            var queryModel = new EntryQueryModel() { From = rangeTimes[0], To = rangeTimes[1], Page= args.Page,  PageSize= args.PageSize };
            BindTable(queryModel);
        }

        private void BindTable(EntryQueryModel queryModel)
        {
            EntryList = history.SearchHistoryEntriesByPageOrderBy(queryModel);
            _count = (int)history.GetHistoryEntriesCountByDate(queryModel);
            StateHasChanged();
        }


    }
}
