using McMaster.Extensions.CommandLineUtils;

namespace kiwiread.Commands;

[Command("devices",
	 Description = "Lists registered kiwigrid devices")]
public class DevicesCommand : KiwiCommand
{
	[Option(ShortName = "j",
	LongName = "json",
	Description = "Optional. Outputs the device information as json array with detailed information.")]
	public bool FormatAsJson { get; } = false;

	protected override async Task ExecuteAsync(Kiwi kiwi)
	{
		var devices = await kiwi.FindDevices();

		string? result;

		if (FormatAsJson)
			result = System.Text.Json.JsonSerializer.Serialize(devices);
		else
			result = string.Join(Environment.NewLine, devices.Select(d => d.Name));

		Console.WriteLine(result);
	}
}
