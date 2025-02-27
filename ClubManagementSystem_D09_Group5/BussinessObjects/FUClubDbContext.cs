using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects
{
    public class FUClubDbContext : DbContext
    {
        public FUClubDbContext(DbContextOptions<FUClubDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Club> Clubs { get; set; }
        public DbSet<Models.ClubMember> ClubMembers { get; set; }
        public DbSet<Models.ClubRequest> ClubRequests { get; set; }
        public DbSet<Models.Notification> Notifications { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Models.TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Models.User> Users { get; set; }
    }
}
