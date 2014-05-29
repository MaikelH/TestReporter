using System;

namespace TestReporter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ReporterOptions options = parseArguments (args);

			TestReporter reporter = new TestReporter (options);

			reporter.Start ();
		}	

		private static ReporterOptions parseArguments(string[] arguments)
		{
			ReporterOptions options = new ReporterOptions ();

			foreach (string argument in arguments) 
			{
				string[] split = argument.Split (':');
				switch (split [0]) {
					case "-c":
						if (split [1].ToLower ().Equals ("csc")) {
							options.Compiler = CompilerType.CSC;
						} else {
							options.Compiler = CompilerType.MCS;
						}
						break;
					case "-n":
						options.NumberOfRuns = Convert.ToInt32 (split [1]);
						break;
					case "-r":
						if(split [1].ToLower ().Equals ("mono"))
						{
							options.Runtime = RuntimeType.MONO;
						}
						break;
					case "-t":
						options.Target = split [1];
						break;
				}
			}

			return options;
		}
	}
}
