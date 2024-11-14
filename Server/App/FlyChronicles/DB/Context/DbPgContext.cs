using System;
using System.Reflection;
using FlyChronicles.DB.Model;
using FlyChronicles.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyChronicles.DB.Context
{
    /// <summary>
    /// Postgresql context
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="options"></param>
    public class DbPgContext(DbContextOptions<DbPgContext> options) : DbContext(options)
    {
        /// <summary>
        /// settings set
        /// </summary>
        public DbSet<Settings> Settings { get; set; }

        /// <summary>
        /// create models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration(new EntityConfiguration<Settings>());

            foreach (var type in Assembly.GetAssembly(typeof(Entity)).GetTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var configType = typeof(EntityConfiguration<>).MakeGenericType(type);
                    var config = Activator.CreateInstance(configType);
                    GetType().GetMethod(nameof(ApplyConf), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                        .MakeGenericMethod(type).Invoke(this, [modelBuilder, config]);

                }
            }
        }

        /// <summary>
        /// ApplyConfiguration generic wrapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="config"></param>
        private static void ApplyConf<T>(ModelBuilder modelBuilder, EntityConfiguration<T> config) where T : class, IEntity
        {
            modelBuilder.ApplyConfiguration(config);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    }
}
