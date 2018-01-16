using Couchbase.Lite;
using Newtonsoft.Json;

namespace SyncMonitor
{
    public class Server
	{
		private string _login;
		private string _password;
		private string _urlServer;

		[JsonProperty("login")]
		public string Login { get => _login; set { _login = value;  } }
		[JsonProperty("password")]
		public string Password { get => _password; set { _password = value;  } }
		[JsonProperty("urlServer")]
		public string UrlServer { get => _urlServer; set { _urlServer = value;  } }
		[JsonProperty("table")]
		public string Table { get => "server"; }
		public string Id { get; set; }

		public MutableDictionary DocumentInitialize()
		{
			MutableDictionary dico = new MutableDictionary();

			dico.SetString("table", Table);
			dico.SetString("login", Login);
			dico.SetString("password", Password);
			dico.SetString("urlServer", UrlServer);
			dico.SetString("id", Id);		

			return dico;
		}
	}
}
