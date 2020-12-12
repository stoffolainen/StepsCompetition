using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorApp.Api.Data;
using BlazorApp.Api.Entities;
using BlazorApp.Shared.Models;
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

        [FunctionName("PostActivity")]
        public async Task<IActionResult> PostActivityAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            CancellationToken cts,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ActivityRequest>(requestBody);

            var user = await dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Email.ToLower() == data.Email.ToLower(), cts);
            if (user == null)
            {
                user = new User
                {
                    Created = DateTime.Now,
                    Email = data.Email
                };

                await dbContext.User.AddAsync(user, cts);
                await dbContext.SaveChangesAsync(cts);
            }

            var activity = new Activity
            {
                ActivityDate = data.ActivityDate,
                Created = DateTime.Now,
                UserId = user.UserId,
                Steps = data.Steps
            };

            await dbContext.Activity.AddAsync(activity, cts);
            await dbContext.SaveChangesAsync(cts);

            return new OkObjectResult(await GetActivityDataAsync(cts));
        }

        [FunctionName("GetActivityData")]
        public async Task<IActionResult> GetActivityDataAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            CancellationToken cts,
            ILogger log) => new OkObjectResult(await GetActivityDataAsync(cts));

        private async Task<ActivityData> GetActivityDataAsync(CancellationToken cts)
        {
            var numberOfDays = (DateTime.Today - new DateTime(2020, 12, 1)).TotalDays;

            var activities = await dbContext.Activity.AsNoTracking().ToListAsync(cts);
            var totalSteps = activities?.Sum(x => x.Steps) ?? 0;
            var totalUsers = await dbContext.User.CountAsync(cts);

            return new ActivityData
            {
                TotalSteps = totalSteps,
                AvgGroupSteps = totalSteps == 0 ? 0 : totalSteps / numberOfDays,
                AvgPersonSteps = totalSteps == 0 || totalUsers == 0 ? 0 : (totalSteps / numberOfDays) / totalUsers
            };
        }
    }
}
