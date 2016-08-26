using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PosteroOrg.Mural.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset DtNote { get; set; }
        public string PureContent { get; set; }

        public string Username { get; set; }
        public virtual User User { get; set; }
    }
}