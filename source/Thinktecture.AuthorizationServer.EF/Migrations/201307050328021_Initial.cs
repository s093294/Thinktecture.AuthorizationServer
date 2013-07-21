namespace Thinktecture.AuthorizationServer.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "authz.GlobalConfigurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AuthorizationServerName = c.String(nullable: false),
                        AuthorizationServerLogoUrl = c.String(),
                        Issuer = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "authz.AuthorizationServerAdministrators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NameID = c.String(),
                        GlobalConfiguration_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("authz.GlobalConfigurations", t => t.GlobalConfiguration_ID)
                .Index(t => t.GlobalConfiguration_ID);
            
            CreateTable(
                "authz.Applications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        LogoUrl = c.String(),
                        Namespace = c.String(nullable: false),
                        Audience = c.String(nullable: false),
                        TokenLifetime = c.Int(nullable: false),
                        AllowRefreshToken = c.Boolean(nullable: false),
                        RequireConsent = c.Boolean(nullable: false),
                        AllowRememberConsentDecision = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        SigningKey_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("authz.SigningKeys", t => t.SigningKey_ID, cascadeDelete: true)
                .Index(t => t.SigningKey_ID);
            
            CreateTable(
                "authz.SigningKeys",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.Int(),
                        StoreName = c.Int(),
                        FindType = c.Int(),
                        FindValue = c.String(),
                        Value = c.Binary(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "authz.Scopes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                        Description = c.String(),
                        Emphasize = c.Boolean(nullable: false),
                        Application_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("authz.Applications", t => t.Application_ID, cascadeDelete: true)
                .Index(t => t.Application_ID);
            
            CreateTable(
                "authz.Clients",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 128),
                        ClientSecret = c.String(nullable: false),
                        AuthenticationMethod = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Flow = c.Int(nullable: false),
                        AllowRefreshToken = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        RequireConsent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "authz.ClientRedirectUris",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false),
                        Description = c.String(),
                        Client_ClientId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("authz.Clients", t => t.Client_ClientId, cascadeDelete: true)
                .Index(t => t.Client_ClientId);
            
            CreateTable(
                "authz.TokenHandles",
                c => new
                    {
                        HandleId = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        RedirectUri = c.String(),
                        Type = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Expiration = c.DateTime(),
                        CreateRefreshToken = c.Boolean(nullable: false),
                        RefreshTokenExpiration = c.DateTime(),
                        Client_ClientId = c.String(maxLength: 128),
                        Application_ID = c.Int(),
                    })
                .PrimaryKey(t => t.HandleId)
                .ForeignKey("authz.Clients", t => t.Client_ClientId)
                .ForeignKey("authz.Applications", t => t.Application_ID)
                .Index(t => t.Client_ClientId)
                .Index(t => t.Application_ID);
            
            CreateTable(
                "authz.TokenHandleClaims",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.String(),
                        TokenHandle_HandleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("authz.TokenHandles", t => t.TokenHandle_HandleId, cascadeDelete: true)
                .Index(t => t.TokenHandle_HandleId);
            
            CreateTable(
                "authz.ScopeClients",
                c => new
                    {
                        Scope_ID = c.Int(nullable: false),
                        Client_ClientId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Scope_ID, t.Client_ClientId })
                .ForeignKey("authz.Scopes", t => t.Scope_ID, cascadeDelete: true)
                .ForeignKey("authz.Clients", t => t.Client_ClientId, cascadeDelete: true)
                .Index(t => t.Scope_ID)
                .Index(t => t.Client_ClientId);
            
            CreateTable(
                "authz.TokenHandleScopes",
                c => new
                    {
                        TokenHandle_HandleId = c.String(nullable: false, maxLength: 128),
                        Scope_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TokenHandle_HandleId, t.Scope_ID })
                .ForeignKey("authz.TokenHandles", t => t.TokenHandle_HandleId, cascadeDelete: true)
                .ForeignKey("authz.Scopes", t => t.Scope_ID, cascadeDelete: true)
                .Index(t => t.TokenHandle_HandleId)
                .Index(t => t.Scope_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("authz.TokenHandleScopes", "Scope_ID", "authz.Scopes");
            DropForeignKey("authz.TokenHandleScopes", "TokenHandle_HandleId", "authz.TokenHandles");
            DropForeignKey("authz.TokenHandleClaims", "TokenHandle_HandleId", "authz.TokenHandles");
            DropForeignKey("authz.TokenHandles", "Application_ID", "authz.Applications");
            DropForeignKey("authz.TokenHandles", "Client_ClientId", "authz.Clients");
            DropForeignKey("authz.Scopes", "Application_ID", "authz.Applications");
            DropForeignKey("authz.ScopeClients", "Client_ClientId", "authz.Clients");
            DropForeignKey("authz.ScopeClients", "Scope_ID", "authz.Scopes");
            DropForeignKey("authz.ClientRedirectUris", "Client_ClientId", "authz.Clients");
            DropForeignKey("authz.Applications", "SigningKey_ID", "authz.SigningKeys");
            DropForeignKey("authz.AuthorizationServerAdministrators", "GlobalConfiguration_ID", "authz.GlobalConfigurations");
            DropIndex("authz.TokenHandleScopes", new[] { "Scope_ID" });
            DropIndex("authz.TokenHandleScopes", new[] { "TokenHandle_HandleId" });
            DropIndex("authz.TokenHandleClaims", new[] { "TokenHandle_HandleId" });
            DropIndex("authz.TokenHandles", new[] { "Application_ID" });
            DropIndex("authz.TokenHandles", new[] { "Client_ClientId" });
            DropIndex("authz.Scopes", new[] { "Application_ID" });
            DropIndex("authz.ScopeClients", new[] { "Client_ClientId" });
            DropIndex("authz.ScopeClients", new[] { "Scope_ID" });
            DropIndex("authz.ClientRedirectUris", new[] { "Client_ClientId" });
            DropIndex("authz.Applications", new[] { "SigningKey_ID" });
            DropIndex("authz.AuthorizationServerAdministrators", new[] { "GlobalConfiguration_ID" });
            DropTable("authz.TokenHandleScopes");
            DropTable("authz.ScopeClients");
            DropTable("authz.TokenHandleClaims");
            DropTable("authz.TokenHandles");
            DropTable("authz.ClientRedirectUris");
            DropTable("authz.Clients");
            DropTable("authz.Scopes");
            DropTable("authz.SigningKeys");
            DropTable("authz.Applications");
            DropTable("authz.AuthorizationServerAdministrators");
            DropTable("authz.GlobalConfigurations");
        }
    }
}
