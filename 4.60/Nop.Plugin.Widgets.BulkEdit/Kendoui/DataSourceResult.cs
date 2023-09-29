using System.Collections;

namespace Nop.Plugin.Widgets.BulkEdit.Kendoui
{
    public class DataSourceResult
    {
        public object ExtraData { get; set; }

        public object Errors { get; set; }

        public int Total { get; set; }

        public IEnumerable Data { get; set; }
    }
}
