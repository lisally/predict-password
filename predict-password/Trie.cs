using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace predict_password
{
    public class Trie
    {
        public Node root { get; private set; }
        public List<string> passwords { get; private set; }

        public Trie()
        {
            this.root = new Node();
            this.passwords = new List<string>();
        }

        //public void DownloadData(string filepath)
        public void DownloadData()
        {
            using (StreamReader reader = new StreamReader("C:/Users/iGuest/Downloads/passwords.txt"))
            {
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    this.passwords.Add(line);
                }
            }
        }

        public void AddPassword(string password)
        {
            Node temp = root;
            string word = "";
            List<string> results = new List<string>();
            for (int i = 0; i < password.Length; i++)
            {
                word += password[i];
                if (temp.dictionary == null)
                {
                    temp.dictionary = new Dictionary<char, Node>();
                }

                if (!temp.dictionary.ContainsKey(password[i]))
                {
                    temp.dictionary.Add(password[i], new Node());
                    var searchQuery = this.passwords.Select(result => result)
                        .Where(result => result.StartsWith(word))
                        .GroupBy(result => result)
                        .OrderByDescending(result => result.Count())
                        .Take(10)
                        .ToList();

                    for (int j = 0; j < searchQuery.Count(); j++)
                    {
                        temp.dictionary[password[i]].list.Add(searchQuery[j].Key);
                    }
                }
                temp = temp.dictionary[password[i]];
            }
        }

        public void BuildTrie()
        {
            for (int i = 0; i < passwords.Count; i++)
            {
                this.AddPassword(passwords[i]);
            }
        }

        public List<string> SearchForResults(string search)
        {
            Node temp = root;
            string result = "";
            List<string> searchResults = new List<string>();

            for (int i = 0; i < search.Length; i++)
            {
                /* Checks if search exists in all of the search character's dictionaries
                Else return no results. */
                if (temp.dictionary.ContainsKey(search[i]))
                {
                    temp = temp.dictionary[search[i]];
                    result += search[i];
                }
                else
                {
                    break;
                }
            }

            // If all characters of search string exist in the Trie, search for results.
            if (search == result && search != "")
            {
                searchResults = temp.list;
            }
            return searchResults;
        }
    }
}