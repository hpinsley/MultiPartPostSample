using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MultiPartServer.Tests
{
    [TestClass]
    public class PostTests
    {
        HttpClient client;

        [TestInitialize]
        public void TestInit() {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("http://localhost:5000/");
        }

        [TestMethod]
        public void CanMakeSimpleRequest()
        {
            var result = client.GetStringAsync("/api/multi/names").Result;
            Assert.AreEqual("\"Howard\"", result);
        }

        [TestMethod]
        public void CanMakeSimplePost()
        {
            var filename = "/Users/howard.pinsley/dev/dotnet/multi-part-body/MultiPartServer.Tests/data/sample.zip";

            using (var stm = File.OpenRead(filename)) {

                var fileContent = new StreamContent(stm)
                {
                    Headers =
                    {
                        ContentLength = stm.Length,
                        ContentType = new MediaTypeHeaderValue("application/zip")
                    }
                };

                // Send two pieces of data and a file (zip content tpe)
                var formDataContent = new MultipartFormDataContent();

                formDataContent.Add(new StringContent("15"), "fundId");
                formDataContent.Add(new StringContent("linear"), "regressionMode");
                formDataContent.Add(fileContent, "datafile", "sample.zip");          // file

                var result = client.PostAsync("/api/multi/returns", formDataContent).Result;
                Assert.IsNotNull(result);
                Console.WriteLine(result);
            }
        }
    }
}
