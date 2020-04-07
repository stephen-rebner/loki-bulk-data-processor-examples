using LokiBulkDataProcessorExamples.Models;

namespace LokiBulkDataProcessorExamples.ObjectBuilder
{
    public class PostDtoBuilder
    {
        private PostDto _post;

        public PostDtoBuilder CreatePost()
        {
            _post = new PostDto();
            return this;
        }

        public PostDtoBuilder WithTitle(string title)
        {
            _post.Title = title;
            return this;
        }

        public PostDtoBuilder WithContent(string content)
        {
            _post.Content = content;
            return this;
        }

        public PostDtoBuilder WithBlogId(int blogId)
        {
            _post.BlogId = blogId;
            return this;
        }

        public PostDto Build()
        {
            return _post;
        }
    }
}
