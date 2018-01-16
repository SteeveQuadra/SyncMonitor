using Couchbase.Lite;

namespace SyncMonitor
{

	public class DataBaseConfictResolver : IConflictResolver
	{
		public Document Resolve(Conflict conflict) // db20
		//public ReadOnlyDocument Resolve(Conflict conflict) // db19
		{
			var baseProperties = conflict.Base;
			var mine = conflict.Mine;
			var theirs = conflict.Theirs;

			return theirs;
		}
	}
}
