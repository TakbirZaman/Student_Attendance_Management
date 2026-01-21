using BLL.DTOs;
using DAL;
using DAL.EF.Models;

namespace BLL.Services
{
    public class StudentService
    {
        DataAccessFactory factory;

        public StudentService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public List<StudentDTO> Get()
        {
            var data = factory.StudentData().Get();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<StudentDTO>>(data);
        }


        public StudentDTO Get(int id)
        {
            var data = factory.StudentData().Get(id);
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<StudentDTO>(data);
        }

        public bool Create(StudentDTO s)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Student>(s);

            return factory.StudentData().Create(data);
        }


        public bool Update(StudentDTO s)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Student>(s);
            return factory.StudentData().Update(data);
        }

        public bool Delete(int id)
        {
            return factory.StudentData().Delete(id);
        }


        public StudentAttendanceDTO GetWithAttendance(int id)
        {
            var data = factory.StudentAttendance().GetWithAttendance(id);

            var ret = MapperConfig.GetMapper().Map<StudentAttendanceDTO>(data);

            return ret;
        }




        public StudentDTO FindByName(string name)
        {
            var data = factory.StudentAttendance().FindByName(name);
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<StudentDTO>(data);
        }

        public StudentAttendanceDTO FindByNameWithAttendances(string name)
        {
            var data = factory.StudentAttendance().FindByNameWithAttendances(name);
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<StudentAttendanceDTO>(data);
        }
        public StudentAttendanceDTO HighestAttendance()
        {
            var data = factory.StudentAttendance().HigestAttendance();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<StudentAttendanceDTO>(data);
        }

        public List<StudentAttendanceDTO> Top3Attendance()
        {
            var data = factory.StudentAttendance().Top3Attendance();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<StudentAttendanceDTO>>(data);
        }

        public List<StudentAttendanceDTO> Low3Attendance()
        {
            var data = factory.StudentAttendance().Low3Attendance();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<StudentAttendanceDTO>>(data);
        }

        public StudentAttendanceDTO LowestAttendance()
        {
            var data = factory.StudentAttendance().LowestAttendance();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<StudentAttendanceDTO>(data);
        }
    }
}
