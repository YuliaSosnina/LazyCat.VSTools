namespace LazyCat.VSTools.AttachToPlugin
{
	public class SimpleSettingsProvider : ISettingsProvider
	{
		public Settings Get()
		{
			var settings = new Settings();

			settings.Processes.Add(new ProcessInfo
				{
					AttachToIIS = true,
					AppPool = "test"
				});

			settings.Processes.Add(new ProcessInfo
				{
					ProcessName = "aaa"
				});

			settings.Processes.Add(new ProcessInfo
				{
					ProcessName = @"d:\tmp\test.exe"
				});

			return settings;
		}
	}
}