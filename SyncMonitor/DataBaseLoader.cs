using Couchbase.Lite.Query;
using System.Collections.Generic;
using System.Linq;

namespace SyncMonitor
{
	public interface IDataBaseLoader
	{
		string Resume();
		List<string> Acquereurs();
	}

	public class DataBaseLoader : IDataBaseLoader
	{
		int clic = 0;
		private readonly IDataBaseGetter _dataBaseGetter;

		public DataBaseLoader(IDataBaseGetter dataBaseGetter)
		{
			_dataBaseGetter = dataBaseGetter;
		}

		

		public string Resume()
		{
			IQuery query = Query.Select(SelectResult.Expression(Expression.Property("payCode")))
			   .From(DataSource.Database(_dataBaseGetter.Get()))
			   .Where(Expression.Property("table").EqualTo("pays"));

			var rows = query.Execute();

			query = Query.Select(SelectResult.Expression(Expression.Property("finCode")))
			   .From(DataSource.Database(_dataBaseGetter.Get()))
			   .Where(Expression.Property("table").EqualTo("typefinancement"));

			var rowsFin = query.Execute();

			query = Query.Select(SelectResult.Expression(Expression.Property("sfaCode")))
			   .From(DataSource.Database(_dataBaseGetter.Get()))
			   .Where(Expression.Property("table").EqualTo("situationfamille"));

			var rowsSit = query.Execute();

			query = Query.Select(SelectResult.Expression(Expression.Property("Num")))
			   .From(DataSource.Database(_dataBaseGetter.Get()))
			   .Where(Expression.Property("table").EqualTo("commune"));

			var rowsCom = query.Execute();

			query = Query.Select(SelectResult.All())
			  .From(DataSource.Database(_dataBaseGetter.Get()))
			  .Where(Expression.Property("table").EqualTo("futuracquereur"));

			var rowsAq = query.Execute();

			

			return $"{clic++} : Pays={rows.Count}, Financement={rowsFin.Count}, Situation Familiale={rowsSit.Count}, Commune={rowsCom.Count}, Futur Acq={rowsAq.Count}";
		}

		public List<string> Acquereurs()
		{
			List<string> acquereurs = new List<string>();

			IQuery query = Query.Select(SelectResult.Expression(Expression.Property("aquereurPrincipal.nom")))
			  .From(DataSource.Database(_dataBaseGetter.Get()))
			  .Where(Expression.Property("table").EqualTo("futuracquereur"));

			var rowsAq = query.Execute();

			foreach (var row in rowsAq)
			{
				acquereurs.Add(row.GetString("nom"));
			}
			return acquereurs;
		}
	}
}
