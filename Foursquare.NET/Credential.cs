using System;
using System.Text;
using FoursquareNET.OAuth;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace FoursquareNET
{
    public class Credential
    {
        /// <summary>
        /// Credential type to use for Basic Authentication
        /// </summary>
        public class Basic : CredentialBase
        {
            public Basic()
                : base()
            {
            }

            public Basic(string phoneOrEmail, string password)
                : base(phoneOrEmail, password)
            {
            }
        }

        /// <summary>
        /// Credential type to use for OAuth
        /// </summary>
        public class OAuth : CredentialBase
        {
            public OAuth()
                : base()
            {
            }

            public OAuth(string phoneOrEmail, string password)
                : base(phoneOrEmail, password)
            {
            }

            public OAuth(string phoneOrEmail, string password, string key, string secret)
                : base(phoneOrEmail, password)
            {
                ConsumerKey = key;
                ConsumerSecret = secret;
            }

            private bool _HasToken;
            public bool HasToken { get { return _HasToken; } }
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string OAuthToken { get; set; }
            public string OAuthTokenSecret { get; set; }

            public bool Login()
            {
                var uri = new Uri("http://api.foursquare.com/v1/authexchange");

                var sha1 = new HMACSHA1();
                var key = Encoding.ASCII.GetBytes(ConsumerSecret + "&" + "");
                if (key.Length > 64)
                {

                    var coreSha1 = new SHA1CryptoServiceProvider();
                    key = coreSha1.ComputeHash(key);
                }
                sha1.Key = key;

                var timeStamp = Common.GenerateTimeStamp();
                var parameters = new System.Collections.Generic.List<string>
                                     {
                                         "oauth_consumer_key=" + HttpUtility.UrlEncode(ConsumerKey),
                                         "oauth_nonce=" + HttpUtility.UrlEncode(Common.GetNonce(7)),
                                         "oauth_timestamp=" + timeStamp,
                                         "oauth_signature_method=HMAC-SHA1",
                                         "oauth_version=1.0",
                                         "fs_username=" + Username,
                                         "fs_password=" + Password
                                     };

                parameters.Sort();
                var parametersStr = string.Join("&", parameters.ToArray());


                var baseStr = "GET" + "&" +
                    HttpUtility.UrlEncode(uri.ToString()) + "&" +
                    HttpUtility.UrlEncode(parametersStr);


                var baseStringBytes = Encoding.UTF8.GetBytes(baseStr.Replace("%3d", "%3D").Replace("%3a", "%3A").Replace("%2f", "%2F"));
                var baseStringHash = sha1.ComputeHash(baseStringBytes);
                var base64StringHash = Convert.ToBase64String(baseStringHash);
                var encBase64StringHash = HttpUtility.UrlEncode(base64StringHash);

                parameters.Add("oauth_signature=" + encBase64StringHash);
                parameters.Sort();

                var requestUrl = uri + "?" + string.Join("&", parameters.ToArray());
                var r = (HttpWebRequest)WebRequest.Create(requestUrl);

                r.Method = "GET";

                r.ContentType = "application/x-www-form-urlencoded";

                try
                {

                    var response = (HttpWebResponse)r.GetResponse();
                    //UserName = userName;
                    //Password = passWord;

                    var cevap = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    var doc = new XmlDocument();
                    doc.LoadXml(cevap);

                    OAuthToken = doc.SelectSingleNode("//oauth_token").InnerText;
                    OAuthTokenSecret = doc.SelectSingleNode("//oauth_token_secret").InnerText;

                    response.Close();
                    //FS.OAuthToken = oauth_token;
                    //FS.OAuthTokenSecret = oauth_token_secret;
                    //message = cevap;

                    _HasToken = true;
                    return true;
                }
                catch (WebException ex)
                {
                    //message = ex.Message;
                    return false;
                }
            }
        }


        public class CredentialBase
        {
            public CredentialBase()
            {
            }

            public CredentialBase(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
