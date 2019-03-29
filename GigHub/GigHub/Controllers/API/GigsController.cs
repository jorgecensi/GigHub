using GigHub.Core;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGigWithAttendees(id);

            if (gig == null || gig.IsCanceled)
                return NotFound();

            if (gig.ArtistId != userId)
                return Unauthorized();

            gig.Cancel();

            _unitOfWork.Complete();

            return Ok();

        }

        [HttpGet]
        public IEnumerable<Gig> Gigs()
        {
            var gigs = _unitOfWork.Gigs.GetUpcomingGigs();
            return gigs;
        }

        [HttpPost]
        public IHttpActionResult PostGig(Gig gigViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = gigViewModel.DateTime,
                GenreId = gigViewModel.GenreId,
                Venue = gigViewModel.Venue
            };
            
            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = gig.Id }, gig);
        }

        [HttpPut]
        public IHttpActionResult UpdateGig(int id, Gig gigViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != gigViewModel.Id)
            {
                return BadRequest();
            }
            var userId = User.Identity.GetUserId();
            if (userId != gigViewModel.ArtistId)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(gigViewModel.Id);
            if (gig == null)
                return NotFound();
            if (gig.ArtistId != User.Identity.GetUserId())
                return BadRequest();
            gig.Modify(gigViewModel.DateTime, gigViewModel.Genre.Id, gigViewModel.Venue);
            _unitOfWork.Complete();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
