﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithoutIdentity.Models
{
    public class ApplicationUser : IdentityUser<Guid> //herdando o identityUser por se caso precise criar novos métodos
    {
    }
}
