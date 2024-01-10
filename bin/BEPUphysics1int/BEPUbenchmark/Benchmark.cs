using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUutilities.Threading;
using FixMath.NET;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BEPUbenchmark
{
	public abstract class Benchmark
	{
		protected Space Space;
		
		protected abstract void InitializeSpace();
		protected virtual void Step()
		{
			Space.Update();
		}

		public void Initialize()
		{
			Space = new Space();
			Space.ForceUpdater.Gravity = new Vector3(0, (Fix64)(-9.81m), 0);
			Space.TimeStepSettings.TimeStepDuration = 1 / 60m;

			InitializeSpace();
		}

		public void Dispose()
		{
			Space = null;
		}

		public string RunToNextHash()
		{
			for (int i = 0; i < 20; i++)
				Step();

			return HashSpace();
		}

		public double RunBenchmark()
		{
			Console.WriteLine("");
			long startTime = DateTime.Now.Ticks;
			long opStartTime = DateTime.Now.Ticks;
			int opCount = 0;
			for (int i = 0; i < 1000; i++)
			{
				Step();
				opCount++;
				long time = DateTime.Now.Ticks - opStartTime;
				if (time > TimeSpan.TicksPerSecond)
				{
					Console.Write(string.Format("\rAvg. duration per Step: {0}ms                ", (time/TimeSpan.TicksPerMillisecond)/opCount));
					opCount = 0;
					opStartTime = DateTime.Now.Ticks;
				}					
			}

			long runTime = (DateTime.Now.Ticks - startTime);

			Console.Write("\r                                                                          \r");
			return (double)runTime / TimeSpan.TicksPerSecond;
		}

		public string GetName()
		{
			return this.GetType().Name;
		}

		private string HashSpace()
		{
			const int valuesPerEntity = 3 + 4 + 3;
			byte[] state = new byte[Space.Entities.Count * valuesPerEntity * sizeof(long)];
			int offset = 0;
			foreach (Entity e in Space.Entities)
			{
				Fix64IntoByteArray(e.Position.X, offset++, state);
				Fix64IntoByteArray(e.Position.Y, offset++, state);
				Fix64IntoByteArray(e.Position.Z, offset++, state);

				Fix64IntoByteArray(e.Orientation.X, offset++, state);
				Fix64IntoByteArray(e.Orientation.Y, offset++, state);
				Fix64IntoByteArray(e.Orientation.Z, offset++, state);
				Fix64IntoByteArray(e.Orientation.W, offset++, state);

				Fix64IntoByteArray(e.LinearVelocity.X, offset++, state);
				Fix64IntoByteArray(e.LinearVelocity.Y, offset++, state);
				Fix64IntoByteArray(e.LinearVelocity.Z, offset++, state);
			}
			HashAlgorithm md5 = MD5.Create();
			byte[] hash = md5.ComputeHash(state);

			StringBuilder hex = new StringBuilder(hash.Length * 2);
			foreach (byte b in hash)
				hex.AppendFormat("{0:x2}", b);

			return hex.ToString();
		}

		private void Fix64IntoByteArray(Fix64 value, int offset, byte[] destination)
		{
			offset *= sizeof(long);
			long raw = value.RawValue;
			destination[offset++] = (byte)(raw & 0xFF);
			destination[offset++] = (byte)((raw >> 8) & 0xFF);
			destination[offset++] = (byte)((raw >> 16) & 0xFF);
			destination[offset++] = (byte)((raw >> 24) & 0xFF);
		}
	}
}
