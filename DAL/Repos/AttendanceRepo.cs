using DAL.EF;
using DAL.EF.Models;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    internal class AttendanceRepo : IRepository<Attendance>
    {
        SMSContext db;

        public AttendanceRepo(SMSContext db)
        {
            this.db = db;
        }

        public bool Create(Attendance a)
        {
            db.Attendances.Add(a);
            return db.SaveChanges() > 0;
        }

        public List<Attendance> Get()
        {
            return db.Attendances.ToList();
        }
        


        public Attendance Get(int id)
        {
            return db.Attendances.Find(id);
        }

        public bool Update(Attendance a)
        {
            var ex = Get(a.Id);
            db.Entry(ex).CurrentValues.SetValues(a);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var ex = Get(id);
            db.Attendances.Remove(ex);
            return db.SaveChanges() > 0;
        }

     
    }
}
