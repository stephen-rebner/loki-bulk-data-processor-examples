using BulkDataProcessorExamples.Models.ModelsRequiringMapping;
using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class PostMapping : ModelMapping<PostModelRequiringMapping>
    {
        public PostMapping()
        {
            Map(x => x.PostTitle).ToDestinationColumn("Title");
            Map(x => x.PostContent).ToDestinationColumn("Content");
            Map(x => x.SomeBlogId).ToDestinationColumn("BlogId");
        }
    }
}
