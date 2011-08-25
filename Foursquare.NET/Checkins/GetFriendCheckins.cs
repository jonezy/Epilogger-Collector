using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Checkins
{
    public class GetFriendCheckins
    {
        private const string URL = "http://api.foursquare.com/v1/checkins";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns a list of recent checkins from friends.\n
        /// If you pass in a geolat/geolong pair (optional, but recommended), we'll send you back a distance inside each checkin object that you can use to sort your results.
        /// </summary>
        public GetFriendCheckins()
        {
            
        }

        public List<Schema.Checkin> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.CheckinsObj>(result).Checkins;
        }

        #region Parameters

        private Parameter _Geolat = new Parameter("Geolat", "geolat", false);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", false);

        /// <summary>
        /// (optional, but recommended)
        /// </summary>
        public double Geolat { set { _Geolat.Value = value.ToString(); } }

        /// <summary>
        /// (optional, but recommended)
        /// </summary>
        public double Geolong { set { _Geolong.Value = value.ToString(); } }

        #endregion
    }
}
