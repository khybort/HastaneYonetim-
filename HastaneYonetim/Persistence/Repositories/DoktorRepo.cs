using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Persistence.Repositories
{
    public class DoktorRepo : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoktorRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Doktor> GetDectors()
        {
            return _context.Doctors
                .Include(s => s.Specialization)
                .Include(u => u.Physician)
                .ToList();
        }

        /// <summary>
        /// Get the available doctors
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Doktor> GetAvailableDoctors()
        {
            return _context.Doctors
                .Where(a => a.IsAvailable == true)
                .Include(s => s.Specialization)
                .Include(u => u.Physician)
                .ToList();
        }
        /// <summary>
        /// Get single Doctor - Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Doktor GetDoctor(int id)
        {
            return _context.Doctors
                .Include(s => s.Specialization)
                .Include(u => u.Physician)
                .SingleOrDefault(d => d.Id == id);
        }

        public Doktor GetProfile(string userId)
        {
            return _context.Doctors
                .Include(s => s.Specialization)
                .Include(u => u.Physician)
                .SingleOrDefault(d => d.PhysicianId == userId);
        }
        public void Add(Doktor doctor)
        {
            _context.Doctors.Add(doctor);
        }
    }
}