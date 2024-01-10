using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUutilities;
using ConversionHelper;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos
{
    /// <summary>
    /// A long string of blocks connected by joints.
    /// </summary>
    public class BridgeDemo : StandardDemo
    {


        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public BridgeDemo(DemosGame game)
            : base(game)
        {
            //Form a long chain of planks connected by revolute joints.
            //The revolute joints control the three linear degrees of freedom and two angular degrees of freedom.
            //The third allowed angular degree of freedom allows the bridge to flex like a rope bridge.
            Vector3 startPosition = new Vector3(0, 0, 0);
            var startPlatform = new Box(startPosition - new Vector3(0, 0, (Fix64)3.2m), 8, (Fix64).5m, 8);
            Space.Add(startPlatform);
            Vector3 offset = new Vector3(0, 0, (Fix64)1.7m);
            Box previousLink = startPlatform;
            Vector3 position = new Vector3();
            for (int i = 1; i <= 200; i++)
            {
                position = startPosition + offset * i;
                Box link = new Box(position, (Fix64)4.5m, (Fix64).3m, (Fix64)1.5m, 50);
                Space.Add(link);
                Space.Add(new RevoluteJoint(previousLink, link, position - offset * (Fix64).5m, Vector3.Right));

                previousLink = link;
            }
            var endPlatform = new Box(position - new Vector3(0, 0, (Fix64)(-4.8m)), 8, (Fix64).5m, 8);
            Space.Add(endPlatform);

            Space.Add(new RevoluteJoint(previousLink, endPlatform, position + offset * (Fix64).5m, Vector3.Right));


            game.Camera.Position = startPosition + new Vector3(0, 1, offset.Z * 200 + 5);
        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Bridge"; }
        }
    }
}