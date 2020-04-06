using LokiBulkDataProcessorExamples.Models;

namespace LokiBulkDataProcessorExamples.ObjectBuilder
{
    public class PostBuilder
    {
        private Post _post;

        public PostBuilder CreatePost()
        {
            _post = new Post();
            return this;
        }

        public PostBuilder WithTitle(string title)
        {
            _post.Title = title;
            return this;
        }

        public PostBuilder WithContent(string content)
        {
            _post.Content = content;
            return this;
        }

        public PostBuilder WithBlogId(int blogId)
        {
            _post.BlogId = blogId;
            return this;
        }

        public Post Build()
        {
            return _post;
        }
    }
}
