using JsonPathway;
using kiwi.Exceptions;

namespace kiwi;

public class KiwiDevice
{
	private Kiwi _kiwi;
	public string Name { get; }
	public string Guid { get; }

	public KiwiDevice(Kiwi kiwi, string name, string guid)
	{
		_kiwi = kiwi ?? throw new ArgumentNullException(nameof(kiwi));

		Name = name ?? throw new ArgumentNullException(nameof(name));
		Guid = guid ?? throw new ArgumentNullException(nameof(guid));
	}

	public string GetValue(string tag)
	{
		const string VALUE_PROPERTY = "value";

		try
		{
			var valueSelector = $"result.items[*].tagValues[?(@.tagName=='{tag}' && @.guid == '{Guid}')]";

			var element = JsonPath.ExecutePath(valueSelector, _kiwi.JsonContent);
			return element.First().GetProperty(VALUE_PROPERTY).ToString();
		}
		catch (InvalidOperationException ex)
		{
			throw new TagNotFoundException(tag, ex);
		}
		catch (KeyNotFoundException ex)
		{
			throw new PropertyNotFoundException(VALUE_PROPERTY, ex);
		}
	}

	public override string ToString() => $"{Name} ({Guid})";
}

