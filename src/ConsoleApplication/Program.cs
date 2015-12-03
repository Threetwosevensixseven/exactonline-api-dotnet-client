using ExactOnline.Client.Models;
using ExactOnline.Client.Sdk.Controllers;
using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApplication
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// These are the authorisation properties of your app.
			// You can find the values in the App Center when you are maintaining the app.
            const string clientId = "***REMOVED***";
            const string clientSecret = "***REMOVED***";

			// This can be any url as long as it is identical to the callback url you specified for your app in the App Center.
            var callbackUrl = new Uri("***REMOVED***");
            var connector = new ExactConnector(clientId, clientSecret, callbackUrl);
            
            Console.WriteLine("Use internal login form (1) or paste one-time code (2)?");
            if (Console.ReadKey(true).Key == ConsoleKey.D2)
            {
                bool auth = connector.RequestAuthorizationCode();
                Console.WriteLine("Paste the one-time code from your browser, followed by ENTER:");
                connector.AuthorizationCode = ReadLongLine();
            }
            var client = new ExactOnlineClient(connector.EndPoint, connector.GetAccessToken);

			// Get the Code and Name of a random account in the administration
			var fields = new[] { "Code", "Name" };
			var account = client.For<Account>().Top(1).Select(fields).Get().FirstOrDefault();

			Debug.WriteLine("Account {0} - {1}", account.Code.TrimStart(), account.Name);
		}

        private static string ReadLongLine()
        {
            System.IO.Stream inputStream = Console.OpenStandardInput(1024);
            byte[] bytes = new byte[1024];
            int outputLength = inputStream.Read(bytes, 0, 1024);
            char[] chars = System.Text.Encoding.UTF7.GetChars(bytes, 0, outputLength);
            return new string(chars).TrimEnd(new char[] { '\r', '\n' });
        }
	}
}
