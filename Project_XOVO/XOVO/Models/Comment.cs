using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class Comment
    {
        public string UserName { get; set; }
        public int FeedID { get; set; }
        public string Content { get; set; }

        public Comment() : this("",0, "") { }

        public Comment(string username, int feedid, string content)
        {
            this.UserName = username;
            this.FeedID = feedid;
            this.Content = content;
        }

    }
}