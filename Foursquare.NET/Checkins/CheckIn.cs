using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Checkins
{
    public class CheckIn
    {
        private const string URL = "http://api.foursquare.com/v1/checkin";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to check-in to a place.
        /// </summary>
        public CheckIn()
        {
            
        }

        public Schema.Checkin Execute(Credential.CredentialBase credential)
        {
            if (string.IsNullOrEmpty(_VenueId.Value) && string.IsNullOrEmpty(_Shout.Value) && string.IsNullOrEmpty(_VenueName.Value))
                throw new Exception.OptionalParameterException("You must specify a VenueId, VenueName, or Shout");

            Parameters.Clear();
            Parameters.Add(_VenueId.ToString());
            Parameters.Add(_VenueName.ToString());
            Parameters.Add(_Shout.ToString());
            Parameters.Add(_Private.ToString());
            Parameters.Add(_Twitter.ToString());
            Parameters.Add(_Facebook.ToString());
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.CheckinObj>(result).Checkin;
        }

        #region Parameters

        private Parameter _VenueId = new Parameter("VenueId", "vid", false);
        private Parameter _VenueName = new Parameter("VenueName", "venue", false);
        private Parameter _Shout = new Parameter("Shout", "shout", false);
        private Parameter _Private = new Parameter("Private", "private", false);
        private Parameter _Twitter = new Parameter("Twitter", "twitter", false);
        private Parameter _Facebook = new Parameter("Facebook", "facebook", false);
        private Parameter _Geolat = new Parameter("Geolat", "geolat", false);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", false);

        /// <summary>
        /// (optional, not necessary if you are 'shouting' or have a venue name). ID of the venue where you want to check-in
        /// </summary>
        public int? VenueId { set { _VenueId.Value = value != null ? value.ToString() : string.Empty; } }

        /// <summary>
        /// (optional, not necessary if you are 'shouting' or have a vid) if you don't have a venue ID or would rather prefer a 'venueless' checkin, pass the venue name as a string using this parameter. it will become an 'orphan' (no address or venueid but with geolat, geolong)
        /// </summary>
        public string Venue { set { _VenueName.Value = value; } }

        /// <summary>
        /// (optional) a message about your check-in. the maximum length of this field is 140 characters
        /// </summary>
        public string Shout { set { _Shout.Value = value; } }

        /// <summary>
        /// (optional). "true" means "don't show your friends". "false" means "show everyone"
        /// </summary>
        public bool? Private { set { _Private.Value = value != null ? (value.Value ? "true" : "false") : string.Empty; } }

        /// <summary>
        /// (optional, defaults to the user's setting). "true" means "send to Twitter". "false" means "don't send to Twitter"
        /// </summary>
        public bool? Twitter { set { _Twitter.Value = value != null ? (value.Value ? "true" : "false") : string.Empty; } }

        /// <summary>
        /// (optional, defaults to the user's setting). "true" means "send to Facebook". "false" means "don't send to Facebook"
        /// </summary>
        public bool? Facebook { set { _Facebook.Value = value != null ? (value.Value ? "true" : "false") : string.Empty; } }

        /// <summary>
        /// (optional, but recommended)
        /// </summary>
        public string Geolat { set { _Geolat.Value = value; } }

        /// <summary>
        /// (optional, but recommended)
        /// </summary>
        public string Geolong { set { _Geolong.Value = value; } }

        #endregion
    }
}
