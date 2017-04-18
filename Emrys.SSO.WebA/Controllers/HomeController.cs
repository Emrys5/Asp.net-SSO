using Emrys.SSO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emrys.SSO.WebA.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionsDB _db = new SessionsDB();

        private static string key = System.Configuration.ConfigurationManager.AppSettings["SSOKey"];
        private static string ssoToken = System.Configuration.ConfigurationManager.AppSettings["SSOToken"];
        private static string passportUrl = System.Configuration.ConfigurationManager.AppSettings["SSOURL"];

        public ActionResult Index()
        {
            if (Session["ISLOGIN"] == null && !Convert.ToBoolean(Session["ISLOGIN"]))
            {

                Session["userReUrl"] = Request.Url.ToString();

                var time = SSOCommon.DateToTimestamp(DateTime.Now);
                var reUrl = "http://www.weba.com/home/ssologincallback";

                string url = string.Format("{0}?reurl={1}&key={2}&time={3}&sign={4}", passportUrl, reUrl, key, time, SSOCommon.Md5Encrypt(reUrl + key + time + ssoToken));

                return Redirect(url);
            }

            return View();

        }


        public ActionResult SSOLoginCallback(string token, long time, string sign)
        {

            if (SSOCommon.Md5Encrypt(token + time + ssoToken) != sign)
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


        public ActionResult About()
        {
            ViewBag.Message = Convert.ToString(Session["lining"]);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}