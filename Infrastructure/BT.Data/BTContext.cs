using BT.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace BT.Data;
public class BTContext : DbContext {
	public DbSet<Log> Logs { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder.UseSqlServer(@"Server=.\BUGRAME;Database=BT;Trusted_Connection=True;TrustServerCertificate=true");
	}
}