using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace TestReporter
{
	public class TestReporter
	{
		private ReporterOptions _options;
		private string resultFile = "result";
		private string compileFlags = "/debug- /out:";
		private string outputFile;
		private bool compileSuccessfull = false;
		private bool runSuccessfull = false;
		private Dictionary<String, List<double>> _result = new Dictionary<string, List<double>>();
		private const int SLEEP_TIME = 100;

		public TestReporter (): this(new ReporterOptions())
		{
		}

		public TestReporter(ReporterOptions Options)
		{
			this._options = Options;
		}

		public void Start()
		{
			Task compile = Task.Factory.StartNew (() => this.Compile ());
			compile.Wait ();
			Task run = Task.Factory.StartNew (() => this.Run ());
			run.Wait ();
			WriteFile ();
		}

		private void WriteFile()
		{
			string filename = _options.Compiler + " " + _options.Runtime + ".txt";

			using(StreamWriter writer = new StreamWriter(filename, false))
			{
				foreach(KeyValuePair<string, List<double>> pair in _result)
				{
					string output = pair.Key;

					pair.Value.ForEach (x => output += " ," + x);

					writer.WriteLine (output);
				}
			}
		}

		private string buildCompileString()
		{
			string compiler;

			if(_options.Compiler == CompilerType.CSC)
			{
				compiler = "csc";
			}
			else
			{
				compiler = "mcs";
			}

			this.outputFile = _options.Target.Substring (0, _options.Target.IndexOf ('.')) + ".exe";

			return "/C " + compiler + " " + compileFlags + this.outputFile +" " + _options.Target;
		}

		private string buildRunString(int nr = 0)
		{
			string output = "/C";
			if(_options.Runtime == RuntimeType.MONO)
			{
				output += " mono";
			}
			return output + " " + this.outputFile + " > " + this.resultFile + "-" + nr;
		}

		private void Compile()
		{
			Process proc = new Process ();

			proc.StartInfo.FileName = "CMD.exe";
			proc.StartInfo.Arguments = buildCompileString();
			proc.EnableRaisingEvents = true;

			proc.Exited += new EventHandler(onCompileExit);

			proc.Start();

			// wait 5 seconds maximum for compilation
			int elapsedTime = 0;
			while(!this.compileSuccessfull)
			{
				if(elapsedTime > 5000)
				{
					break;
				}
				Thread.Sleep (SLEEP_TIME);
				elapsedTime += elapsedTime;
			}
		}

		private void onCompileExit(object sender, System.EventArgs e)
		{
			Process proc = (Process)sender;
			if(proc.ExitCode == 0)
			{
				this.compileSuccessfull = true;	
			}
		}

		private void onRunExit(object sender, System.EventArgs e)
		{
			this.runSuccessfull = true;
		}

    	private void Run()
		{
			for(int i = 1; i <= _options.NumberOfRuns; i++)
			{
				Process proc = new Process ();

				proc.StartInfo.FileName = "CMD.exe";
				proc.StartInfo.Arguments = buildRunString(i);
				proc.EnableRaisingEvents = true;
				proc.Exited += new EventHandler (onRunExit);
				proc.Start();

				int elapsedTime = 0;
				while(!this.runSuccessfull)
				{
					if(elapsedTime > 30000)
					{
						break;
					}
					Thread.Sleep (SLEEP_TIME);
					elapsedTime += elapsedTime;
				}

				if(this.runSuccessfull)
				{
					this.runSuccessfull = false;
					this.Process (resultFile + "-" + i);
				}
			}
		}

		private void Process(string file)
		{
			using(StreamReader reader = new StreamReader(file))
			{
				while(reader.Peek() >= 0)
				{
					string[] line = reader.ReadLine ().Split(':');

					if(line.Length == 2 )
					{
						if(_result.ContainsKey(line[0]))
						{
							_result [line [0]].Add (Convert.ToDouble (line [1]));
						}
						else
						{
							_result.Add (line [0], new List<double> () { Convert.ToDouble (line [1]) });
						}
					}
				}
			}
		}
	}
}

