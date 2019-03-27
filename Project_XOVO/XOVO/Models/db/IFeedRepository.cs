using System;
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

        bool InsertFeedItem(FeedItem itemToInsert);
        bool UserLikeFeed(int UserID, int feedID);
        int CountLike(int feedID);

    }
}
