using System;
using System.IO;
using EnvDTE;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class ProcessInfo
	{
		private const string IISProcess = "w3wp.exe";

		private string _processName;
		private bool _attachToIIS;

		public string ProcessName
		{
			get { return _processName; }
			set
			{
				_processName = value;

				if (string.IsNullOrEmpty(_processName))
				{
					_attachToIIS = false;

					return;
				}

				if (Is(IISProcess))
					_attachToIIS = true;
			}
		}

		public bool AttachToIIS
		{
			get { return _attachToIIS; }
			set
			{
				_attachToIIS = value;

				if (_attachToIIS)
					_processName = IISProcess;
			}
		}

		public string AppPool { get; set; }

		public bool Is(Process process)
		{
			return Is(process.Name);
		}

		public bool Is(string process)
		{
			if (string.IsNullOrEmpty(process))
				return false;

			var processExt = Path.GetExtension(process);
			var processDir = Path.GetDirectoryName(process);
			var processName = Path.GetFileNameWithoutExtension(process);

			var userProcessExt = Path.GetExtension(_processName);
			var userProcessDir = Path.GetDirectoryName(_processName);
			var userProcessName = Path.GetFileNameWithoutExtension(_processName);

			bool result = string.Compare(processName, userProcessName, StringComparison.OrdinalIgnoreCase) == 0;

			if (!string.IsNullOrEmpty(processDir) && !string.IsNullOrEmpty(userProcessDir))
				result &= string.Compare(processDir, userProcessDir, StringComparison.OrdinalIgnoreCase) == 0;

			if (!string.IsNullOrEmpty(processExt) && !string.IsNullOrEmpty(userProcessExt))
				result &= string.Compare(processExt, userProcessExt, StringComparison.OrdinalIgnoreCase) == 0;

			return result;
		}

		public string GetDisplayString()
		{
			string name = Path.GetFileName(ProcessName);

			if (!AttachToIIS || string.IsNullOrEmpty(AppPool))
				return name;

			return string.Format("{0}[{1}]", name, AppPool);
		}

		public string GetShortString()
		{
			var name = Path.GetFileNameWithoutExtension(ProcessName);

			if (!AttachToIIS || string.IsNullOrEmpty(AppPool))
				return name;

			return string.Format("{0}_{1}", name, AppPool);
		}
	}
}