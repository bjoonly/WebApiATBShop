using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AppUser : IdentityUser<long>
    {
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string SecondName { get; set; }
        [StringLength(100)]
        public string Photo { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
