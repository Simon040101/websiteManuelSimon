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
        public  int UserForFeedID { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Image { get; set; }
        public string FeedContent { get; set; }
        public int LikeCount { get; set; }
        public List<Comment> Comments { get; set; }
        
       

        // ctors
        public FeedItem() : this(0, 0, DateTime.MinValue, null, "",0, null){ }

        public FeedItem(int feedId, int userID, DateTime creationDateTime, string image, string feedCont, int likeCount, List<Comment> comments)
        {
            this.Id = feedId;
            this.UserForFeedID = userID;
            this.CreationDateTime = creationDateTime;
            this.Image = image;
            this.FeedContent = feedCont;
            this.LikeCount = likeCount;
            this.Comments = comments;
        }



    }
}