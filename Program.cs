using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

public class Program
{
	// usage
	//   kiwiread.exe -i 192.168.178.83 -d MyReserve -t StateOfCharge
	//   kiwiread.exe --ip=192.168.178.83 --device=MyReserve --tagname=StateOfCharge

	// usage with dotnet run
	//   dotnet run -- -i 192.168.178.83 -d MyReserve -t StateOfCharge
	//   dotnet run -- --ip=192.168.178.83 --device=MyReserve --tagname=StateOfCharge --add-timestamp

	public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

	[Required]
	[Option(ShortName = "i", 
		LongName = "ip", 
		Description = "Required. The ip address of your kiwigrid api endpoint. Check if you can read a json from this URL: http://IPADDRESS/rest/kiwigrid/wizard/devices")]
	public string EndpointIpAddress { get; } = "";

	[Required]
	[Option(ShortName = "d", 
		LongName = "device", 
		Description = "Required. The full name of the device to read values from.")]
	public string Device { get; } = "";

	[Required]
	[Option(CommandOptionType.MultipleValue, 
		ShortName = "t", 
		LongName = "tagname", 
		Description = "Required. One or more tag names of the json classes to return the values from.")]
	public string[] TagNames { get; } = Array.Empty<string>();

	[Option(ShortName = "ts", 
		LongName = "add-timestamp",
		Description = "Optional. Adds an UTC time stamp to the result.")]
	public bool AddTimeStamp { get; } = false;

	private async Task OnExecute()
	{
		var client = new HttpClient();
		var response = await client.GetStringAsync($"http://{EndpointIpAddress}/rest/kiwigrid/wizard/devices").ConfigureAwait(false);

		var kiwi = new kiwipush.Kiwi(response);

		var device = kiwi.FindDevice(Device);

		string? result;

		var returnComplex = TagNames.Length != 1 || AddTimeStamp;
		if (returnComplex)
		{
			var resultValues = new Dictionary<string, object>();

			foreach (var tagName in TagNames)
				resultValues[tagName] = device.GetValue(tagName);

			if (AddTimeStamp)
				resultValues["UtcTimeStamp"] = DateTime.UtcNow;

			result = System.Text.Json.JsonSerializer.Serialize(resultValues);
		}
		else
		{ 
			result = device.GetValue(TagNames.Single());
		}

		Console.WriteLine(result);
	}
}