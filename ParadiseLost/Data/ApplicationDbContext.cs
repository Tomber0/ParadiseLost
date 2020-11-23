using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParadiseLost.Models;

namespace ParadiseLost.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //DbSet 
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Link> Links  { get; set; }
        public DbSet<Trip> Trips  { get; set; }
        public DbSet<User> Persons  { get; set; }
        public DbSet<Location> Locations  { get; set; }
        public DbSet<Message> Messages  { get; set; }  
        public DbSet<Images> Images  { get; set; }  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();

        }
    }
}
