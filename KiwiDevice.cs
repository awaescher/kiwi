using JsonPathway;
using kiwipush;
using kiwipush.Exceptions;

public class KiwiDevice
{
	public Kiwi Kiwi { get; }
	public string Name { get; }
	public string Guid { get; }

	public KiwiDevice(Kiwi kiwi, string name, string guid)
	{
		Kiwi = kiwi;
		Name = name;
		Guid = guid;
	}

	public string GetValue(string tag)
	{
		const string VALUE_PROPERTY = "value";

		try
		{
			var valueSelector = $"result.items[*].tagValues[?(@.tagName=='{tag}' && @.guid == '{Guid}')]";

			var element = JsonPath.ExecutePath(valueSelector, Kiwi.JsonContent);
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

