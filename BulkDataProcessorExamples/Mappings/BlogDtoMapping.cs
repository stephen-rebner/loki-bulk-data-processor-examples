using BulkDataProcessorExamples.Models.ModelsRequiringMapping;
using Loki.BulkDataProcessor.Mappings;

namespace BulkDataProcessorExamples.Mappings
{
    public class BlogDtoMapping : ModelMapping<BlogDtoModelRequiringMapping>
    {
        public BlogDtoMapping()
        {
            Map(x => x.BlogUrl).ToDestinationColumn("Url");
        }
    }
}
