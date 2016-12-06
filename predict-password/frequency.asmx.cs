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
    /// Summary description for frequency
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class frequency : System.Web.Services.WebService
    {

        private static Dictionary<string, string> noun;
        private static Dictionary<string, string> verb;
        private static Dictionary<string, string> name;
        private static Dictionary<string, string> cuss;
        private static Dictionary<string, string> adj;

        private string filePath = System.IO.Path.GetTempPath() + "\\data.txt";

        [WebMethod]
        public string DownloadData()

        {
            noun = new Dictionary<string, string>();
            verb = new Dictionary<string, string>();
            name = new Dictionary<string, string>();
            cuss = new Dictionary<string, string>();
            adj = new Dictionary<string, string>();

            File.Delete(this.filePath);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("info370");
            {
                CloudBlockBlob blob1 = container.GetBlockBlobReference("nounFreq.txt");
                CloudBlockBlob blob2 = container.GetBlockBlobReference("verbFreq.txt");
                CloudBlockBlob blob3 = container.GetBlockBlobReference("nameFreq.txt");
                CloudBlockBlob blob4 = container.GetBlockBlobReference("cussFreq.txt");
                CloudBlockBlob blob5 = container.GetBlockBlobReference("adjectiveFreq.txt");

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob1.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] list = line.Split(',');
                        noun.Add(list[0], list[1]);
                    }
                }

                File.Delete(this.filePath);

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob2.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] list = line.Split(',');
                        verb.Add(list[0], list[1]);
                    }
                }

                File.Delete(this.filePath);

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob3.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] list = line.Split(',');
                        name.Add(list[0], list[1]);
                    }
                }

                File.Delete(this.filePath);

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob4.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] list = line.Split(',');
                        cuss.Add(list[0], list[1]);
                    }
                }

                File.Delete(this.filePath);

                using (var fileStream = System.IO.File.OpenWrite(this.filePath))
                {
                    blob5.DownloadToStream(fileStream);
                }

                using (StreamReader reader = new StreamReader(this.filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] list = line.Split(',');
                        adj.Add(list[0], list[1]);
                    }
                }

            }
            return "success downloading data";
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Search(string search)
        {
            search = search.ToLower();
            List<string> results = new List<string>();
            string frequency = "";

            if (noun.ContainsKey(search)) {
                frequency = noun[search];
                results.Add("noun");
            }

            if (verb.ContainsKey(search))
            {
                frequency = verb[search];
                results.Add("verb");
            }

            if (name.ContainsKey(search))
            {
                frequency = name[search];
                results.Add("name");
            }

            if (cuss.ContainsKey(search))
            {
                frequency = cuss[search];
                results.Add("cuss");
            }

            if (adj.ContainsKey(search))
            {
                frequency = adj[search];
                results.Add("adj");
            }

            results.Add(frequency);

            if (results.Count() > 1)
            {
                return new JavaScriptSerializer().Serialize(results);
            }
            else
            {
                List<string> noResults = new List<string>();
                return new JavaScriptSerializer().Serialize(noResults);
            }
 
        }
    }
}
