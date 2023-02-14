using Wexflow.Core.Db;

namespace Wexflow.BlazorServer.ViewModel
{
    public class EntryQueryModel
    {

        public string? Keyword { get; set; }

        public DateTime From { get; set; } = DateTime.Now.AddDays(-1);
        public DateTime To { get; set; } = DateTime.Now.AddDays(1);

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public EntryOrderBy OrderField { get; set; } = EntryOrderBy.StatusDateDescending;
    }
}
