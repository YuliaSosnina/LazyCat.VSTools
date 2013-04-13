namespace LazyCat.VSTools.AttachToPlugin
{
	public class SimpleSettingsProvider : ISettingsProvider
	{
		public Settings Get()
		{
			var settings = new Settings();

			settings.Processes.Add(new ProcessData
				{
					AttachToIIS = true,
					AppPool = "test"
				});

			settings.Processes.Add(new ProcessData
				{
					Name = "aaa"
				});

			settings.Processes.Add(new ProcessData
				{
					Name = @"d:\tmp\test.exe"
				});

			return settings;
		}
	}
}