using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Emrys.SSO.Common
{
    public sealed class SSOPassportConfiguration : ConfigurationSection
    {
        private SSOPassportConfiguration() { }

        private static SSOPassportConfiguration _sso = (SSOPassportConfiguration)ConfigurationManager.GetSection("ssoPassport");
        public static SSOPassportConfiguration Instance()
        {
            return _sso;
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public SSOPassportCollection SSOPassportCollection
        {
            get
            {
                return (SSOPassportCollection)base[""];
            }
        }
    }


    public class SSOPassportCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new SSOPassportWeb();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SSOPassportWeb)element).Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "web";
            }
        }

        public SSOPassportWeb this[int index]
        {
            get
            {
                return (SSOPassportWeb)BaseGet(index);
            }
        }

        public SSOPassportWeb this[object key]
        {
            get
            {
                return (SSOPassportWeb)BaseGet(key);
            }
        }
    }


    public class SSOPassportWeb : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"].ToString(); }
        }

        [ConfigurationProperty("token", IsRequired = true)]
        public string Token
        {
            get { return this["token"].ToString(); }
        }
    }

}
