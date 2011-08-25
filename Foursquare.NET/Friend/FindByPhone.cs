using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class FindByPhone
    {
        private const string URL = "http://api.foursquare.com/v1/findfriends/byphone";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// When passed phone number(s), returns a list of matching user objects. The method only returns matches of people with whom you are not already friends. You can pass a single number as a parameter, or you can pass multiple numbers separated by commas.
        /// </summary>
        public FindByPhone()
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

        private Parameter _Query = new Parameter("Query", "q", true);

        /// <summary>
        /// the string you want to use to search for phone numbers
        /// </summary>
        public string Query { set { _Query.Value = value; } }

        #endregion
    }
}
