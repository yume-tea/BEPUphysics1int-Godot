
using System;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysicsDemos.SampleCode;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using System.Collections.Generic;
using BEPUutilities;

namespace BEPUphysicsDemos.Demos
{
    /// <summary>
    /// A spaceship blasts off into the sky (void).
    /// </summary>
    public class SpaceshipDemo : StandardDemo
    {
        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public SpaceshipDemo(DemosGame game)
            : base(game)
        {
            //Build the ship
            var shipFuselage = new CompoundShapeEntry(new CylinderShape(3, .7m), new Vector3(0, 5, 0), 4);
            var shipNose = new CompoundShapeEntry(new ConeShape(2, .7m), new Vector3(0, 7, 0), 2);
            var shipWing = new CompoundShapeEntry(new BoxShape(5, 2, .2m), new Vector3(0, 5, 0), 3);
            var shipThrusters = new CompoundShapeEntry(new ConeShape(1, .5m), new Vector3(0, 3.25m, 0), 1);

            var bodies = new List<CompoundShapeEntry>();
            bodies.Add(shipFuselage);
            bodies.Add(shipNose);
            bodies.Add(shipWing);
            bodies.Add(shipThrusters);

            var ship = new CompoundBody(bodies, 10);

            //Setup the launch pad and ramp
            Entity toAdd = new Box(new Vector3(10, 4, 0), 26, 1, 6);
            Space.Add(toAdd);
            toAdd = new Box(new Vector3(32, 7.8m, 0), 20, 1, 6);
            toAdd.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Forward, -MathHelper.Pi / 8);
            Space.Add(toAdd);
            toAdd = new Box(new Vector3(32, 8.8m, -3.5m), 20, 1, 1);
            toAdd.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Forward, -MathHelper.Pi / 8);
            Space.Add(toAdd);
            toAdd = new Box(new Vector3(32, 8.8m, 3.5m), 20, 1, 1);
            toAdd.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Forward, -MathHelper.Pi / 8);
            Space.Add(toAdd);
            toAdd = new Box(new Vector3(-2.75m, 5.5m, 0), .5m, 2, 3);
            Space.Add(toAdd);

            //Blast-off!
            ship.AngularDamping = .4m; //Helps keep the rocket on track for a little while longer :D
            var thruster = new Thruster(ship, new Vector3(0, -2, 0), new Vector3(0, 300, 0), 0);
            Space.Add(thruster);
            ship.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.Pi / 2) * Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi / 2);
            Space.Add(ship);


            game.Camera.Position = new Vector3(-14, 12, 25);
            game.Camera.Yaw(MathHelper.Pi / -4);
        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Spaceship"; }
        }
    }
}