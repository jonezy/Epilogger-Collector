using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace FoursquareNET.Venue
{
    public class GetDetail
    {
        private const string URL = "http://api.foursquare.com/v1/venue";
        private const string URLv2 = "https://api.foursquare.com/v2/venues";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        public GetDetail()
        {
            
        }

        public Schema.Venue Execute()
        {
            return Execute(new Credential.CredentialBase());
        }

        public Schema.Venue Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_VenueId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.VenueObj>(result).Venue;
        }

        //public Schema.Venue Executev2(string ClientSecret)
        //{
        //    Parameters.Clear();
        //    Parameters.Add(_VenueId.ToString());
        //    Parameters.Add(ClientSecret);

        //    //var result = Common.HttpPost(URLv2, Parameters, credential, HttpRequestMethod);

        //    //return new JavaScriptSerializer().Deserialize<Schema.VenueObj>(result).Venue;


        //    //Parameters.Clear();


        //    ////var result = Common.HttpPost(TheURL, 

        //    //HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(TheURL);

        //    //HttpWebResponse response = (HttpWebResponse)req.GetResponse();

        //    //string result = "";
        //    //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //    //{
        //    //    result = sr.ReadToEnd();
        //    //}

        //    //return new JavaScriptSerializer().Deserialize<Schema.VenueObj>(result).Venue;
        //}

        #region Parameters

        private Parameter _VenueId = new Parameter("VenueId", "vid", false);

        /// <summary>
        /// the ID for the venue for which you want information 
        /// </summary>
        public int VenueId { set { _VenueId.Value = value.ToString(); } }

        #endregion
    }
}
