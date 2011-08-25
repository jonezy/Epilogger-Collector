using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using FoursquareNET.Schema;
using System.Web.Script.Serialization;
using CredentialBase = FoursquareNET.Credential.CredentialBase;
using System.Security.Cryptography;
using FoursquareNET.OAuth;

namespace FoursquareNET
{
    public static class Common
    {
        public enum HttpRequestMethod
        {
            POST,
            GET
        }

        /// <summary>
        /// GenerateTimeStamp method generates TimeStamp value.
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp()
        {

            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            var timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            return timeStamp;
        }
        /// <summary>
        /// Nonce is a unique string based value. GetNonce method generates this value
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetNonce(int length)
        {
            const string allowedCharacters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var password = new StringBuilder();
            var rand = new Random();

            for (var i = 0; i < length; i++)
            {
                password.Append(allowedCharacters[rand.Next(0, (allowedCharacters.Length - 1))]);
            }
            return password.ToString();

        }

        private static string GenerateUrl(string URL, List<string> parameters)
        {
            if (URL == null) throw new ArgumentNullException("URL");
            var url = new StringBuilder(URL);

            //url.Append(".json?");

            parameters.RemoveAll(p => string.IsNullOrEmpty(p));
            url.Append(string.Join("&", parameters.ToArray()));

            return url.ToString();
        }

        internal static string HttpPost(string url, List<string> parameters, CredentialBase credential, HttpRequestMethod httpRequestMethod)
        {
            url += ".json?";

            parameters = parameters.Where(p => !string.IsNullOrEmpty(p)).ToList();

            url = GenerateUrl(url, parameters);

            HttpWebRequest httpRequest;

            if (credential.GetType() == typeof(Credential.OAuth))
            {
                if (!((Credential.OAuth)credential).HasToken)
                    ((Credential.OAuth)credential).Login();

                httpRequest = HttpPostOAuth(url, (Credential.OAuth)credential, httpRequestMethod);
            }
            else
                httpRequest = HttpPostBasic(url, credential, httpRequestMethod);

            try
            {
                using (var response = (HttpWebResponse)httpRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException ex)
            {
                var result = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                var error = new JavaScriptSerializer().Deserialize<ErrorObj>(result);

                if (!string.IsNullOrEmpty(error.Error))
                    throw new Exception.WebException(error.Error);

                if (!string.IsNullOrEmpty(error.RateLimited))
                    throw new Exception.RateLimitedException(error.RateLimited);

                if (!string.IsNullOrEmpty(error.Unauthorized))
                    throw new Exception.UnauthorizedException(error.Unauthorized);

                throw;
            }
        }

        private static HttpWebRequest HttpPostBasic(string url, CredentialBase credential, HttpRequestMethod httpRequestMethod)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            if (credential.GetType() == typeof(Credential.Basic))
            {
                if (!string.IsNullOrEmpty(credential.Username) && !string.IsNullOrEmpty(credential.Password))
                {
                    var authenticationHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", credential.Username, credential.Password)));
                    httpRequest.Headers["Authorization"] = "Basic " + authenticationHeader;
                }
            }

            httpRequest.Method = httpRequestMethod.ToString();

            return httpRequest;
        }

        internal static HttpWebRequest HttpPostOAuth(string url, Credential.OAuth credential, HttpRequestMethod httpRequestMethod)
        {
            var sha1 = new HMACSHA1();
            var key = Encoding.ASCII.GetBytes(credential.ConsumerSecret + "&" + credential.OAuthTokenSecret);
            if (key.Length > 64)
            {
                var coreSha1 = new SHA1CryptoServiceProvider();
                key = coreSha1.ComputeHash(key);
            }
            sha1.Key = key;

            var oAuth = new OAuthBase();
            var nonce = oAuth.GenerateNonce();
            var timeStamp = oAuth.GenerateTimeStamp();
            string normalizedUrl;
            string normalizedRequestParameters;
            var uri = new Uri(url);
            var signature = oAuth.GenerateSignature(uri, credential.ConsumerKey, credential.ConsumerSecret, credential.OAuthToken, credential.OAuthTokenSecret, httpRequestMethod.ToString(), timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
            signature = HttpUtility.UrlEncode(signature);

            var parameters = new List<string> {"oauth_signature=" + signature, normalizedRequestParameters};

            HttpWebRequest httpRequest = null;
            
            switch (httpRequestMethod.ToString())
            {
                case "GET":
                    url = GenerateUrl(url, parameters);
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = httpRequestMethod.ToString();
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                    break;

                case "POST":
                    {
                        httpRequest = (HttpWebRequest)WebRequest.Create(normalizedUrl);

                        httpRequest.ContentType = "application/x-www-form-urlencoded";

                        var parametersStr = string.Join("&", parameters.ToArray());
                        var bytes = Encoding.ASCII.GetBytes(parametersStr);
                        httpRequest.ContentLength = bytes.Length;

                        httpRequest.Method = httpRequestMethod.ToString();
                        using (var s = httpRequest.GetRequestStream())
                        {
                            s.Write(bytes, 0, bytes.Length);
                            s.Close();
                        }
                    }
                    break;
            }

            return httpRequest;
        }
    }
}
