using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class PostDataTableMapping : DataMapping
    {
        public override string SourceTableName => "PostDataTable";

        public PostDataTableMapping()
        {
            Map("PostTitle").ToDestinationColumn("Title");
            Map("PostContent").ToDestinationColumn("Content");
            Map("SomeBlogId").ToDestinationColumn("BlogId");
        }
    }
}
