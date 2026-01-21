using AutoMapper;
using BLL.DTOs;
using DAL.EF.Models;

namespace BLL
{
    public class MapperConfig
    {
        static MapperConfiguration cfg = new MapperConfiguration(cfg =>
        {
            // Add all mappings here
            cfg.CreateMap<Student, StudentDTO>().ReverseMap();
            cfg.CreateMap<Class, ClassDTO>().ReverseMap();
            cfg.CreateMap<Attendance, AttendanceDTO>().ReverseMap();
            cfg.CreateMap<Student, StudentAttendanceDTO>().ReverseMap();
        });

        public static Mapper GetMapper()
        {
            return new Mapper(cfg);
        }
    }
}
