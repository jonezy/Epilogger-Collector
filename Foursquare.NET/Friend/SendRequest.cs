using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class SendRequest
    {
        private const string URL = "http://api.foursquare.com/v1/friend/sendrequest";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Sends a friend request to another user. On success, returns the user object.
        /// </summary>
        public SendRequest()
        {
            
        }

        public Schema.User Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_UserId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.UserObj>(result).User;
        }

        #region Parameters

        private Parameter _UserId = new Parameter("UserId", "uid", true);

        /// <summary>
        /// the user ID of the user to whom you want to send a friend request
        /// </summary>
        public int UserId { set { _UserId.Value = value.ToString(); } }

        #endregion
    }
}
