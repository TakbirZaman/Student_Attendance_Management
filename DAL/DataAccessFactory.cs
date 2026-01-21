using DAL.EF;
using DAL.EF.Models;
using DAL.Interface;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataAccessFactory
    {
        SMSContext db;
        public DataAccessFactory(SMSContext db)
        {
            this.db = db;
        }
        public IRepository<Class> ClassData()
        {
            return new ClassRepo(db);
        }


        public IRepository<Student> StudentData()
        {
            return new StudentRepo(db);
        }

        public IRepository<Attendance> AttendanceData()
        {
            return new AttendanceRepo(db);




        }
        public IStudentFeature StudentAttendance()
        {
            return new StudentRepo(db);
        }

    }
}
