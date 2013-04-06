using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.Web.Administration;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class IISCommand : Command
	{
		public IISCommand(ProcessInfo info, DTE2 applicationObject) : base(info, applicationObject)
		{
		}

		public override IList<Process> GetProcessesToAttach(IList<Process> processes)
		{
			if (!string.IsNullOrEmpty(ProcessInfo.AppPool))
			{
				var processIds = GetAppPoolProcessIds(ProcessInfo.AppPool);

				if (processIds != null)
					return processes.Where(p => processIds.Contains(p.ProcessID)).ToList();
			}

			return base.GetProcessesToAttach(processes);
		}

		private IList<int> GetAppPoolProcessIds(string poolName)
		{
			var manager = new ServerManager();

			foreach (var pool in manager.ApplicationPools)
			{
				if (string.Compare(pool.Name, poolName, StringComparison.OrdinalIgnoreCase) == 0)
					return pool.WorkerProcesses.Select(process => process.ProcessId).ToList();
			}

			return null;
		}
	}
}