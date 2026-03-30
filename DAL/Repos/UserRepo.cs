using DAL.EF;
using DAL.EF.Models;
using DAL.Interfaces;

namespace DAL.Repos
{
    public class UserRepo : IRepository<User>
    {
        private readonly SMSContext db;

        public UserRepo(SMSContext db)
        {
            this.db = db;
        }

        public bool Create(User entity)
        {
            db.Users.Add(entity);
            return db.SaveChanges() > 0;
        }

        public List<User> Get()
        {
            return db.Users.ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public bool Update(User entity)
        {
            var ex = Get(entity.Id);
            if (ex != null)
            {
                db.Entry(ex).CurrentValues.SetValues(entity);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var ex = Get(id);
            if (ex != null)
            {
                db.Users.Remove(ex);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public User GetByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}