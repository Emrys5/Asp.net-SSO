using Emrys.SSO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Emrys.SSO.WebA.Filter
{
    public class AuthLoginAttribute : AuthorizeAttribute
    {

        private readonly SessionsDB _db = new SessionsDB();
        private static string _key = System.Configuration.ConfigurationManager.AppSettings["SSOKey"];
        private static string _ssoToken = System.Configuration.ConfigurationManager.AppSettings["SSOToken"];
        private static string _passportUrl = System.Configuration.ConfigurationManager.AppSettings["SSOURL"];
        private static string _SSOLoginCallback = System.Configuration.ConfigurationManager.AppSettings["SSOLoginCallback"];
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (httpContext.Session["ISLOGIN"] == null && !Convert.ToBoolean(httpContext.Session["ISLOGIN"]))
            {
                httpContext.Session["userReUrl"] = httpContext.Request.Url.ToString();

                var time = SSOCommon.DateToTimestamp(DateTime.Now);

                string url = string.Format("{0}?reurl={1}&key={2}&time={3}&sign={4}", _passportUrl, _SSOLoginCallback, _key, time, SSOCommon.Md5Encrypt(_SSOLoginCallback + _key + time + _ssoToken));

                filterContext.Result = new RedirectResult(url);

            }

        }

    }
}