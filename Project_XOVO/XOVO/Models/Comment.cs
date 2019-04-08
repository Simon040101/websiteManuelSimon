using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class Comment
    {
        public int UserID { get; set; }
        public int FeedID { get; set; }
        public string Content { get; set; }

        public Comment() : this(0,0, "") { }

        public Comment(int userid, int feedid, string content)
        {
            this.UserID = userid;
            this.FeedID = feedid;
            this.Content = content;
        }

    }
}