using Couchbase.Lite;
using Couchbase.Lite.Sync;
using System;


namespace SyncMonitor
{
	public interface IReplicatorGetter
	{
		Replicator Get();
	}

	public class ReplicatorGetter : IReplicatorGetter
	{
		private readonly IDataBaseGetter _dataBaseGetter;

		private Replicator _replicator = null;
		private ListenerToken _token;
		private Server _serverInfo = null;

		public ReplicatorGetter(IDataBaseGetter dataBaseGetter, Server serverInfo)
		{
			_dataBaseGetter = dataBaseGetter;
			_serverInfo = serverInfo;
		}

		private void Restart()
		{
			try
			{
				_replicator.RemoveChangeListener(_token);
				_replicator.Stop();
				_replicator = null;
				_replicator.Dispose();
			}
			catch(Exception)
			{ }

			Connect();

		}
				

		public Replicator Get()
		{
			try
			{
				if (_replicator != null)
				{
					return _replicator;
				}

				Connect();

				return _replicator;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void Connect()
		{
			Uri target = new Uri($"{_serverInfo.UrlServer}");
			ReplicatorConfiguration replicationConfig = new ReplicatorConfiguration(_dataBaseGetter.Get(), target);
			replicationConfig.Continuous = true;
			replicationConfig.ReplicatorType = ReplicatorType.PushAndPull;
			replicationConfig.Authenticator = new BasicAuthenticator(_serverInfo.Login, _serverInfo.Password);
			_replicator = new Replicator(replicationConfig);
			_replicator.Start();
			_token = _replicator.AddChangeListener(_replicator_StatusChanged);
		}

		private void _replicator_StatusChanged(object sender, ReplicationStatusChangedEventArgs e)
		{
			try
			{
				Console.WriteLine($"Replicator Satus : {e.Status.Activity} | {e.Status.Progress.Completed}/{e.Status.Progress.Total}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
