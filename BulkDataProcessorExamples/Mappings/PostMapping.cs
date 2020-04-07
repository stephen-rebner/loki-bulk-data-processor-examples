using BulkDataProcessorExamples.Models;
using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class PostMapping : ModelMapping<Post>
    {
        public PostMapping()
        {
            Map(post => post.Title).ToDestinationColumn("Title");
            Map(post => post.Content).ToDestinationColumn("Content");
            Map(post => post.BlogId).ToDestinationColumn("BlogId");
        }
    }
}
