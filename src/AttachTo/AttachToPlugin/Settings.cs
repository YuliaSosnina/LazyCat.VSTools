using System.Collections.Generic;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class Settings
	{
		private readonly List<ProcessInfo> _processes = new List<ProcessInfo>();

		public List<ProcessInfo> Processes
		{
			get { return _processes; }
		}

		public bool ToolButtons { get; set; }
	}
}