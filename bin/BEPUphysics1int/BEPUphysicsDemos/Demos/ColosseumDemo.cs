using System;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos
{
	/// <summary>
	/// Ring-shaped structure made of blocks.
	/// </summary>
	public class ColosseumDemo : StandardDemo
	{
		/// <summary>
		/// Constructs a new demo.
		/// </summary>
		/// <param name="game">Game owning this demo.</param>
		public ColosseumDemo(DemosGame game)
			: base(game)
		{
			Fix64 angle;
			int numBoxesPerRing = 12;
			Fix64 blockWidth = 2;
			Fix64 blockHeight = 2;
			Fix64 blockLength = 6;
			Fix64 radius = 15;
			Entity toAdd;
			Space.Add(new Box(new Vector3(0, -blockHeight / 2 - 1, 0), 100, 2, 100));
			Fix64 increment = MathHelper.TwoPi / numBoxesPerRing;
			for (int i = 0; i < 8; i++)
			{
				for (int k = 0; k < numBoxesPerRing; k++)
				{
					if (i % 2 == 0)
					{
						angle = k * increment;
						toAdd = new Box(new Vector3(-Fix64.Cos(angle) * radius, i * blockHeight, Fix64.Sin(angle) * radius), blockWidth, blockHeight, blockLength, 20);
						toAdd.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, (Fix64) angle);
						Space.Add(toAdd);
					}
					else
					{
						angle = (k + .5m) * increment;
						toAdd = new Box(new Vector3(-Fix64.Cos(angle) * radius, i * blockHeight, Fix64.Sin(angle) * radius), blockWidth, blockHeight, blockLength, 20);
						toAdd.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Up, (Fix64) angle);
						Space.Add(toAdd);
					}
				}
			}
			game.Camera.Position = new Vector3(0, 2, 2);
		}

		/// <summary>
		/// Gets the name of the simulation.
		/// </summary>
		public override string Name
		{
			get { return "Colosseum"; }
		}
	}
}
