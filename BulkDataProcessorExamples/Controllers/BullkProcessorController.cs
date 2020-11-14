using BulkDataProcessorExamples.EntityFramework;
using BulkDataProcessorExamples.Models;
using BulkDataProcessorExamples.Models.ModelsRequiringMapping;
using Loki.BulkDataProcessor;
using LokiBulkDataProcessorExamples.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TestBulkProcssorApi.Controllers
{
    [ApiController]
    public class BullkProcessorController : Controller
    {
        private IBulkProcessor _bulkProcessor;
        private BlogDbContext _dbContext;
        private IConfiguration _configuration;

        public BullkProcessorController(IBulkProcessor bulkProcessor, BlogDbContext dbContext, IConfiguration configuration)
        {
            _bulkProcessor = bulkProcessor;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [Route("SavePostModels")]
        [HttpPost]
        public async Task<IActionResult> SavePostsWithoutMapping(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-models-test-without-mapping");

            // Create a list of PostDtos 
            var postsDtosToSave = new List<PostDto>();

            for (var i = 1; i <= items; i++)
            {
                var post = new PostDto
                {
                    Title = $"Title{i}",
                    Content = $"Content{i}",
                    BlogId = blog.Id
                };

                postsDtosToSave.Add(post);
            }

            // Call the Bulk Processor to save the data.
            await _bulkProcessor.SaveAsync(postsDtosToSave, "Posts");

            return Ok();
        }

        [Route("SavePostModelsWithMapping")]
        [HttpPost]
        public async Task<IActionResult> SaveModelsWithMapping(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-models-test-with-mapping");

            // Create a list of Post Dtos which requires the PostDtoMapping located under mappings
            var postsDtosToSave = new List<PostDtoModelRequiringMapping>();

            for (var i = 1; i <= items; i++)
            {
                var post = new PostDtoModelRequiringMapping
                {
                    PostTitle = $"Title For Post Requiring Mapping {i}",
                    PostContent = $"Content For Post Requiring Mapping {i}",
                    SomeBlogId = blog.Id
                };

                postsDtosToSave.Add(post);
            }

            // Call the save method on the Bulk Processor 
            await _bulkProcessor.SaveAsync(postsDtosToSave, "Posts");

            return Ok();
        }

        [Route("SaveDomainModelsWithMapping")]
        [HttpPost]
        public async Task<IActionResult> SaveDomainModelsWithMapping(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-domain-models-test-without-mapping");

            // Create a list of Post Domain objects which requires the PostMapping located under mappings
            var posts = new List<Post>();

            for (var i = 1; i <= items; i++)
            {
                var post = new Post
                {
                    Title = $"Title {i}",
                    Content = $"Content {i}",
                    BlogId = blog.Id
                };

                posts.Add(post);
            }

            // Call the save method on the Bulk Processor 
            // Note: unlike entity framework, the Id will not be brought back
            // and neither will the full blog object. To do this would impact on the performance.
            await _bulkProcessor.SaveAsync(posts, "Posts");

            return Ok();
        }

        [Route("SavePostsDataTable")]
        [HttpPost]
        public async Task<IActionResult> SavePostsDataTable(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-data-table-test-without-mapping");

            // Create a data table with column names that match the column names on the database exactly
            var postsTable = new DataTable();

            postsTable.Columns.Add(new DataColumn("Title"));
            postsTable.Columns.Add(new DataColumn("Content"));
            postsTable.Columns.Add(new DataColumn("BlogId"));

            for (var i = 1; i <= items; i++)
            {
                var postRow = postsTable.NewRow();
                postRow["Title"] = $"Post Data Table Title {i}";
                postRow["Content"] = $"Post Data Table Content {i}";
                postRow["BlogId"] = blog.Id;

                postsTable.Rows.Add(postRow);
            }

            // Call the save method on the Bulk Processor 
            await _bulkProcessor.SaveAsync(postsTable, "Posts");

            return Ok();
        }

        [Route("SavePostsDataTableWithMapping")]
        [HttpPost]
        public async Task<IActionResult> SavePostsDataTableUsingMapping(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-data-table-test-with-mapping");

            // Create a posts data table, giving the table name that matches 
            // the SourceTableName property on the PostDataTableMapping class
            // (located in the mappings folder)
            var postsTable = new DataTable { TableName = "PostDataTable" } ;

            // A new columns which which the Source Columns on the 
            // PostDataTableMapping class exactly. 
            postsTable.Columns.Add(new DataColumn("PostTitle"));
            postsTable.Columns.Add(new DataColumn("PostContent"));
            postsTable.Columns.Add(new DataColumn("SomeBlogId"));

            for (var i = 1; i <= items; i++)
            {
                var postRow = postsTable.NewRow();
                postRow["PostTitle"] = $"Post Data Table Title Using Mapping {i}";
                postRow["PostContent"] = $"Post Data Table Content Using Mapping {i}";
                postRow["SomeBlogId"] = blog.Id;

                postsTable.Rows.Add(postRow);
            }

            // Call the save method on the Bulk Processor 
            await _bulkProcessor.SaveAsync(postsTable, "Posts");

            return Ok();
        }

        [Route("SavePostModelsWithTransaction")]
        [HttpPost]
        public async Task<IActionResult> SavePostsWithTransaction(int items)
        {
            // Create a Blog and save it to the db using EF
            var blog = await CreateBlog("http://blog-models-test-without-mapping");

            // Create a list of PostDtos 
            var postsDtos = new List<PostDto>();

            for (var i = 1; i <= items; i++)
            {
                var post = new PostDto
                {
                    Title = $"Title{i}",
                    Content = $"Content{i}",
                    BlogId = blog.Id
                };

                postsDtos.Add(post);
            }

            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("BlogsDb")))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    _bulkProcessor.Transaction = transaction;

                    // Call the save method on the Bulk Processor 
                    await _bulkProcessor.SaveAsync(postsDtos, "Posts");

                    //transaction.Commit();
                    transaction.Rollback();
                }
            }

            return Ok();
        }

        private async Task<Blog> CreateBlog(string url)
        {
            var blog = new Blog() { Url = url };
            _dbContext.Add(blog);
            await _dbContext.SaveChangesAsync();

            return blog;
        }
    }
}