using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bolnica.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BolnicaUser class
    public class BolnicaUser : IdentityUser
    {
        public int? PacientId { get; set; }

        public int? DoktorId { get; set; }
    }
}
