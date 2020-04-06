using Loki.BulkDataProcessor;
using LokiBulkDataProcessorExamples.Models;
using LokiBulkDataProcessorExamples.ObjectBuilder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TestModel;
using TestObjectBuilders;

namespace TestBulkProcssorApi.Controllers
{
    [ApiController]
    public class BullkProcessorController : Controller
    {
        private IBulkProcessor _bulkProcessor;
        private BlogBuilder _blogBuilder;
        private PostBuilder _postBuilder;

        public BullkProcessorController(IBulkProcessor bulkProcessor)
        {
            _bulkProcessor = bulkProcessor;
            _blogBuilder = new BlogBuilder();
            _postBuilder = new PostBuilder();
        }

        [Route("SavePosts")]
        [HttpPost]
        public async Task<IActionResult> Insert(int items)
        {
             var blog = _blogBuilder.CreateBlog()
                .WithUrl("http://")
                .Build();

            await _bulkProcessor.SaveAsync(new [] { blog }, "Blogs");

            var postsToSave = new List<Post>();

            for (var i = 1; i <= items; i++)
            {
                var post = _postBuilder.CreatePost()
                    .WithTitle($"Title{i}")
                    .WithContent($"Content{i}")
                    .WithBlogId(1)
                    .Build();

                postsToSave.Add(post);
            }

            await _bulkProcessor
                .WithConnectionString("Server=(local);Database=IntegrationTestsDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .SaveAsync(postsToSave, "Posts");

            return Ok();
        }


        [Route("SavePosts")]
        [HttpPost]
        public async Task<IActionResult> SavePosts(int items)
        {

            var blogTable = new DataTable();

            blogTable.Columns.Add(new DataColumn("Url"));

            var blogRow = blogTable.NewRow();
            blogRow["Url"] = "A Url";
            blogTable.Rows.Add(blogRow);

            await _bulkProcessor.SaveAsync(blogTable, "Blogs");

            var postsTable = new DataTable();

            postsTable.Columns.Add(new DataColumn("Title"));
            postsTable.Columns.Add(new DataColumn("Content"));
            postsTable.Columns.Add(new DataColumn("BlogId"));

            for (var i = 1; i <= items; i++)
            {
                var postRow = postsTable.NewRow();
                postRow["Title"] = $"Title {i}";
                postRow["Content"] = $"Content {i}";
                postRow["BlogId"] = 1;

                postsTable.Rows.Add(postRow);
            }

            await _bulkProcessor.SaveAsync(postsTable, "Posts");

            return Ok();
        }

        [Route("IncorrectColumnTest")]
        [HttpPost]
        public async Task<IActionResult> IncorrectMappingTest(int items)
        {
            var blogTable = new DataTable();

            blogTable.Columns.Add(new DataColumn("Url"));

            var blogRow = blogTable.NewRow();
            blogRow["Url"] = "A Url";
            blogTable.Rows.Add(blogRow);

            await _bulkProcessor.SaveAsync(blogTable, "Blogs");

            var postsTable = new DataTable();

            postsTable.Columns.Add(new DataColumn("title"));
            postsTable.Columns.Add(new DataColumn("Content"));
            postsTable.Columns.Add(new DataColumn("BlogId"));

            for (var i = 1; i <= items; i++)
            {
                var postRow = postsTable.NewRow();
                postRow["title"] = $"Title {i}";
                postRow["Content"] = $"Content {i}";
                postRow["BlogId"] = 1;

                postsTable.Rows.Add(postRow);
            }

            try
            {
                await _bulkProcessor.SaveAsync(postsTable, "Posts");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}