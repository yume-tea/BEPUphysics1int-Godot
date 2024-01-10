using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Materials;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos
{
    /// <summary>
    /// Earthquake simulator with a jenga stack test subject.
    /// </summary>
    public class EarthquakeDemo : StandardDemo
    {
        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public EarthquakeDemo(DemosGame game)
            : base(game)
        {
            Entity ground = new Box(new Vector3(0, 0, 0), 50, 1, 50);
            Space.Add(ground);
            //Springs: Create a lattice of springs to hold the boxes together.
            var platform = new Box(new Vector3(0, 4, 0), 18, 1, 18, 400);
            platform.Material.KineticFriction = .8m;

            Space.Add(platform);

            //Create a big mess of DistanceJoints representing springs.

            var distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, -9), new Vector3(9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 3000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, -9), new Vector3(-9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 3000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, -9), new Vector3(9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 3000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, -9), new Vector3(-9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 3000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);

            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, 9), new Vector3(9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, 9), new Vector3(-9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, 9), new Vector3(9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, 9), new Vector3(-9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);

            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, -9), new Vector3(9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, -9), new Vector3(9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, 9), new Vector3(9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(9, .5m, 9), new Vector3(9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);

            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, -9), new Vector3(-9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, -9), new Vector3(-9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, 9), new Vector3(-9, 3.5m, -9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);
            distanceJoint = new DistanceJoint(ground, platform, new Vector3(-9, .5m, 9), new Vector3(-9, 3.5m, 9));
            distanceJoint.SpringSettings.Stiffness = 6000;
            distanceJoint.SpringSettings.Damping = 0;
            Space.Add(distanceJoint);

            int numBlocksTall = 10; //How many 'stories' tall.
            Fix64 blockWidth = 4; //Total width/length of the tower.
            Fix64 blockHeight = 1.333m;
            Entity toAdd;
            for (int i = 0; i < numBlocksTall; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        toAdd =
                            new Box(
                                new Vector3(
                                    j * blockWidth / 3 - blockWidth / 3,
                                    5 + blockHeight / 2 + i * blockHeight,
                                    0),
                                blockWidth / 3, blockHeight, blockWidth, 3 * numBlocksTall + 1 - 2 * i);
                        Space.Add(toAdd);
                        toAdd.Material = new Material(.8m, .8m, 0);
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        toAdd =
                            new Box(
                                new Vector3(0,
                                            5 + blockHeight / 2 + i * blockHeight,
                                            j * blockWidth / 3 - blockWidth / 3),
                                blockWidth, blockHeight, blockWidth / 3, 3 * numBlocksTall + 1 - 2 * i);
                        Space.Add(toAdd);
                        toAdd.Material = new Material(.8m, .8m, 0);
                    }
                }
            }

            game.Camera.Position = new Vector3(0, 7, 30);
        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Earthquake!"; }
        }
    }
}