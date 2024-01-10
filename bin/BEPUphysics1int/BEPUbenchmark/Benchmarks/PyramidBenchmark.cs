using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using FixMath.NET;

namespace BEPUbenchmark.Benchmarks
{
	public class PyramidBenchmark : Benchmark
	{
		protected override void InitializeSpace()
		{
			Fix64 boxSize = 2;
			int boxCount = 20;
			Fix64 platformLength = MathHelper.Min(50, boxCount * boxSize + 10);
			Space.Add(new Box(new Vector3(0, -.5m, 0), boxCount * boxSize + 20, 1,
							  platformLength));

			for (int i = 0; i < boxCount; i++)
			{
				for (int j = 0; j < boxCount - i; j++)
				{
					Space.Add(new Box(
								  new Vector3(
									  -boxCount * boxSize / 2 + boxSize / 2 * i + j * (boxSize),
									  (boxSize / 2) + i * boxSize,
									  0),
								  boxSize, boxSize, boxSize, 20));
				}
			}
			//Down here are the 'destructors' used to blow up the pyramid.

			Sphere pow = new Sphere(new Vector3(-25, 5, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
			pow = new Sphere(new Vector3(-15, 10, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
			pow = new Sphere(new Vector3(-5, 15, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
			pow = new Sphere(new Vector3(5, 15, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
			pow = new Sphere(new Vector3(15, 10, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
			pow = new Sphere(new Vector3(25, 5, 70), 2, 40);
			pow.LinearVelocity = new Vector3(0, 10, -100);
			Space.Add(pow);
		}
	}
}
