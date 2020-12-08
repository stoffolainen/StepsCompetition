using System;
using System.Collections.Generic;

namespace BlazorApp.Api.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}