using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage2.Models;
using Microsoft.EntityFrameworkCore;
using Garage2.Models.Entities;

namespace Garage2.Data;

public class Garage2Context : DbContext
{
	public Garage2Context(DbContextOptions<Garage2Context> options)
		: base(options)
	{
	}

	public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;
	public DbSet<Member> Member { get; set; } = default!;
	public DbSet<VehicleType> VehicleType { get; set; } = default!;
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	modelBuilder.Entity<ParkedVehicle>()
	//		.HasOne(p => p.Member)
	//		.WithMany(m => m.ParkedVehicles)
	//		.HasForeignKey(p => p.MemberPersonNumber);
	//}
	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	modelBuilder.Entity<Member>()
	//		.HasMany(p => p.ParkedVehicle)
	//		.WithOne(m => m.Member)
	//		.HasForeignKey(f => f.MemberPersonNumber);

	//	modelBuilder.Entity<ParkedVehicle>()
	//		.HasOne(o => o.Member)
	//		.WithMany(v => v.ParkedVehicle)
	//		.HasForeignKey(f => f.MemberPersonNumber);

	//	modelBuilder.Entity<ParkedVehicle>()
	//		.HasOne(p => p.VehicleType)
	//		.WithMany(t => t.ParkedVehicle)
	//		.HasForeignKey(v => v.VehicleTypeId);
	//}
}