using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class BlogDataTableMapping : DataTableMapping
    {
        public override string SourceTableName => "BlogDataTable";

        public BlogDataTableMapping()
        {
            Map("BlogUrl").ToDestinationColumn("Url");
        }
    }
}
