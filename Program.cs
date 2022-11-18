using kiwi.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace kiwi;

/// <summary>
/// In this example, each command a nested class type.
/// </summary>
[Command(Name = "kiwi", Description = "Read values from your kiwigrid API gateway"),
 Subcommand(typeof(ReadCommand), typeof(DevicesCommand))]
partial class Program
{
	public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

	private int OnExecute(CommandLineApplication app, IConsole console)
	{
		console.WriteLine("You must specify at a subcommand.");
		app.ShowHelp();
		return 1;
	}
}