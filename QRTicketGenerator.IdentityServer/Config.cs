// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Project2
{
    public static class Config
    {
        // Implement IConfiguration in this code


        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
            {
                new ApiResource("Ticket_aud"){Scopes={"Ticket_fullpermissions" } },
                new ApiResource("Cron_job_aud"){Scopes={"Cronjob_fullpermissions" } },
                new ApiResource("gateway_aud"){Scopes={"gateway_fullpermissions" } },
                new ApiResource("Order_aud"){Scopes={"Order_fullpermissions"}},
                new ApiResource (IdentityServerConstants.LocalApi.ScopeName)
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Email(),
                    new IdentityResource("roles","Roles",new List<string>{"role"}),
                    new IdentityResources.Profile()
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Ticket_fullpermissions"),
                new ApiScope("Cronjob_fullpermissions"),
                new ApiScope("gateway_fullpermissions"),
                new ApiScope("Order_fullpermissions"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "ReactClient",
                    //ClientId = Startup.InitializeConfig.GetSection("SecretkeyIdendity:ClinetId").Value,
                    ClientSecrets = { new Secret("49C1A7E1-murad-0C7998FB86B01979superzor..!"
                        //Startup.InitializeConfig.GetSection("SecretkeyIdendity:ClientSecret").Value
                        .Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "Ticket_fullpermissions", "gateway_fullpermissions", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientId ="ReactClientForUser",
                    // Startup.InitializeConfig.GetSection("SecretkeyIdendity:ClientIdUser").Value,
                    ClientSecrets = { new Secret("49C1A7E1-murad-0C7998FB86B01979superzor..!"
                        //Startup.InitializeConfig.GetSection("SecretkeyIdendity:ClientSecret").Value
                        .Sha256()) },
                    AllowOfflineAccess = true,
                    // allow scope add profile email of user
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.Profile, " Order_fullpermissions",
                    " Ticket_fullpermissions", " gateway_fullpermissions", IdentityServerConstants.LocalApi.ScopeName,
                     IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.OfflineAccess,"roles"},
                     RefreshTokenUsage = TokenUsage.ReUse,
                }
            };
    }
}