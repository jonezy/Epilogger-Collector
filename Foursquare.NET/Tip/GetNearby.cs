using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Tip
{
    public class GetNearby
    {
        private const string URL = "http://api.foursquare.com/v1/tips";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns a list of tips near the area specified. (The distance returned is in meters).
        /// </summary>
        public GetNearby()
        {
            
        }

        public List<Schema.TipGroup> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());
            Parameters.Add(_Limit.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.TipsObj>(result).Groups;
        }

        #region Parameters

        private Parameter _Geolat = new Parameter("Geolat", "geolat", true);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", true);
        private Parameter _Limit = new Parameter("Limit", "l", false);

        /// <summary>
        /// latitude (required)
        /// </summary>
        public string Geolat { set { _Geolat.Value = value; } }

        /// <summary>
        /// longitude (required)
        /// </summary>
        public string Geolong { set { _Geolong.Value = value; } }

        /// <summary>
        /// limit of results (optional, default 30)
        /// </summary>
        public int? Limit { set { _Limit.Value = value != null ? value.ToString() : string.Empty; } }

        #endregion
    }
}
