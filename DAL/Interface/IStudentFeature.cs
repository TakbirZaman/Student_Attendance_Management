using DAL.EF.Models;

namespace DAL.Interface
{
    public interface IStudentFeature
    {
      
        Student GetWithAttendance(int id);
       
        Student FindByName(string name);

        Student FindByNameWithAttendances(string name);
        Student HigestAttendance();
        public List<Student> Top3Attendance();

        public List<Student> Low3Attendance();
        Student LowestAttendance();
    }
}
