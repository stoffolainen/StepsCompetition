using System;

namespace BlazorApp.Api.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public int Steps { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime ActivityDate { get; set; }

        public virtual User User { get; set; }
    }
}