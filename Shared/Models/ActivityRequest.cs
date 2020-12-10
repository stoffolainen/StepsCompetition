using System;

namespace BlazorApp.Shared.Models
{
    public class ActivityRequest
    {
        public int Steps { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Email { get; set; }
    }
}