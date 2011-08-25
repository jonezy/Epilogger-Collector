using System.Collections.Generic;
using System.Web.Script.Serialization;
using FoursquareNET.Schema;

namespace FoursquareNET.Tip
{
    public class Add
    {
        private const string URL = "http://api.foursquare.com/v1/addtip";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to add a new tip or to-do at a venue.
        /// </summary>
        public Add()
        {
            
        }

        public Schema.Tip Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_VenueId.ToString());
            Parameters.Add(_Text.ToString());
            Parameters.Add(_Type.ToString());
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<TipObj>(result).Tip;
        }

        #region Parameters

        private Parameter _VenueId = new Parameter("VenueId", "vid", true);
        private Parameter _Text = new Parameter("Text", "text", true);
        private Parameter _Type = new Parameter("Type", "type", false);
        private Parameter _Geolat = new Parameter("Geolat", "geolat", false);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", true);

        /// <summary>
        /// the venue where you want to add this tip (required)
        /// </summary>
        public int VenueId { set { _VenueId.Value = value.ToString(); } }

        /// <summary>
        /// the text of the tip or to-do item (required)
        /// </summary>
        public string Text { set { _Text.Value = value; } }

        /// <summary>
        /// specify one of 'tip' or 'todo' (optional, default: tip)
        /// </summary>
        public TipType? TipType { set { _Type.Value = value != null ? value.ToString() : string.Empty; } }

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
