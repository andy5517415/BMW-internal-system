using System;
using InternalSystem.Dotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace InternalSystem.Models
{
    public partial class MSIT44Context2 : MSIT44Context
    {
        public MSIT44Context2()
        {
        }

        public MSIT44Context2(DbContextOptions<MSIT44Context> options)
            : base(options)
        {
        }
        public virtual DbSet<Leftjoin> Leftjoin { get; set; }
        
        public virtual DbSet<MaleLeaveOver> MaleLeaveOver { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Leftjoin>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<MaleLeaveOver>(entity =>
            {
                entity.HasNoKey();



            });
        }
    }
}
