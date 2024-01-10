using System;

namespace BEPUbenchmark
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Running benchmarks...\n");

			double runtime = 0;
			foreach (Benchmark b in AllBenchmarks.Benchmarks)
			{
				b.Initialize();
				Console.Write(b.GetName()+"... ");
				double time = b.RunBenchmark();
				b.Dispose();

				Console.WriteLine(Math.Round(time, 2) + "s");
				runtime += time;
			}

			Console.WriteLine("\nCumulative runtime: "+Math.Round(runtime, 2));
		}
	}
}
