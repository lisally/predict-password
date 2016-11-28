using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace predict_password
{
    public class Node
    {
        public Dictionary<char, Node> dictionary { get; set; }
        public List<string> list { get; set; }

        public Node()
        {
            this.dictionary = null;
            this.list = new List<string>();
        }
    }
}