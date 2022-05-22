using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}