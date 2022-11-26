using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Database;

public class UoW : DbContext
{
	public DbSet<Word> Words { get; set; } = null!;
	public DbSet<Guess> Guesses { get; set; } = null!;

	public UoW(DbContextOptions<UoW> dbContextOptions) : base(dbContextOptions)
	{
		try
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
					//RestoreDatabase();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message); // CHAGE TO LOGGER
		}
	}

	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//	var configBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
	//	IConfiguration configuration = configBuilder.Build();

	//	optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
	//}


	//public static void RestoreDatabase()
	//{
	//	var uoW = new UoW();
	//	const string backupFile = "C:/Users/silvi/Desktop/WordleSolver/api/Wordle-ASC/Database.bak";

	//	const string sqlCommand = @"RESTORE DATABASE [{0}] FROM DISK = '{1}'";

	//	uoW.Database.ExecuteSqlRaw(string.Format(sqlCommand, "WordleASC_TEST2", backupFile));
	//}
}
