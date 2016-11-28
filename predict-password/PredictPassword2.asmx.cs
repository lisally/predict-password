using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace predict_password
{
    /// <summary>
    /// Summary description for PredictPassword2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class PredictPassword2 : System.Web.Services.WebService
    {
        private static List<string> passwords;

        [WebMethod]
        //public string DownloadData(string filepath)
        public string DownloadData()

        {
            passwords = new List<string>();
            using (StreamReader reader = new StreamReader("C:/Users/iGuest/Downloads/passwords.txt"))
            {
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    passwords.Add(line);
                }
            }
            return "success downloading data";
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchForPasswords(string search)
        {
            List<string> results = new List<string>();

            var searchQuery = passwords.Select(result => result)
                .Where(result => result.StartsWith(search))
                .GroupBy(result => result)
                .OrderByDescending(result => result.Count())
                .Take(10)
                .ToList();

            for (int i = 0; i < searchQuery.Count(); i++)
            {
                results.Add(searchQuery[i].Key);
            }

            return new JavaScriptSerializer().Serialize(results);
        }
    }
}
