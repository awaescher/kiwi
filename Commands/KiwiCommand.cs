using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace kiwi.Commands;

public abstract class KiwiCommand
{
	[Required]
	[Option(ShortName = "i",
		LongName = "ip",
		Description = "Required. The ip address of your kiwigrid api endpoint. Check if you can read a json from this URL: http://IPADDRESS/rest/kiwigrid/wizard/devices")]
	public string EndpointIpAddress { get; } = "";

	public async Task OnExecuteAsync()
	{
		var kiwi = await ReadKiwiAsymc().ConfigureAwait(false);
		await ExecuteAsync(kiwi).ConfigureAwait(false);
	}

	protected abstract Task ExecuteAsync(Kiwi kiwi);

	protected async Task<Kiwi> ReadKiwiAsymc()
	{
		var client = new HttpClient();
		var response = await client.GetStringAsync($"http://{EndpointIpAddress}/rest/kiwigrid/wizard/devices").ConfigureAwait(false);
		return new Kiwi(response);
	}
}
