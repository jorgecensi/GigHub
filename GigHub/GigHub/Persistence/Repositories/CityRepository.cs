using GigHub.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class CityRepository
    {
        private readonly IApplicationDbContext _context;

        public CityRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetAllCities()
        {
            return _context.Cities.ToList();
        }

        public City GetCity(int id)
        {
            return _context.Cities.SingleOrDefault(c => c.Id == id);
        }

        public void Add(City city)
        {
            _context.Cities.Add(city);
        }

        public void Remove(City city)
        {
            _context.Cities.Remove(city);
        }
    }
}
