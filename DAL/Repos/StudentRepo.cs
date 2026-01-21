using DAL.EF;
using DAL.EF.Models;
using DAL.Interface;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace DAL.Repos
{
    internal class StudentRepo : IRepository<Student>, IStudentFeature
    {
       SMSContext db;

        public StudentRepo(SMSContext db)
        {
            this.db = db;
        }

        public bool Create(Student s)
        {
            db.Students.Add(s);
            return db.SaveChanges() > 0;
        }

        public List<Student> Get()
        {
            return db.Students.ToList();
        }

       //
         public List<Student> GetWithAttendances()
        {
            return db.Students.Include(a=>a.Attendances).ToList();
        }
      


        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        public bool Update(Student s)
        {
            var ex = Get(s.Id);
            db.Entry(ex).CurrentValues.SetValues(s);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var ex = Get(id);
        
            db.Students.Remove(ex);
            return db.SaveChanges() > 0;
        }

        public Student GetWithAttendance(int id)
        {
            var att = (from a in db.Students.Include(a => a.Attendances)
                              where a.Id==id
                              select a).SingleOrDefault();
            return att;

        }

       



        public Student FindByName(string name)
        {
            var att= (from a in db.Students
                       where a.Name.Contains(name)
                      select a).SingleOrDefault();
            return att;
        }
        public Student FindByNameWithAttendances(string name)
        {
            var att = (from a in db.Students
                       where a.Name.Contains(name)
                       select a).SingleOrDefault();
            return att;
        }

        public Student HigestAttendance()
        { 
            var att = (from a in db.Students.Include(at=>at.Attendances )
                       orderby a.Attendances.Count() descending
                       select a).FirstOrDefault();
            return att;
        }


        
        public List<Student> Top3Attendance()
        {
            var att = (from s in db.Students.Include(x => x.Attendances)
                        orderby s.Attendances.Count() descending
                        select s).Take(3).ToList();

            return att;
        }

        
       
       

        public List<Student> Low3Attendance()
        {
            var att = (from s in db.Students.Include(x => x.Attendances)
                       orderby s.Attendances.Count() ascending
                       select s).Take(3).ToList();

            return att;
        }





        public Student LowestAttendance()
        {
            var att = (from a in db.Students.Include(at => at.Attendances )
                       orderby a.Attendances.Count() descending
                       select a).LastOrDefault();
            return att;
        }



    }
}
