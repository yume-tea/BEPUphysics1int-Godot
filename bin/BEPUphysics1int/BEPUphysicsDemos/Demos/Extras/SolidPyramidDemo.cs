
using System;
using BEPUphysics.Constraints;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Materials;
using BEPUphysics.Settings;
using BEPUutilities;
using FixMath.NET;
using Microsoft.Xna.Framework.Input;

namespace BEPUphysicsDemos.Demos.Extras
{
    /// <summary>
    /// Bunch of blocks arranged in a 3d pyramid, waiting to be blown up.
    /// </summary>
    public class SolidPyramidDemo : StandardDemo
    {
        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public SolidPyramidDemo(DemosGame game)
            : base(game)
        {
            Fix64 boxSize = 1;
            int bottomBoxCount = 10;

            var ground = new Box(new Vector3(0, -.5m, 0), 40, 1, 40);
            Space.Add(ground);

            Fix64 spacing = 0.05m;

            Fix64 offset = -0.5m * ((bottomBoxCount - 1) * (boxSize + spacing));
            var origin = new Vector3(offset, -boxSize * 0.5m, offset);
            for (int heightIndex = 0; heightIndex < bottomBoxCount - 2; ++heightIndex)
            {
                var levelWidth = bottomBoxCount - heightIndex;
                Fix64 perBoxWidth = boxSize + spacing;
                //Move the origin for this level.
                origin.X += perBoxWidth * 0.5m;
                origin.Y += boxSize;
                origin.Z += perBoxWidth * 0.5m;

                for (int i = 0; i < levelWidth; ++i)
                {
                    for (int j = 0; j < levelWidth; ++j)
                    {
                        var position = new Vector3(
                            origin.X + i * perBoxWidth,
                            origin.Y,
                            origin.Z + j * perBoxWidth);

                        var box = new Box(position, boxSize, boxSize, boxSize, 20);

                        Space.Add(box);
                    }
                }
            }

            game.Camera.Position = new Vector3(-bottomBoxCount * boxSize, 2, bottomBoxCount * boxSize);
            game.Camera.Yaw(MathHelper.Pi / -4);
            game.Camera.Pitch(MathHelper.Pi / 9);
        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Solid Pyramid"; }
        }
    }
}