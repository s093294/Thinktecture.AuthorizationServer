﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Data.Entity;
using Thinktecture.AuthorizationServer.Models;

namespace Thinktecture.AuthorizationServer.EF
{
    public class AuthorizationServerContext : DbContext
    {
        public AuthorizationServerContext()
            : base("AuthorizationServerContext")
        {
        }

        public DbSet<GlobalConfiguration> GlobalConfiguration { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<SigningKey> SigningKeys { get; set; }
        public DbSet<TokenHandle> TokenHandles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("authz");
            modelBuilder.Entity<Client>().HasMany(x => x.RedirectUris).WithRequired();
            modelBuilder.Entity<Application>().HasMany(x => x.Scopes).WithRequired();
            modelBuilder.Entity<Scope>().HasMany(x => x.AllowedClients).WithMany();
            modelBuilder.Entity<TokenHandle>().HasMany(x => x.Scopes).WithMany();
            modelBuilder.Entity<TokenHandle>().HasMany(x => x.ResourceOwner).WithRequired();
        }
    }
}
