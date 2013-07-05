/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Thinktecture.AuthorizationServer.WebHost.Controllers
{
    public class AccountController : Controller
    {
        public async Task<ActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)            
            {
                var authModule = FederatedAuthentication.WSFederationAuthenticationModule;
                var url = WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(authModule.Issuer, authModule.Realm, null);
                var request = WebRequest.Create(url);
                var task = request.GetResponseAsync();

                authModule.SignOut();


                var result = await task;
                
                
            }
            return View();
        }

    }
}
