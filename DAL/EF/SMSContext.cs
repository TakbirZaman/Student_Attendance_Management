using DAL.EF.Models;
//using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class SMSContext : DbContext
    {
        public SMSContext(DbContextOptions<SMSContext> opt) : base(opt) { }
        public DbSet<Student> Students { get; set; }

        public DbSet<Class> Classs { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
    }
