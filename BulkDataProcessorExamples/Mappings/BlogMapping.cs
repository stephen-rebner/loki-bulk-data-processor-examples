using BulkDataProcessorExamples.Models.ModelsRequiringMapping;
using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class BlogMapping : ModelMapping<BlogModelRequiringMapping>
    {
        public BlogMapping()
        {
            Map(x => x.BlogUrl).ToDestinationColumn("Url");
        }
    }
}
