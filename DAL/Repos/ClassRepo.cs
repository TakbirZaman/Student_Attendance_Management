using DAL.EF;
using DAL.EF.Models;
using DAL.Interface;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    internal class ClassRepo :IRepository<Class> 
    {
        SMSContext db;

        public ClassRepo(SMSContext db)
        {
            this.db = db;
        }

        public bool Create(Class c)
        {
            db.Classs.Add(c);
            return db.SaveChanges() > 0;
        }

        public List<Class> Get()
        {
            return db.Classs.ToList();
        }


        public Class Get(int id)
        {
            return db.Classs.Find(id);
        }

        public bool Update(Class c)
        {
            var ex = Get(c.Id);
            db.Entry(ex).CurrentValues.SetValues(c);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var ex = Get(id);
            db.Classs.Remove(ex);
            return db.SaveChanges() > 0;
        }
    }
}
