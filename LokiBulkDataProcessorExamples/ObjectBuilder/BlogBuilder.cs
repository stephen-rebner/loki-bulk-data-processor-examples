using LokiBulkDataProcessorExamples.Models;

namespace LokiBulkDataProcessorExamples.ObjectBuilder
{
    public class BlogBuilder
    {
        private Blog _blog;

        public BlogBuilder CreateBlog()
        {
            _blog = new Blog();
            return this;
        }

        public BlogBuilder WithUrl(string url)
        {
            _blog.Url = url;
            return this;
        }

        public Blog Build()
        {
            return _blog;
        }
    }
}
