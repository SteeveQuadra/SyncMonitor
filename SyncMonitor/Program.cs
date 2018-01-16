using Couchbase.Lite.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncMonitor
{
	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				if (args.Length != 3)
				{
					Console.WriteLine("Usage : SyncMonitor [userName] [password] [Url sync Gateway]");
					return 1;
				}
				Couchbase.Lite.Support.NetDesktop.Activate();

				Server serverInfo = new Server();
				serverInfo.Login = args[0];
				serverInfo.Password = args[1];
				serverInfo.UrlServer = args[2];

				IDataBaseGetter dataBaseGetter = new DataBaseGetter();

				if (dataBaseGetter == null)
				{
					Console.WriteLine("Error when to create the local database");
					return 2;
				}


				IReplicatorGetter replicatorGetter = new ReplicatorGetter(dataBaseGetter, serverInfo);
				Replicator replicator = replicatorGetter.Get();

				IDataBaseLoader dataBaseLoader = new DataBaseLoader(dataBaseGetter);
				while (!Console.KeyAvailable)
				{
					if (Console.ReadKey(true).Key == ConsoleKey.I)
						Console.WriteLine(dataBaseLoader.Resume());

					if (Console.ReadKey(true).Key == ConsoleKey.A)
					{
						int i = 1;
						List<string> acquereurs = dataBaseLoader.Acquereurs();
						foreach (string nom in acquereurs)
							Console.WriteLine($"{i++} {nom}");
					}

					if (Console.ReadKey(true).Key == ConsoleKey.R)
					{
						Console.WriteLine("Stopping the replicator");
						replicator.Stop();
						Console.WriteLine("Waiting 2 seconds ...");
						Task.Delay(2000).Wait();
						Console.WriteLine("starting the replicator");
						replicator.Start();

					}
					if (Console.ReadKey(true).Key == ConsoleKey.Escape)
					break;

					Task.Delay(500).Wait();
				}

				return 0;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				return 3;
			}
			return 0;
		}
	}
}
