using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Venue
{
    public class NearbyAndSearch
    {
        private const string URL = "http://api.foursquare.com/v1/venues";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns a list of venues near the area specified or that match the search term. (The distance returned is in meters). 
        /// If you authenticate, the method will return venue meta-data related to you and your friends. 
        /// If you do not authenticate, you will not get this data.
        /// </summary>
        public NearbyAndSearch()
        {
        }

        public List<Schema.VenueGroup> Execute()
        {
            return Execute(new Credential.CredentialBase());
        }

        public List<Schema.VenueGroup> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());
            Parameters.Add(_Limit.ToString());
            Parameters.Add(_SearchTerm.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.VenuesObj>(result).Groups;
        }

        #region Parameters

        private Parameter _Geolat = new Parameter("Geolat", "geolat", true);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", true);
        private Parameter _Limit = new Parameter("Limit", "l", false);
        private Parameter _SearchTerm = new Parameter("SearchTerm", "q", false);
        

        /// <summary>
        /// latitude (required)
        /// </summary>
        public string Geolat { set { _Geolat.Value = value; } }

        /// <summary>
        /// longitude (required)
        /// </summary>
        public string Geolong { set { _Geolong.Value = value; } }

        /// <summary>
        /// limit of results (optional, default 10, maximum 50)
        /// </summary>
        public int? Limit { set { _Limit.Value = value != null ? value.ToString() : string.Empty; } }

        /// <summary>
        /// keyword search (optional)
        /// </summary>
        public string SearchTerm { set { _SearchTerm.Value = value; } }

        #endregion
    }
}
