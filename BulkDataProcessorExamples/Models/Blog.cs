using System.Collections.Generic;

namespace BulkDataProcessorExamples.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }
}
