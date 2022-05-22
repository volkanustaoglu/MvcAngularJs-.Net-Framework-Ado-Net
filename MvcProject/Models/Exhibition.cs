using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
        public int UserId { get; set; }
    }
}