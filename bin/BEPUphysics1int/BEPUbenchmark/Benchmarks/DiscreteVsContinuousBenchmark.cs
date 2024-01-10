using BEPUphysics.Entities.Prefabs;
using BEPUphysics.PositionUpdating;
using BEPUutilities;

namespace BEPUbenchmark.Benchmarks
{
	public class DiscreteVsContinuousBenchmark : Benchmark
	{
		protected override void InitializeSpace()
		{
			//Create the discretely updated spheres.  These will fly through the ground due to their high speed.
			var toAdd = new Sphere(new Vector3(-6, 150, 0), .5m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			Space.Add(toAdd);

			toAdd = new Sphere(new Vector3(-4, 150, 0), .25m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			Space.Add(toAdd);

			toAdd = new Sphere(new Vector3(-2, 150, 0), .1m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			Space.Add(toAdd);

			Space.Add(new Box(new Vector3(0, 0, 0), 20m, .1m, 20));


			//Create the continuously updated spheres.  These will hit the ground and stop.
			toAdd = new Sphere(new Vector3(6, 150, 0), .5m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			toAdd.PositionUpdateMode = PositionUpdateMode.Continuous;
			Space.Add(toAdd);

			toAdd = new Sphere(new Vector3(4, 150, 0), .25m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			toAdd.PositionUpdateMode = PositionUpdateMode.Continuous;
			Space.Add(toAdd);

			toAdd = new Sphere(new Vector3(2, 150, 0), .1m, 1);
			toAdd.LinearVelocity = new Vector3(0, -100, 0);
			toAdd.PositionUpdateMode = PositionUpdateMode.Continuous;
			Space.Add(toAdd);
		}
	}
}
