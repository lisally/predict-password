using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
namespace predict_password
{
    /// <summary>
    /// Summary description for predict
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class predict : System.Web.Services.WebService
    {
        private List<string> passwords;
        private static Dictionary<string, int> dictionary;
        private string filePath = System.IO.Path.GetTempPath() + "\\data.txt";

        [WebMethod]
        public string DownloadData()

        {
            passwords = new List<string>();
            dictionary = new Dictionary<string, int>();
            File.Delete(this.filePath);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("info370");
            {
                //CloudBlockBlob blob = container.GetBlockBlobReference("password_data.txt");
                CloudBlockBlob blob = container.GetBlockBlobReference("passwords.txt");

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        passwords.Add(line);
                    }
                }

                var searchDictionary = passwords.GroupBy(x => x)
                .Select(x => new Tuple<string, int>(x.Key, x.Count()))
                .ToList();

                for (int j = 0; j < searchDictionary.Count(); j++)
                {
                    dictionary.Add(searchDictionary[j].Item1, searchDictionary[j].Item2);
                }

                passwords = null;

            }
            return "success downloading data";
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchForPasswords(string search)
        {
            List<string> results = new List<string>();

            var searchQuery = dictionary.Select(x => new Tuple<string, int>(x.Key, x.Value))
                .Where(x => x.Item1.StartsWith(search))
                .OrderByDescending(x => x.Item2)
                .Take(10)
                .ToList();

            for (int i = 0; i < searchQuery.Count(); i++)
            {
                results.Add(searchQuery[i].Item1);
                results.Add("" + searchQuery[i].Item2);

            }

            return new JavaScriptSerializer().Serialize(results);
        }
    }
}
