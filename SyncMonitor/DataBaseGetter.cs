using Couchbase.Lite;
using System;
using System.Collections.Generic;

namespace SyncMonitor
{
	public interface IDataBaseGetter
	{
		Database Get();
	}
	public class DataBaseGetter : IDataBaseGetter
	{
		private readonly IConflictResolver _conflictResolver;
		private readonly IDataBaseIndexesCreator _dataBaseIndexesCreator;

		private Database _database = null;
		private List<string> indexes = null;

		public DataBaseGetter()
		{
			_conflictResolver = new DataBaseConfictResolver();
			_dataBaseIndexesCreator = new DataBaseIndexesCreator();
		}

		public Database Get()
		{
			try
			{
				if (_database != null)
					return _database;

				string databaseName = "mobilotis";

				DatabaseConfiguration config = new DatabaseConfiguration
				{
					ConflictResolver = _conflictResolver
				};
				_database =  new Database(databaseName, config);

				if (indexes == null)
				{
					indexes = _dataBaseIndexesCreator.Create(_database);
				}

				return _database;
			}
			catch (Exception )
			{
				return null;
			}
		}
	}
}
