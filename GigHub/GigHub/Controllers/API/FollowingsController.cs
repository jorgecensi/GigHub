using GigHub.Core;
using GigHub.Core.DTOs;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDTO dto)
        {
            var userId = User.Identity.GetUserId();

            if (_unitOfWork.Followings.GetFollowing(dto.FolloweeId, userId) != null)
            {
                return BadRequest("Following already exists.");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };
            _unitOfWork.Followings.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _unitOfWork.Followings.GetFollowing(id, userId);


            if (following == null)
                return NotFound();


            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();


            return Ok(id);

        }
    }
}
