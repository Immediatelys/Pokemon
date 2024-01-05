using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CountryRepository : ICountryRepository
    {

        public readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountry(int countryID)
        {
            return _context.Countries.Where(c => c.Id == countryID).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByCountry(int countryId)
        {
            return _context.Owners.Where(c => c.Id == countryId).ToList();
        }

        public bool CountryExists(int id)
        { 
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }
    }
}
