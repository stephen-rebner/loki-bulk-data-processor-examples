//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Loki.BulkDataProcessor;
//using TestModel;
//using TestObjectBuilders;

//namespace TestBlkDataProcessorApp
//{
//    class Program
//    {
//        static async Task Main(string[] args)
//        {
//            //var bulkCopy = new BulkProcessor("Server=(local);Database=IntegrationTestsDb;Trusted_Connection=True;MultipleActiveResultSets=true");
//            var objectBuilder = new TestDbModelObjectBuilder();

//            var modelObject1 = objectBuilder
//                .WithId(0)
//                .WithBoolColumnValue(true)
//                .WithDateColumnValue(DateTime.Now)
//                .WithNullableBoolColumnValue(false)
//                .WithNullableDateColumnValue(DateTime.Now.AddDays(1))
//                .Build();

//            var modelObject2 = objectBuilder
//                .WithId(0)
//                .WithBoolColumnValue(false)
//                .WithDateColumnValue(DateTime.Now.AddDays(1))
//                .WithNullableBoolColumnValue(true)
//                .WithNullableDateColumnValue(DateTime.Now.AddDays(2))
//                .Build();

//            var models = new List<TestDbModel> { modelObject1, modelObject2 };

//            await bulkCopy.SaveAsync(models, "TestDbModels");
//        }

//    }
//}
