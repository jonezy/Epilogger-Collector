using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.User
{
    public class GetDetail
    {
        private const string URL = "http://api.foursquare.com/v1/user";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns profile information (badges, etc) for a given user. If the user has recent check-in data (ie, if the user is self or is a friend of the authenticating user), this data will be returned as well in a checkin block.
        /// </summary>
        public GetDetail()
        {
        }

        public Schema.User Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_UserId.ToString());
            Parameters.Add(_ShowBadges.ToString());
            Parameters.Add(_ShowMayorships.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.UserObj>(result).User;
        }

        #region Parameters

        private Parameter _UserId = new Parameter("UserId", "uid", false);
        private Parameter _ShowBadges = new Parameter("ShowBadges", "badges", false);
        private Parameter _ShowMayorships = new Parameter("ShowMayorships", "mayor", false);

        /// <summary>
        /// userid for the user whose information you want to retrieve. if you do not specify a 'uid', the authenticated user's profile data will be returned.
        /// </summary>
        public int? UserId { set { _UserId.Value = value != null ? value.ToString() : string.Empty; } }

        /// <summary>
        /// (optional, default: false) set to "true" to also show badges for this user. by default, this will show badges earned worldwide.
        /// </summary>
        public bool? ShowBadges { set { _ShowBadges.Value = value != null ? (value.Value ? "1" : "0") : string.Empty; } }

        /// <summary>
        /// (optional, default: false) set to "true" to also show venues for which this user is a mayor. by default, this will show mayorships worldwide.
        /// </summary>
        public bool? ShowMayorships { set { _ShowMayorships.Value = value != null ? (value.Value ? "1" : "0") : string.Empty; } }

        #endregion
    }
}
