using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class Command
	{
		public Command(ProcessData data, DTE2 applicationObject, AddIn addInInstance)
		{
			ProcessData = data;

			_applicationObject = applicationObject;

			Name = data.GetShortString();
			FullName = string.Format("{0}.{1}", addInInstance.ProgID, Name);
			DisplayText = string.Format("Attach to {0}", data.GetDisplayString()); 
			Description = string.Format("Attach to process {0}", data.GetDisplayString());
		}

		public ProcessData ProcessData { get; set; }

		public string Name { get; set; }
		public string FullName { get; set; }
		public string DisplayText { get; set; }
		public string Description { get; set; }

		public void Run()
		{
			var processes = _applicationObject.Debugger.LocalProcesses.Cast<Process2>().ToList();

			foreach (var process in GetProcessesToAttach(processes))
			{
				var engines = GetEngines(process);

				if (engines != null)
					process.Attach2(engines.ToArray());
				else
					process.Attach();
			}
		}

		public virtual IList<Process2> GetProcessesToAttach(IList<Process2> processes)
		{
			return processes
				.Where(p => ProcessData.Is(p))
				.ToList();
		}

		public virtual IList<Engine> GetEngines(Process2 process)
		{
			if (ProcessData.CodeTypes.Count == 0)
				return null;

			var engines = ProcessData.CodeTypes
				.Select(type => process.Transport.Engines.Item(type))
				.Where(engine => engine != null)
				.ToList();

			if (engines.Count > 0)
				return engines;

			return null;
		}

		private readonly DTE2 _applicationObject;
	}
}