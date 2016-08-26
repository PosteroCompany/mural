using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PosteroOrg.Mural.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        [MaxLength(64)]
        public string Password { get; set; }
        public DateTimeOffset DtRegister { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}