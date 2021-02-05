using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WithoutIdentity.Models;

namespace WithoutIdentity.Data
{
    public class ApplicationDataContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid> //guid aqui é o tipo de PK para users e roles
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
            :base(options)
        {

        }
    }
}
