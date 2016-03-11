using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PosteroCompany.Mural.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        public DateTime DtNote { get; set; }
        public string PureContent { get; set; }

        public string Username { get; set; }
        public virtual User User { get; set; }
    }
}