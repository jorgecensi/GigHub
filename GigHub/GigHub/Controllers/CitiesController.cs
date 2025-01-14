using GigHub.Core;
using GigHub.Core.Models;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var cities = _unitOfWork.Cities.GetAllCities();
            return View(cities);
        }

        public ActionResult Edit(int id)
        {
            var city = _unitOfWork.Cities.GetCity(id);
            if (city == null)
                return HttpNotFound();

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(City city)
        {
            if (!ModelState.IsValid)
                return View(city);

            var cityInDb = _unitOfWork.Cities.GetCity(city.Id);
            if (cityInDb == null)
                return HttpNotFound();

            cityInDb.Name = city.Name;
            cityInDb.IsPartner = city.IsPartner;

            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(City city)
        {
            if (!ModelState.IsValid)
                return View(city);

            _unitOfWork.Cities.Add(city);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var city = _unitOfWork.Cities.GetCity(id);
            if (city == null)
                return HttpNotFound();

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var city = _unitOfWork.Cities.GetCity(id);
            if (city == null)
                return HttpNotFound();

            _unitOfWork.Cities.Remove(city);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}
