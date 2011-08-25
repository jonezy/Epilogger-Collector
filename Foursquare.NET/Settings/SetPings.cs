using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Settings
{
    public class SetPings
    {
        private const string URL = "http://api.foursquare.com/v1/settings/setpings";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to add a new tip or to-do at a venue.
        /// </summary>
        public SetPings()
        {
            
        }

        public Schema.PingStatus Execute(Credential.CredentialBase credential)
        {
            if (string.IsNullOrEmpty(_Friend.Key) ^ string.IsNullOrEmpty(_Friend.Value))
                throw new Exception.OptionalParameterException("If setting ping for a friend, both FriendUserId and FriendPingStatus must be set.");

            if (string.IsNullOrEmpty(_Self.Key) && (string.IsNullOrEmpty(_Friend.Key) || string.IsNullOrEmpty(_Friend.Value)))
                throw new Exception.OptionalParameterException("You may only set pings for either authenticated user or a friend at one time.");

            Parameters.Clear();
            Parameters.Add(_Self.ToString());
            Parameters.Add(_Friend.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.SettingsObj>(result).Settings.Pings;
        }

        #region Parameters

        private Parameter _Self = new Parameter("SelfPingStatus", "self", false);
        private Parameter _Friend = new Parameter("", "", false);

        /// <summary>
        /// the ping status for yourself (globally). possible values are on and off.
        /// </summary>
        public Schema.PingStatus SelfPingStatus { set { _Self.Value = value.ToString(); } }

        /// <summary>
        /// id of a friend to set set the ping status
        /// </summary>
        public int FriendUserId
        {
            set
            {
                _Friend = new Parameter("FriendPingStatus", value.ToString(), false)
                              {Value = _FriendPingStatus.ToString()};
            }
        }

        /// <summary>
        /// set the ping status for a friend. possible values are on and off.
        /// </summary>
        public Schema.PingStatus FriendPingStatus
        {
            set
            {
                _FriendPingStatus = value;
                _Friend.Value = value.ToString();
            }
        }
        private Schema.PingStatus? _FriendPingStatus;


        #endregion
    }
}
