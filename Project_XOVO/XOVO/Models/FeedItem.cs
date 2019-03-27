using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class FeedItem
    {
        // Variablen and Konstanten
        public int Id { get; set; }

        // properties
        public  int UserForFeed { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string ImgPath { get; set; }
        public string FeedContent { get; set; }

        // ctors
        public FeedItem() : this(0, 0, DateTime.MinValue, "", ""){ }

        public FeedItem(int feedId, int userID, DateTime creationDateTime, string imgPath, string feedCont)
        {
            this.Id = feedId;
            this.UserForFeed = userID;
            this.CreationDateTime = creationDateTime;
            this.ImgPath = imgPath;
            this.FeedContent = feedCont;
        }



    }
}