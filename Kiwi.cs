using JsonPathway;
using kiwipush.Exceptions;

namespace kiwipush;

public class Kiwi
{
	public string JsonContent { get; }

	public Kiwi(string jsonContent)
	{
		JsonContent = jsonContent ?? throw new ArgumentNullException(nameof(jsonContent));
	}

	public KiwiDevice FindDevice(string deviceName)
	{
		const string GUID_PROPERTY = "guid";

		try
		{
			var nameSelector = $"result.items[*].tagValues[?(@.tagName=='IdName' && @.value=='{deviceName}')]";

			var element = JsonPath.ExecutePath(nameSelector, JsonContent);
			var guid = element.First().GetProperty(GUID_PROPERTY).ToString();

			return new KiwiDevice(this, deviceName, guid);
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