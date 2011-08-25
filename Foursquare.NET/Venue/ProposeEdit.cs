using System.Collections.Generic;

namespace FoursquareNET.Venue
{
    public class ProposeEdit
    {
        private const string URL = "http://api.foursquare.com/v1/venue/proposeedit";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        public ProposeEdit()
        {
            
        }

        public string Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_VenueId.ToString());
            Parameters.Add(_Name.ToString());
            Parameters.Add(_Address.ToString());
            Parameters.Add(_CrossStreet.ToString());
            Parameters.Add(_City.ToString());
            Parameters.Add(_State.ToString());
            Parameters.Add(_Zip.ToString());
            Parameters.Add(_Phone.ToString());
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return result;
        }

        #region Parameters

        private Parameter _VenueId = new Parameter("VenueId", "vid", true);
        private Parameter _Name = new Parameter("Name", "name", true);
        private Parameter _Address = new Parameter("Address", "address", true);
        private Parameter _CrossStreet = new Parameter("CrossStreet", "crossstreet", false);
        private Parameter _City = new Parameter("City", "city", true);
        private Parameter _State = new Parameter("State", "state", true);
        private Parameter _Zip = new Parameter("Zip", "zip", false);
        private Parameter _Phone = new Parameter("Phone", "phone", false);
        private Parameter _Geolat = new Parameter("Geolat", "geolat", true);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", true);

        /// <summary>
        /// (required) the venue for which you want to propose an edit
        /// </summary>
        public int VenueId { set { _VenueId.Value = value.ToString(); } }

        /// <summary>
        /// (required) the name of the venue
        /// </summary>
        public string Name { set { _Name.Value = value; } }

        /// <summary>
        /// (required) the address of the venue (e.g., "202 1st Avenue")
        /// </summary>
        public string Address { set { _Address.Value = value; } }

        /// <summary>
        /// the cross streets (e.g., "btw Grand & Broome")
        /// </summary>
        public string CrossStreet { set { _CrossStreet.Value = value; } }

        /// <summary>
        /// (required) the city name where this venue is
        /// </summary>
        public string City { set { _City.Value = value; } }

        /// <summary>
        /// (required) the state where the city is
        /// </summary>
        public string State { set { _State.Value = value; } }

        /// <summary>
        /// (optional) the ZIP code for the venue
        /// </summary>
        public string Zip { set { _Zip.Value = value; } }

        /// <summary>
        /// (optional) the phone number for the venue
        /// </summary>
        public string Phone { set { _Phone.Value = value; } }

        /// <summary>
        /// (required)
        /// </summary>
        public string Geolat { set { _Geolat.Value = value; } }

        /// <summary>
        /// (required)
        /// </summary>
        public string Geolong { set { _Geolong.Value = value; } }

        #endregion
    }
}
