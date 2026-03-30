using DAL.EF;
using DAL.EF.Models;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class AuditLogRepo : IRepository<AuditLog>
    {
        private readonly SMSContext db;

        public AuditLogRepo(SMSContext db)
        {
            this.db = db;
        }

        public bool Create(AuditLog entity)
        {
            db.AuditLogs.Add(entity);
            return db.SaveChanges() > 0;
        }

        public List<AuditLog> Get()
        {
            return db.AuditLogs.OrderByDescending(a => a.Timestamp).ToList();
        }

        public AuditLog Get(int id)
        {
            return db.AuditLogs.Find(id);
        }

        public bool Update(AuditLog entity)
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
                db.AuditLogs.Remove(ex);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// Get audit logs for a specific user
        /// </summary>
        public List<AuditLog> GetByUserId(int userId)
        {
            return db.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Get audit logs within a date range
        /// </summary>
        public List<AuditLog> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return db.AuditLogs
                .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
                .OrderByDescending(a => a.Timestamp)
                .ToList();
        }
    }
}
