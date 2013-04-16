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
		public IISCommand(ProcessData data, DTE2 applicationObject, AddIn addInInstance)
			: base(data, applicationObject, addInInstance)
		{
		}

		public override IList<Process2> GetProcessesToAttach(IList<Process2> processes)
		{
			if (!string.IsNullOrEmpty(ProcessData.AppPool))
			{
				var processIds = GetAppPoolProcessIds(ProcessData.AppPool);

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