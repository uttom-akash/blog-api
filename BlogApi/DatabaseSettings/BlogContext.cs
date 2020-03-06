using System;
using System.Diagnostics.CodeAnalysis;
using Blog_Rest_Api.Persistent_Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog_Rest_Api{
    public class BlogContext:DbContext{
        public readonly IOptions<DatabaseInfo> databaseInfo;
        public readonly ILoggerFactory loggerFactory;

        public BlogContext([NotNullAttribute] DbContextOptions options,IOptions<DatabaseInfo> databaseInfo) : base(options)
        {
              this.databaseInfo=databaseInfo;
              loggerFactory=LoggerFactory.Create(builder=>builder.AddConsole());

        }

        public DbSet<Story> Stories {get;set;}
        public DbSet<User> Users {get;set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer($"Data Source={databaseInfo.Value.Host};Initial Catalog={databaseInfo.Value.DatabaseName};Integrated Security=True")
                    .UseLoggerFactory(loggerFactory);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
        }
    }
}