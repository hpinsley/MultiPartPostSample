using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiPartServer.Models;

namespace MultiPartServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiController : ControllerBase
    {
        // GET api/values
        [HttpGet("names")]
        public ActionResult<string> Get()
        {
            return "Howard";
        }

        /// <summary>
        /// Dummy method that takes two parameters (and fundId and a regressionMode)
        /// and also expects a file upload.  See the test project for a sample
        /// post which sends the data as multi-part form data post.
        /// </summary>
        /// <returns>A sample dummy DTO</returns>
        [HttpPost("returns")]
        public ActionResult<ReturnModel> SaveReturns()
        {
            Console.WriteLine($"Request content type: {this.Request.ContentType}");

            var form = this.Request.Form;

            var fundId = form["fundId"];
            var regresssionMode = form["regressionMode"];
            var file = form.Files.Single();

            Console.WriteLine($"FundId: {fundId}");
            Console.WriteLine($"RegressionMode: {regresssionMode}");
            Console.WriteLine($"FILE: Name: {file.Name}\nFilename: {file.FileName}\nLength: {file.Length}\nContent Type: {file.ContentType}\nContent disposition: {file.ContentDisposition}");

            var outputFile = $"/Users/howard.pinsley/dev/dotnet/multi-part-body/MultiPartServer.Tests/data/output-{file.FileName}";
            Console.WriteLine($"Saving to {outputFile}");

            using (var outfile = System.IO.File.Create(outputFile)) {
                file.CopyTo(outfile);
            }

            var result = new ReturnModel
            {
                OutputFile = outputFile,
                Comments = "It seems to work"
            };

            return result;
        }
    }
}