using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string PageId { get; set; }
        public string Img { get; set; }
        public string ArtistName { get; set; }
        public string Title { get; set; }
        public string Technique { get; set; }
        public string Size { get; set; }
        public string DateYear { get; set; }
        public string Signature { get; set; }
        public int UserId { get; set; }
        public string PageKey { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }
        public int ExhibitionId { get; set; }
        public string ExhibitionName { get; set; }

    }
}