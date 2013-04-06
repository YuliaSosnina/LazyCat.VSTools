using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class FileSettingsProvider : ISettingsProvider
	{
		public FileSettingsProvider(string fileName)
		{
			FileName = fileName;
		}

		public string FileName { get; set; }

		public Settings Get()
		{
			Settings settings;

			
			var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			using (var stream = new FileStream(Path.Combine(dir, FileName), FileMode.Open))
				settings = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(stream);


			return settings;
		}
	}
}