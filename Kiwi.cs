using JsonPathway;
using kiwiread.Exceptions;

namespace kiwiread;

public class Kiwi
{
	public string JsonContent { get; }

	public Kiwi(string jsonContent)
	{
		JsonContent = jsonContent ?? throw new ArgumentNullException(nameof(jsonContent));
	}

	public Task<IEnumerable<KiwiDevice>> FindDevices()
	{
		const string GUID_PROPERTY = "guid";
		const string VALUE_PROPERTY = "value";
		
		try
		{
			var nameSelector = $"result.items[*].tagValues[?(@.tagName=='IdName')]";

			var elements = JsonPath.ExecutePath(nameSelector, JsonContent);

			var devices = elements.Select(e => new KiwiDevice(this, e.GetProperty(VALUE_PROPERTY).ToString(), e.GetProperty(GUID_PROPERTY).ToString()));

			return Task.FromResult(devices);
		}
		catch (KeyNotFoundException ex)
		{
			throw new PropertyNotFoundException(GUID_PROPERTY, ex);
		}
	}

	public Task<KiwiDevice> FindDevice(string deviceName)
	{
		const string GUID_PROPERTY = "guid";

		try
		{
			var nameSelector = $"result.items[*].tagValues[?(@.tagName=='IdName' && @.value=='{deviceName}')]";

			var elements = JsonPath.ExecutePath(nameSelector, JsonContent);
			var guid = elements.First().GetProperty(GUID_PROPERTY).ToString();

			var device = new KiwiDevice(this, deviceName, guid);
			return Task.FromResult(device);
		}
		catch (InvalidOperationException ex)
		{
			throw new DeviceNotFoundException(deviceName, ex);
		}
		catch (KeyNotFoundException ex)
		{
			throw new PropertyNotFoundException(GUID_PROPERTY, ex);
		}
	}
}