using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Tip
{
    public class MarkDone
    {
        private const string URL = "http://api.foursquare.com/v1/tip/markdone";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to mark a tip as done.
        /// </summary>
        public MarkDone()
        {
            
        }

        public Schema.Tip Execute(Credential.CredentialBase credential)
        {
            Parameters.Clear();
            Parameters.Add(_TipId.ToString());

            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.TipObj>(result).Tip;
        }

        #region Parameters

        private Parameter _TipId = new Parameter("TipId", "tid", true);

        /// <summary>
        /// the tip that you want to mark to-do (required)
        /// </summary>
        public int TipId { set { _TipId.Value = value.ToString(); } }
        
        #endregion
    }
}
