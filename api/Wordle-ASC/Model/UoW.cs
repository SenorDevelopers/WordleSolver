using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Database;

public class UoW : DbContext
{
	public DbSet<Word> Words { get; set; } = null!;
	public DbSet<Guess> Guesses { get; set; } = null!;

	public UoW()
	{
		var databaseCreator = Database.GetService<IDatabaseCreator>() as IRelationalDatabaseCreator;
		if (databaseCreator != null)
		{
			if (!databaseCreator.CanConnect())
			{
				databaseCreator.Create();
			}

			if (!databaseCreator.HasTables())
			{
				databaseCreator.CreateTables();
			}
		}
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var configBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
		IConfiguration configuration = configBuilder.Build();
		optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
	}
}
