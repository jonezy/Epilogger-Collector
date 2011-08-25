using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Checkins
{
    public class History
    {
        private const string URL = "http://api.foursquare.com/v1/history";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;

        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Returns a history of checkins for the authenticated user.
        /// </summary>
        public History()
        {
            
        }

        public List<Schema.Checkin> Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_Limit.ToString());
            Parameters.Add(_SinceId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.CheckinsObj>(result).Checkins;
        }

        #region Parameters

        private Parameter _Limit = new Parameter("Limit", "l", false);
        private Parameter _SinceId = new Parameter("SinceId", "sinceid", false);

        /// <summary>
        /// limit of results (optional, default: 20, max: 250). number of checkins to return
        /// </summary>
        public int? Limit { set { _Limit.Value = value.HasValue ? value.ToString() : string.Empty; } }

        /// <summary>
        /// id to start returning results from (optional, if omitted returns most recent results)
        /// </summary>
        public string SinceId { set { _SinceId.Value = value; } }

        #endregion
    }
}
