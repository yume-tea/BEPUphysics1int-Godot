using BEPUbenchmark;
using BEPUbenchmark.Benchmarks;
using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BEPUtests
{
	public class CrossPlatformDeterminismTests
    {
		private readonly ITestOutputHelper output;

		public CrossPlatformDeterminismTests(ITestOutputHelper output)
		{
			this.output = output;
		}

		private void GetExpectedHash(Benchmark b, StringBuilder result)
		{
			result.AppendFormat("{{\"{0}\", new string[] {{", b.GetName());

			b.Initialize();
			long startTime = DateTime.Now.Ticks;
			for (int i = 0; i < 50; i++)
			{
				string expectedHash = b.RunToNextHash();
				if (i != 0)
					result.Append(",\n");
				result.AppendFormat("\"{0}\"", expectedHash);

				if (i > 5 && DateTime.Now.Ticks - startTime > TimeSpan.TicksPerSecond * 5)
					break;
			}
			result.AppendLine("}}");
		}

		//[Fact]
		public void OutputExpectedHashes()
		{
			StringBuilder result = new StringBuilder();

			result.AppendLine(@"
using System.Collections.Generic;
namespace BEPUtests
{
class CrossPlatformDeterminismExpectedHashes
{
public static readonly Dictionary<string, string[]> Hashes = new Dictionary<string, string[]>()
{");

			bool first = true;
			foreach (Benchmark b in AllBenchmarks.Benchmarks)
			{
				if (!first)
					result.Append(",\n");
				GetExpectedHash(b, result);
				first = false;
			}
			result.AppendLine("};\n}\n}\n");
			output.WriteLine(result.ToString());
		}
		
		//[Fact]
		public void OutputExpectedHashesForBenchmark()
		{
			Benchmark b = new PathFollowingBenchmark();

			StringBuilder result = new StringBuilder();

			GetExpectedHash(b, result);
			output.WriteLine(result.ToString());
		}

		[Fact]
		public void DiscreteVsContinuous()
		{
			TestDeterminism(new DiscreteVsContinuousBenchmark());
		}

		[Fact]
		public void InverseKinematic()
		{
			TestDeterminism(new InverseKinematicsBenchmark());
		}

		[Fact]
		public void Pyramid()
		{
			TestDeterminism(new PyramidBenchmark());
		}

		[Fact]
		public void PathFollowing()
		{
			TestDeterminism(new PathFollowingBenchmark());
		}

		[Fact]
		public void SelfCollidingCloth()
		{
			TestDeterminism(new SelfCollidingClothBenchmark());
		}

		private void TestDeterminism(Benchmark b)
		{
			b.Initialize();
			string[] expectedHashes = CrossPlatformDeterminismExpectedHashes.Hashes[b.GetName()];
			int step = 0;
			foreach (string expectedHash in expectedHashes)
			{
				string actualHash = b.RunToNextHash();
				Assert.True(expectedHash == actualHash, string.Format("Expected {0}, actual {1} in step {2}", expectedHash, actualHash, step));
				step++;
			}
		}
	}
}
