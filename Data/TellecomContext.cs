using System;
using System.Collections.Generic;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data;

public partial class TelecomContext : DbContext
{
    public TelecomContext()
    {
    }

    public TelecomContext(DbContextOptions<TelecomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<ServiceContract> ServiceContracts { get; set; }

    public virtual DbSet<ServiceStatistic> ServiceStatistics { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<TariffPlan> TariffPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceContract>(entity =>
        {
            entity.HasKey(e => e.ContractId);
        });

        modelBuilder.Entity<TariffPlan>(entity =>
        {
            entity.HasKey(e => e.Name);
        });
    }

}
