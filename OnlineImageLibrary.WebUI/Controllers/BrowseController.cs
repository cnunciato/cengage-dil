using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineImageLibrary.WebUI.OLR; 
using OnlineImageLibrary.WebUI.SSO;

namespace OnlineImageLibrary.WebUI.Controllers
{
    public class BrowseController : Controller
    {
        public ActionResult Index()
        {
            string token = Request.QueryString["token"];
            string isbn = Request.QueryString["eISBN"];

            if (token != null && isbn != null && token.ToString() != "" && isbn.ToString() != "")
            {
                SSOws sso = new SSOws();
                OLRws olr = new OLRws();
                SSOwsResult tokenResult = sso.validateToken(token);

                if (tokenResult.resultDescription == "Success")
                {
                    getEntitlementParameters parameters = new getEntitlementParameters();
                    parameters.userToken = token;

                    filter eisbnFilter = new filter();
                    parameters.userToken = token;
                    eisbnFilter.filterType = "EISBN";
                    eisbnFilter.filerValue = isbn;
                    parameters.filter = eisbnFilter;
                    OLRwsResult entitlementResult = olr.getEntitlements(parameters);

                    if (entitlementResult.resultDescription == "Success")
                    {
                        // All good; do nothing
                    }
                    else
                    {
                        return View("AccessDenied");
                    }
                }
                else
                {
                    return View("AccessDenied");
                }
            }
            else
            {
                return View("AccessDenied");
            }
            return View();
        }
    }
}
