using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using EnvDTE;
using System.Linq;

namespace LazyCat.VSTools.AttachToPlugin
{
	public class ProcessData
	{
		private const string IISProcess = "w3wp.exe";

		private string _name;
		private bool _attachToIIS;

		public ProcessData()
		{
			CodeTypes = new List<string>();
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;

				if (string.IsNullOrEmpty(_name))
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
					_name = IISProcess;
			}
		}

		public string AppPool { get; set; }

		[XmlElement("CodeType")]
		public List<string> CodeTypes { get; private set; }

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

			var userProcessExt = Path.GetExtension(_name);
			var userProcessDir = Path.GetDirectoryName(_name);
			var userProcessName = Path.GetFileNameWithoutExtension(_name);

			bool result = string.Compare(processName, userProcessName, StringComparison.OrdinalIgnoreCase) == 0;

			if (!string.IsNullOrEmpty(processDir) && !string.IsNullOrEmpty(userProcessDir))
				result &= string.Compare(processDir, userProcessDir, StringComparison.OrdinalIgnoreCase) == 0;

			if (!string.IsNullOrEmpty(processExt) && !string.IsNullOrEmpty(userProcessExt))
				result &= string.Compare(processExt, userProcessExt, StringComparison.OrdinalIgnoreCase) == 0;

			return result;
		}

		public string GetDisplayString()
		{
			var builder = new StringBuilder(Path.GetFileName(Name));

			if (AttachToIIS && !string.IsNullOrEmpty(AppPool))
				builder.AppendFormat("[{0}]", AppPool);

			if (CodeTypes.Count > 0)
				builder.AppendFormat("({0})", string.Join("/", CodeTypes));

			return builder.ToString();
		}

		public string GetShortString()
		{
			var builder = new StringBuilder(Path.GetFileNameWithoutExtension(Name));

			if (AttachToIIS && !string.IsNullOrEmpty(AppPool))
				builder.AppendFormat("_{0}", AppPool);

			if (CodeTypes.Count > 0)
				builder.AppendFormat("_{0}", string.Join("_", CodeTypes));

			var str = builder.ToString();

			return new string(str.Where(ch => char.IsLetterOrDigit(ch) || ch == '_').ToArray());
		}
	}
}