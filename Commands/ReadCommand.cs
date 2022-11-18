using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace kiwi.Commands;

[Command("read", Description = "Reads values for specified kiwigrid devices. Use the 'devices' command to find devices that are available to you.")]
public class ReadCommand : KiwiCommand
{
	// usage
	//   kiwi.exe read -i 192.168.178.83 -d MyReserve -t StateOfCharge
	//   kiwi.exe read --ip=192.168.178.83 --device=MyReserve --tag=StateOfCharge

	// usage with dotnet run
	//   dotnet run -- read -i 192.168.178.83 -d MyReserve -t StateOfCharge
	//   dotnet run -- read --ip=192.168.178.83 --device=MyReserve --tag=StateOfCharge --add-timestamp

	[Required]
	[Option(ShortName = "d",
		LongName = "device",
		Description = "Required. The full name of the device to read values from.")]
	public string Device { get; } = "";

	[Required]
	[Option(CommandOptionType.MultipleValue,
		ShortName = "t",
		LongName = "tag",
		Description = "Required. One or more tag names of the json classes to return the values from.")]
	public string[] Tags { get; } = Array.Empty<string>();

	[Option(ShortName = "ts",
		LongName = "add-timestamp",
		Description = "Optional. Adds an UTC time stamp to the result.")]
	public bool AddTimeStamp { get; } = false;

	protected override async Task ExecuteAsync(Kiwi kiwi)
	{
		var device = await kiwi.FindDevice(Device);

		string? result;

		var returnComplex = Tags.Length != 1 || AddTimeStamp;
		if (returnComplex)
		{
			var resultValues = new Dictionary<string, object>();

			foreach (var tag in Tags)
				resultValues[tag] = device.GetValue(tag);

			if (AddTimeStamp)
				resultValues["UtcTimeStamp"] = DateTime.UtcNow;

			result = System.Text.Json.JsonSerializer.Serialize(resultValues);
		}
		else
		{
			result = device.GetValue(Tags.Single());
		}

		Console.WriteLine(result);
	}
}
