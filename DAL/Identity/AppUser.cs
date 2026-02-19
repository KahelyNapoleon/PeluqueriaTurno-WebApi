using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    public class AppUser : IdentityUser
    {
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }    
    }
}
