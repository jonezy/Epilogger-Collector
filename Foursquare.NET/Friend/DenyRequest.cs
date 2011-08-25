using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Friend
{
    public class DenyRequest
    {
        private const string URL = "http://api.foursquare.com/v1/friend/deny";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Denies a pending friend request from another user. On success, returns the user object.
        /// </summary>
        public DenyRequest()
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
        /// the user ID of the user who you want to deny
        /// </summary>
        public int UserId { set { _UserId.Value = value.ToString(); } }

        #endregion
    }
}
