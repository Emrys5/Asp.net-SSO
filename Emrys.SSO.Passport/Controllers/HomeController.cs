using Emrys.SSO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emrys.SSO.Passport.Controllers
{
    public class HomeController : Controller
    {

        SessionsDB _db = new SessionsDB();

        [HttpGet]
        public ActionResult Index(string reurl, string key, long time, string sign)
        {

            // 验证参数 
            if (string.IsNullOrEmpty(reurl) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(sign))
            {
                return Content("回调url和网站key和签名不能为空！");
            }

            // 从配置文件获取站点信息
            var webConfig = SSOPassportConfiguration.Instance().SSOPassportCollection[key];
            if (webConfig == null)
            {
                return Content("key不正确！");
            }

            // 验证签名
            var checkSign = SSOCommon.Md5Encrypt(reurl + key + time + webConfig.Token);
            if (checkSign != sign)
            {
                return Content("签名不正确！");
            }

            // 验证签名日期
            var checkTime = SSOCommon.TimestampToDate(time);
            if (!(DateTime.Now.AddMinutes(-2) < checkTime && checkTime < DateTime.Now.AddMinutes(2)))
            {
                return Content("签名过期！");
            }

            Session["REURL"] = reurl;
            Session["ISSIGN"] = true;
            Session["WEBTOKEN"] = webConfig.Token;
            if (Session["ISLOGIN"] != null && Convert.ToBoolean(Session["ISLOGIN"]))
            {
                var sessionToken = Convert.ToString(Session["SESSIONTOKEN"]);
                long reTime = SSOCommon.DateToTimestamp(DateTime.Now);
                return Redirect(string.Format("{0}?token={1}&time={2}&sign={3}", reurl, sessionToken, reTime, SSOCommon.Md5Encrypt(sessionToken + reTime + webConfig.Token)));
            }


            return View();
        }

        [HttpPost]
        public ActionResult Index(string name, string pwd)
        {
            if (name == "admin" && pwd == "admin")
            {
                if (Convert.ToBoolean(Session["ISSIGN"]))
                {
                    var sessionToken = _db.ASPStateTempSessions.Where(i => i.SessionId == Session.SessionID).FirstOrDefault().Token;
                    Session["ISLOGIN"] = true;
                    Session["SESSIONTOKEN"] = sessionToken;

                    string reUrl = Convert.ToString(Session["REURL"]);
                    string webToken = Convert.ToString(Session["WEBTOKEN"]);
                    long reTime = SSOCommon.DateToTimestamp(DateTime.Now);

                    return Redirect(string.Format("{0}?token={1}&time={2}&sign={3}", reUrl, sessionToken, reTime, SSOCommon.Md5Encrypt(sessionToken + reTime + webToken)));
                }
                else
                {
                    ViewBag.Msg = "签名失败";
                }
            }
            else
            {
                ViewBag.Msg = "用户名或密码错误！";
            }

            return View();
        }



        public ActionResult Logout(string reurl)
        {
            // 所有授权网站退出

            var token = Convert.ToString(Session["SESSIONTOKEN"]);
            var webSessions = _db.ASPStateTempSessions.Where(i => i.Token == token && !i.IsPassport).ToList();
            //foreach (var item in webSessions)
            //{
            //    var updateItem = _db.ASPStateTempSessions.Where(i => i.SessionId == item.SessionId).FirstOrDefault();
            //    if (updateItem != null)
            //    {
            //        item.Expires = DateTime.Now.AddDays(-1);
            //        //item.Timeout = 0;
            //    }
            //}

            _db.ASPStateTempSessions.RemoveRange(webSessions);
            _db.SaveChanges();

            // passport 退出
            Session["ISLOGIN"] = null;
            Session["SESSIONTOKEN"] = null;

            return Redirect(reurl);
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