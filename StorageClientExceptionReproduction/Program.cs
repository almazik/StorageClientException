using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureQueryReproduce
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(@"Run Fiddler now, make sure it is capturing requests,
and set automatic breakpoints in it: 
[Rules]->[Automatic Breakpoints]->[After Responses] or Alt+F11 in Fiddler window

Then press Enter...");
			Console.ReadLine();


			//using storage emulator credentials as we need any valid name/key pair to force outgoing request to Azure Storage servers.
			var cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");

			var context = new TableServiceContext(cloudStorageAccount.TableEndpoint.AbsoluteUri, cloudStorageAccount.Credentials)
			{
				RetryPolicy = RetryPolicies.RetryExponential(10, TimeSpan.FromSeconds(1)),
			};

			var dataServiceQuery = context.CreateQuery<TableServiceEntity>("TableServiceEntity");

			//var cloudTableQuery = dataServiceQuery.AsTableServiceQuery(); //note that this code doesn't transfer RetryPolicy to CloudTableQuery, so using explicit declaration:
			var cloudTableQuery = new CloudTableQuery<TableServiceEntity>(dataServiceQuery, context.RetryPolicy);


			Console.WriteLine(@"Paused request to devstoreaccount1.table.core.windows.net should appear in Fiddler now.
Please wait 2 minutes to get exception described");


			foreach (var tableServiceEntity in cloudTableQuery)
			{
				//Notice that Fiddler registered only one attempt to get the resource, so RetryPolicy is not used here
			}
		}
	}
}
