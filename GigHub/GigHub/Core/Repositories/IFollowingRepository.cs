using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        void Add(Following following);
        void Remove(Following following);
        Following GetFollowing(string artistId, string userId);
        IEnumerable<ApplicationUser> GetFollowings(string userId);
    }
}