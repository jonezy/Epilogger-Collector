using System.Collections.Generic;

namespace FoursquareNET.Venue
{
    public class FlagClosed
    {
        private const string URL = "http://api.foursquare.com/v1/venue/flagclosed";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        public FlagClosed()
        {
            
        }

        public string Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_VenueId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);
            
            return result;
        }

        #region Parameters

        private Parameter _VenueId = new Parameter("VenueId", "vid", true);

        /// <summary>
        /// the venue that you want marked closed (required)
        /// </summary>
        public int VenueId { set { _VenueId.Value = value.ToString(); } }

        #endregion
    }
}
