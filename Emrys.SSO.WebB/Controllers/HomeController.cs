using Emrys.SSO.Common;
using Emrys.SSO.WebB.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Emrys.SSO.WebB.Controllers
{
    public class HomeController : Controller
    {

        private readonly SessionsDB _db = new SessionsDB();
        private static string _key = System.Configuration.ConfigurationManager.AppSettings["SSOKey"];
        private static string _ssoToken = System.Configuration.ConfigurationManager.AppSettings["SSOToken"];
        private static string _passportUrl = System.Configuration.ConfigurationManager.AppSettings["SSOURL"];
        private static string _SSOLoginCallback = System.Configuration.ConfigurationManager.AppSettings["SSOLoginCallback"];


        [AuthLogin]
        public ActionResult Index()
        { 
            return View();

        }


        public ActionResult SSOLoginCallback(string token, long time, string sign)
        {

            if (SSOCommon.Md5Encrypt(token + time + _ssoToken) != sign)
            {
                return Content("签名验证失败！");

            }
            // 验证签名日期
            var checkTime = SSOCommon.TimestampToDate(time);
            if (!(DateTime.Now.AddMinutes(-2) < checkTime && checkTime < DateTime.Now.AddMinutes(2)))
            {
                return Content("签名过期！");
            }

            var s = _db.ASPStateTempSessions.Where(i => i.Token == token && i.IsPassport).FirstOrDefault();
            if (s == null)
            {
                return Content("token验证失败！");
            }

            var userSession = _db.ASPStateTempSessions.Where(i => i.SessionId == Session.SessionID).FirstOrDefault();
            if (userSession == null)
            {
                return Content("session出现问题！");
            }
            userSession.Token = token;
            _db.SaveChanges();
            // dologin
            Session["ISLOGIN"] = true;

            return Redirect(Convert.ToString(Session["userReUrl"]));
        }


        public ActionResult IsLogin()
        {
            ViewBag.IsLogin = Session["ISLOGIN"] != null && Convert.ToBoolean(Session["ISLOGIN"]);
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}