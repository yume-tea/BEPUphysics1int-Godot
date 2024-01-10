using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.PositionUpdating;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos
{
    /// <summary>
    /// A set of jenga blocks.
    /// </summary>
    public class JengaDemo : StandardDemo
    {
        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public JengaDemo(DemosGame game)
            : base(game)
        {

            Space.Remove(kapow);
            //Have to shrink the ball a little to make it fit between jenga tower blocks.
            kapow = new Sphere(new Vector3(-11000, 0, 0), .2m, 20);
            kapow.PositionUpdateMode = PositionUpdateMode.Continuous; //The ball's really tiny! It will work better if it's handled continuously.
            Space.Add(kapow);
            int numBlocksTall = 18; //How many 'stories' tall.
            Fix64 blockWidth = 3; //Total width/length of the tower.
            Fix64 blockHeight = 1 / (Fix64)2;
            Entity toAdd;


            for (int i = 0; i < numBlocksTall; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        toAdd = new Box(new Vector3(
                                            j * (blockWidth / 3) - blockWidth / 3,
                                            blockHeight / 2 + i * (blockHeight),
                                            0),
                                        blockWidth / 3, blockHeight, blockWidth, 10);
                        Space.Add(toAdd);
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        toAdd = new Box(new Vector3(
                                            0,
                                            blockHeight / 2 + (i) * (blockHeight),
                                            j * (blockWidth / 3) - blockWidth / 3),
                                        blockWidth, blockHeight, blockWidth / 3, 10);
                        Space.Add(toAdd);

                    }
                }
            }
            Space.Add(new Box(new Vector3(0, -.5m, 0), 40, 1, 40));
            game.Camera.Position = new Vector3(0, 5, 15);

        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Jenga"; }
        }
    }
}