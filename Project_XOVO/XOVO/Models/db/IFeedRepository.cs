﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOVO.Models.db
{
    interface IFeedRepository
    {
        void Open();
        void Close();
        List<FeedItem> GetFeedItems();
        List<FeedItem> GetFeedItemsByID(int id);
        bool CheckIfLiked(int uid, int fid);
        User GetUserById(int id);
        bool InsertFeedItem(FeedItem itemToInsert);
        bool UserLikeFeed(int UserID, int feedID);
        int CountLike(int feedID);
        bool UserCommentFeed(string Username, int feedID, string comment);
        bool DeleteFeed(int id);
        List<Comment> GetAllComments(int feedID);


    }
}
