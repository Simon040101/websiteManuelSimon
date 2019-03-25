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
        public  User UserForFeed { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string ImgPath { get; set; }
        public string FeedContent { get; set; }

        // ctors
        public FeedItem() : this(0, null, DateTime.MinValue, "", ""){ }

        public FeedItem(int feedId, User userForFeed, DateTime creationDateTime, string imgPath, string feedCont)
        {
            this.Id = feedId;
            this.UserForFeed = userForFeed;
            this.CreationDateTime = creationDateTime;
            this.ImgPath = imgPath;
            this.FeedContent = feedCont;
        }
        public FeedItem(int feedId, int userIdForFeed, DateTime creationDateTime, string imgPath, string feedCont)
        {
            this.Id = feedId;
            this.UserForFeed = new User();
            this.UserForFeed.ID = userIdForFeed;
            this.CreationDateTime = creationDateTime;
            this.ImgPath = imgPath;
            this.FeedContent = feedCont;
        }


    }
}