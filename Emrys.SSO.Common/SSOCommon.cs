using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emrys.SSO.Common
{
    public class SSOCommon
    {
        public static string Md5Encrypt(string val)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(val);
            bs = md5.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToUpper());
            }
            string password = s.ToString();
            return password;
        }


        public static DateTime TimestampToDate(long strValue)
        { 
            var date = new DateTime(1970, 1, 1);
            date = date.AddMilliseconds(strValue);
            return date; 
        }

        public static long DateToTimestamp(DateTime value)
        {
            var epoc = new DateTime(1970, 1, 1);
            var delta = ((DateTime)value) - epoc;
            return Convert.ToInt64(delta.TotalMilliseconds);
        }
    }
}
