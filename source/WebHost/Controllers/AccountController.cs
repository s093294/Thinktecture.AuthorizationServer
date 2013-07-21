/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.IdentityModel.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Thinktecture.AuthorizationServer.WebHost.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignOut()
        {
            var config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;

            string callbackUrl = Url.Action("Index", "Home", routeValues: null, protocol: Request.Url.Scheme);
            var signoutMessage = new SignOutRequestMessage(new Uri(config.Issuer),callbackUrl);
            signoutMessage.SetParameter("wtrealm",config.Realm);

            FederatedAuthentication.WSFederationAuthenticationModule.SignOut();

            return new RedirectResult(signoutMessage.WriteQueryString());
        }
        public ActionResult SignIn()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            string callbackUrl = Url.Action("Index", "Home", routeValues: null, protocol: Request.Url.Scheme);
            var signInRequest = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest(
                uniqueId: string.Empty,
                returnUrl: callbackUrl,
                rememberMeSet: false);
            return new RedirectResult(signInRequest.RequestUrl.ToString());
        }

    }
}
