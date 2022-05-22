using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        [Required]
        public string NameSurname { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool ConfirmEmail { get; set; }
        public bool ConfirmPhone { get; set; }
        public string DialCode { get; set; }
        public string TaxAdministration { get; set; }
        public string TaxNumber { get; set; }
        public string UserKey { get; set; }
        public string DialCodeIso { get; set; }
        public virtual bool RememberMe { get; set; }
        public virtual string PhoneConfirmCode { get; set; }
    }
}