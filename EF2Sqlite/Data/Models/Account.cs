using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Account
    {
        [Key]
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
