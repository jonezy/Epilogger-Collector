using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class GetPending
    {
        private const string URL = "http://api.foursquare.com/v1/friend/requests";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to add a new tip or to-do at a venue.
        /// </summary>
        public GetPending()
        {
            
        }

        public List<Schema.User> Execute(Credential.CredentialBase credential)
        {
            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.RequestsObj>(result).Requests;
        }

        #region Parameters

        //no parameters

        #endregion
    }
}
