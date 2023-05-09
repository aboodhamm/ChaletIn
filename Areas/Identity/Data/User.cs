using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chaletin.Areas.Identity.Data;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public List<Farm> Farms { get; set; }
    public List<Comments> Comments { get; set; }
    public List<ContactMessage> ContactMessages { get; set; }
    public bool Blocked { get; set; }
}

