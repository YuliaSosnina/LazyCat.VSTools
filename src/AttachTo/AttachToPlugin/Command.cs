using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class Command
	{
		public Command(ProcessInfo info, DTE2 applicationObject)
		{
			ProcessInfo = info;

			_applicationObject = applicationObject;

			Name = info.GetShortString();
			DisplayText = string.Format("Attach to {0}", info.GetDisplayString()); 
			Description = string.Format("Attach to process {0}", info.GetDisplayString());
		}

		public ProcessInfo ProcessInfo { get; set; }

		public string Name { get; set; }
		public string DisplayText { get; set; }
		public string Description { get; set; }

		public void Run()
		{
			var processes = _applicationObject.Debugger.LocalProcesses.Cast<Process>().ToList();

			foreach (var process in GetProcessesToAttach(processes))
				process.Attach();
		}

		public virtual IList<Process> GetProcessesToAttach(IList<Process> processes)
		{
			return processes
				.Where(p => ProcessInfo.Is(p))
				.ToList();
		}

		private readonly DTE2 _applicationObject;
	}
}