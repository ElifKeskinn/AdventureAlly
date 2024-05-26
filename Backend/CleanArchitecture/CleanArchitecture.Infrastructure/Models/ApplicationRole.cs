using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using CleanArchitecture.Core.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;

namespace CleanArchitecture.Infrastructure.Models
{
    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {

        public ApplicationRole() { }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
