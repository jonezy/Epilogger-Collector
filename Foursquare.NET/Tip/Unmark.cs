using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Tip
{
    public class Unmark
    {
        private const string URL = "http://api.foursquare.com/v1/tip/unmark";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.POST;
        
        private List<string> Parameters = new List<string>();

        /// <summary>
        /// Allows you to unmark a tip (it will revert the previous action, if there was any on the tip). For example, if a tip was on your to-do list, it would be taken off.
        /// </summary>
        public Unmark()
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
