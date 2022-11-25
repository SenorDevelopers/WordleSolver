using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database;

public class UoW : DbContext
{
	public DbSet<Word> Words { get; set; } = null!;
	public DbSet<Guess> Guesses { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var configBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
		IConfiguration configuration = configBuilder.Build();
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
	}
}
