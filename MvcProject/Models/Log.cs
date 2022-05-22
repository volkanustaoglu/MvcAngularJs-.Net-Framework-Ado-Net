using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }
        public DateTime CreateDate { get; set; }
        public string Username { get; set; }
    }
}