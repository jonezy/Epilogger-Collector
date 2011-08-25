using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoursquareNET.Venue
{
    public class GetCategoryList
    {
        private const string URL = "http://api.foursquare.com/v1/categories";
        private const Common.HttpRequestMethod HttpRequestMethod = Common.HttpRequestMethod.GET;
        
        private List<string> Parameters = new List<string>();

        public GetCategoryList()
        {
            
        }

        public List<Schema.Category> Execute()
        {
            return Execute(new Credential.CredentialBase());
        }

        public List<Schema.Category> Execute(Credential.CredentialBase credential)
        {
            var result = Common.HttpPost(URL, Parameters, credential, HttpRequestMethod);

            return new JavaScriptSerializer().Deserialize<Schema.CategoriesObj>(result).Categories;
        }

        #region Parameters

        //no parameters

        #endregion
    }
}
