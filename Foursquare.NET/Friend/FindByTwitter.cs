using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class FindByTwitter
    {
        private const string URL = "http://api.foursquare.com/v1/findfriends/bytwitter";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// When passed a Twitter name (user A), returns a list of matching user objects that correspond to user A's friends on Twitter. The method only returns matches of people with whom you are not already friends.
        ///
        /// If you don't pass in a Twitter name, it will attempt to use the Twitter name associated with the authenticating user.
        /// </summary>
        public FindByTwitter()
        {
            
        }

        public List<Schema.User> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_Query.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.UsersObj>(result).Users;
        }

        #region Parameters

        private Parameter _Query = new Parameter("Query", "q", false);

        /// <summary>
        /// (optional) the Twitter name you want to use to search
        /// </summary>
        public string Query { set { _Query.Value = value; } }

        #endregion
    }
}
