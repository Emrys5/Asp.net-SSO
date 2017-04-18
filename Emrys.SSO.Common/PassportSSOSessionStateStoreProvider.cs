using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Emrys.SSO.Common
{
    public class PassportSSOSessionStateStoreProvider : SSOSessionStateStoreProvider
    {
        public override bool IsPassport
        {
            get { return true; }
        }

        public override string Token
        {
            get { return Guid.NewGuid().ToString("N"); }
        }



    }
}
