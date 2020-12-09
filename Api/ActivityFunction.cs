using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlazorApp.Api.Data;
using BlazorApp.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorApp.Api
{
    public class ActivityFunction
    {
        private readonly ApplicationDbContext dbContext;

        public ActivityFunction(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [FunctionName("PostUser")]
        public async Task<IActionResult> PostUserAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            CancellationToken cts,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<User>(requestBody);

            var entity = await dbContext.User.AddAsync(data, cts);
            await dbContext.SaveChangesAsync(cts);

            return new OkObjectResult(JsonConvert.SerializeObject(entity.Entity.Name));
        }

        [FunctionName("GetUser")]
        public async Task<IActionResult> GetUserAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{email}")] HttpRequest req,
            string email,
            CancellationToken cts,
            ILogger log)
        {
            var user = await dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cts);
            if (user != null)
            {
                return new OkObjectResult(JsonConvert.SerializeObject(user.Name));
            }

            return new NotFoundResult();
        }
    }
}
