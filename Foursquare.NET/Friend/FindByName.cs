using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class FindByName
    {
        private const string URL = "http://api.foursquare.com/v1/findfriends/byname";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// When passed a free-form text string, returns a list of matching user objects. The method only returns matches of people with whom you are not already friends.
        /// </summary>
        public FindByName()
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
        /// the string you want to use to search firstnames and lastnames
        /// </summary>
        public string Query { get { return _Query.Value; } set { _Query.Value = value; } }

        #endregion
    }
}
