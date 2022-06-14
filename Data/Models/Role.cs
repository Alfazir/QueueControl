using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QueueControlServer.Models
{
    public class Role : IdentityRole
    {
        public Role() : base() { }
        public Role(string Name): base(Name) { }
          
    }
}


// <TKey> where TKey : IEquatable<TKey>