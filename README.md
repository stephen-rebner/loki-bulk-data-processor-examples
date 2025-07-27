# loki-bulk-data-processor-examples

This is a sample Asp.Net Core Web API solution, demonstrating how to use Loki Bulk Data Processor.

In order to to run the API, the database needs to be created first using Entity Framework. To do this open up
the package manger console and type the following command:

**Update-Database**

The easiest way to execute the Web API examples is to download Postman from https://www.postman.com/.

Then import the LokiTests.postman_collection.json file by clicking on file > import in postman.

This will import a collection of http requests that can be executed against this test Web API.

## Using the JSON Stream API

The application includes a `SavePostsUsingJsonStream` endpoint that demonstrates how to use Loki's JSON stream processing capabilities. This endpoint:

- Accepts a single `items` parameter that determines how many post records to generate
- Creates a JSON stream with the proper format required by the BulkProcessor
- Uses the `SaveAsync(Stream)` method to process the JSON data

## JSON Format

The JSON format expected by the BulkProcessor follows this structure:
```json
{
    "tableName": "TableName",
    "columns": [
        { "name": "Column1", "type": "string" },
        { "name": "Column2", "type": "int" }
    ],
    "data": [
        { "Column1": "Value1", "Column2": 123 },
        { "Column1": "Value2", "Column2": 456 }
    ]
}
```

## API Port Configuration

The application uses the following default ports:

- HTTPS: 44306
- HTTP: 50709

If you encounter port conflicts, you can modify these values in the Properties/launchSettings.json file.

## Logging Configuration

This demo application implements file-based logging for the Loki Bulk Data Processor.

``` c#
// Standalone logger factory approach
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddFile("Logs/bulkprocessor-{Date}.txt", fileSizeLimitBytes: 10 * 1024 * 1024);
});

// Pass logger factory to bulk data processor
services.AddLokiBulkDataProcessor(
    Configuration.GetConnectionString("BlogsDb"),
    Assembly.GetExecutingAssembly(),
    loggerFactory);
```
