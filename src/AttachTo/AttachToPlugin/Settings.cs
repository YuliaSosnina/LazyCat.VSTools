using System.Collections.Generic;
using System.Xml.Serialization;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class Settings
	{
		public Settings()
		{
			Processes = new List<ProcessData>();
		}

		public bool DisplayInToolsMenu { get; set; }

		[XmlElement("Process")]
		public List<ProcessData> Processes { get; private set; }
	}
}