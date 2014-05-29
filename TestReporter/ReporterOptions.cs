using System;

namespace TestReporter
{
	public enum CompilerType 
	{
		MCS,
		CSC
	}

	public enum RuntimeType 
	{
		MONO,
		MS
	}


	public class ReporterOptions
	{
		private CompilerType _compiler = CompilerType.CSC;
		private RuntimeType _runtime = RuntimeType.MS;
		private int _nrOfRuns = 10;

		public CompilerType Compiler {
			get {
				return _compiler;
			}
			set {
				_compiler = value;
			}
		}

		public RuntimeType Runtime {
			get {
				return _runtime;
			}
			set {
				_runtime = value;
			}
		}

		public int NumberOfRuns {
			get {
				return _nrOfRuns;
			}
			set {
				_nrOfRuns = value;
			}
		}

		public string Target { get; set; }
	} 
}

