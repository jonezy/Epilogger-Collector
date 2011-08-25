using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.User
{
    public class GetFriends
    {
        private const string URL = "http://api.foursquare.com/v1/friends";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns a list of friends. If you do not specify uid, the authenticating user's list of friends will be returned. 
        /// If the friend has allowed it, you'll also see links to their Twitter and Facebook accounts.
        /// </summary>
        public GetFriends()
        {
            
        }

        public List<Schema.User> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_UserId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.FriendObj>(result).Friends;
        }

        #region Parameters

        private Parameter _UserId = new Parameter("UserId", "uid", false);

        /// <summary>
        /// user id of the person for whom you want to pull a friend graph (optional)
        /// </summary>
        public int? UserId { set { _UserId.Value = value.HasValue ? value.ToString() : string.Empty; } }

        #endregion
    }
}
