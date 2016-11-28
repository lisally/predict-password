using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace predict_password
{
    /// <summary>
    /// Summary description for PredictPassword
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PredictPassword : System.Web.Services.WebService
    {


        private static Trie trie;

        [WebMethod]
        public string buildTrie()
        {
            trie = new Trie();
            trie.DownloadData();
            trie.BuildTrie();
            return "success building trie";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string searchTrie(string search)
        {
            List<string> results = trie.SearchForResults(search.Trim());
            return new JavaScriptSerializer().Serialize(results);
        }

    }
}
