using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Venue
{
    public class Add
    {
        private const string URL = "http://api.foursquare.com/v1/addvenue";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to add a new venue.
        /// </summary>
        public Add()
        {
            
        }

        public Schema.Venue Execute()
        {
            return Execute(new Credential.CredentialBase());
        }

        public Schema.Venue Execute(Credential.CredentialBase credential)
        {
            if (string.IsNullOrEmpty(_Address.Value) && (string.IsNullOrEmpty(_Geolat.Value) || string.IsNullOrEmpty(_Geolong.Value)))
                throw new Exception.OptionalParameterException("You must specify an Address or Geolat/Geolong pair");

            Parameters.Clear();
            Parameters.Add(_Name.ToString());
            Parameters.Add(_Address.ToString());
            Parameters.Add(_CrossStreet.ToString());
            Parameters.Add(_City.ToString());
            Parameters.Add(_State.ToString());
            Parameters.Add(_Zip.ToString());
            Parameters.Add(_Phone.ToString());
            Parameters.Add(_Geolat.ToString());
            Parameters.Add(_Geolong.ToString());
            Parameters.Add(_PrimaryCategoryId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.VenueObj>(result).Venue;
        }

        #region Parameters

        private Parameter _Name = new Parameter("Name", "name", false);
        private Parameter _Address = new Parameter("Address", "address", false);
        private Parameter _CrossStreet = new Parameter("CrossStreet", "crossstreet", false);
        private Parameter _City = new Parameter("City", "city", false);
        private Parameter _State = new Parameter("State", "state", false);
        private Parameter _Zip = new Parameter("Zip", "zip", false);
        private Parameter _Phone = new Parameter("Phone", "phone", false);
        private Parameter _Geolat = new Parameter("Geolat", "geolat", false);
        private Parameter _Geolong = new Parameter("Geolong", "geolong", false);
        private Parameter _PrimaryCategoryId = new Parameter("PrimaryCategoryId", "primarycategoryid", false);

        /// <summary>
        /// the name of the venue
        /// </summary>
        public string Name { set { _Name.Value = value; } }

        /// <summary>
        /// (optional) the address of the venue (e.g., "202 1st Avenue")
        /// </summary>
        public string Address { set { _Address.Value = value; } }

        /// <summary>
        /// (optional) the cross streets (e.g., "btw Grand & Broome")
        /// </summary>
        public string CrossStreet { set { _CrossStreet.Value = value; } }

        /// <summary>
        /// (optional) the city name where this venue is
        /// </summary>
        public string City { set { _City.Value = value; } }

        /// <summary>
        /// (optional) the state where the city is
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
        /// (optional, but recommended)
        /// </summary>
        public string Geolat { set { _Geolat.Value = value; } }

        /// <summary>
        /// (optional, but recommended)
        /// </summary>
        public string Geolong { set { _Geolong.Value = value; } }

        /// <summary>
        /// (optional) the ID of the category to which you want to assign this venue
        /// </summary>
        public string PrimaryCategoryId { set { _PrimaryCategoryId.Value = value; } }

        #endregion
    }
}
