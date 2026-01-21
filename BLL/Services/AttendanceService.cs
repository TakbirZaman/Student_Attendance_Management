using BLL.DTOs;
using DAL;
using DAL.EF.Models;
using System.Collections.Generic;

namespace BLL.Services
{
    public class AttendanceService
    {
        DataAccessFactory factory;
        public AttendanceService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public List<AttendanceDTO> Get()
        {
            var data = factory.AttendanceData().Get();
            var mapper = MapperConfig.GetMapper();
            var ret = mapper.Map<List<AttendanceDTO>>(data);
            return ret;
        }

        public AttendanceDTO Get(int id)
        {
            return MapperConfig.GetMapper().Map<AttendanceDTO>(factory.AttendanceData().Get(id));
        }

        public bool Create(AttendanceDTO a)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Attendance>(a);
            return factory.AttendanceData().Create(data);
        }

        public bool Update(AttendanceDTO a)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Attendance>(a);
            return factory.AttendanceData().Update(data);
        }

        public bool Delete(int id)
        {
            return factory.AttendanceData().Delete(id);
        }
    }
}
