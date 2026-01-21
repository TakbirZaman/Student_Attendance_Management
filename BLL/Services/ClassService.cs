using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.EF.Models;

namespace BLL.Services
{
    public class ClassService
    {
        DataAccessFactory factory;

        public ClassService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public List<ClassDTO> Get()
        {
            var data = factory.ClassData().Get();
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<List<ClassDTO>>(data);
        }

        public ClassDTO Get(int id)
        {
            var data = factory.ClassData().Get(id);
            var mapper = MapperConfig.GetMapper();
            return mapper.Map<ClassDTO>(data);
        }

        public bool Create(ClassDTO c)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Class>(c);
            return factory.ClassData().Create(data);
        }

        public bool Update(ClassDTO c)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Class>(c);
            return factory.ClassData().Update(data);
        }

        public bool Delete(int id)
        {
            return factory.ClassData().Delete(id);
        }
    }
}
