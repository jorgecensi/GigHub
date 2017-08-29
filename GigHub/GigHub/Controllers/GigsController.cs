using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly GenreRepository _genreRepository;
        private readonly FollowingRepository _followingRepository;


        public GigsController()
        {
            _context = new ApplicationDbContext();
            _gigRepository = new GigRepository(_context);
            _attendanceRepository = new AttendanceRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _followingRepository = new FollowingRepository(_context);
        }

        [Authorize]
        public ActionResult Mine()
        {
            return View(_gigRepository.GetFutureUserGigs(User.Identity.GetUserId()));
        }



        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel()
            {
                UpcommingGigs = _gigRepository.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);

        }





        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _genreRepository.GetAllGenres(),
                Heading = "Add a Gig"

            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {

            var gig = _gigRepository.GetGig(id);
            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = _genreRepository.GetAllGenres(),
                Date = gig.DateTime.ToString("dd/MM/yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Genre = gig.GenreId

            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _attendanceRepository.GetAttendance(gig.Id, userId) != null;

                viewModel.IsFollowing = _followingRepository.GetFollowing(gig.ArtistId, userId) != null;
            }



            return View("Details", viewModel);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _genreRepository.GetAllGenres();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _genreRepository.GetAllGenres();
                return View("GigForm", viewModel);
            }

            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();


            gig.Modify(viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);


            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");

        }
    }
}
