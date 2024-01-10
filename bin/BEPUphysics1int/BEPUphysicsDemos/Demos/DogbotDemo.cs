using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;

namespace BEPUphysicsDemos.Demos
{
    /// <summary>
    /// Motorized walking robo-dog.
    /// </summary>
    public class DogbotDemo : StandardDemo
    {
        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public DogbotDemo(DemosGame game)
            : base(game)
        {
            Entity body = new Box(new Vector3(0, 0, 0), 4, 2, 2, 20);
            Space.Add(body);

            Entity head = new Cone(body.Position + new Vector3(3.2m, .3m, 0), 1.5m, .7m, 4);
            head.OrientationMatrix = Matrix3x3.CreateFromAxisAngle(Vector3.Forward, MathHelper.PiOver2);
            Space.Add(head);

            //Attach the head to the body
            var universalJoint = new UniversalJoint(body, head, head.Position + new Vector3(-.8m, 0, 0));
            Space.Add(universalJoint);
            //Keep the head from swinging around too much.
            var angleLimit = new SwingLimit(body, head, Vector3.Right, Vector3.Right, MathHelper.PiOver4);
            Space.Add(angleLimit);

            var tail = new Box(body.Position + new Vector3(-3m, 1m, 0), 1.6m, .1m, .1m, 4);
            Space.Add(tail);
            //Keep the tail from twisting itself off.
            universalJoint = new UniversalJoint(body, tail, tail.Position + new Vector3(.8m, 0, 0));
            Space.Add(universalJoint);

            //Give 'em some floppy ears.
            var ear = new Box(head.Position + new Vector3(-.2m, 0, -.65m), .01m, .7m, .2m, 1);
            Space.Add(ear);

            var ballSocketJoint = new BallSocketJoint(head, ear, head.Position + new Vector3(-.2m, .35m, -.65m));
            Space.Add(ballSocketJoint);

            ear = new Box(head.Position + new Vector3(-.2m, 0, .65m), .01m, .7m, .3m, 1);
            Space.Add(ear);

            ballSocketJoint = new BallSocketJoint(head, ear, head.Position + new Vector3(-.2m, .35m, .65m));
            Space.Add(ballSocketJoint);


            Box arm;
            Cylinder shoulder;
            PointOnLineJoint pointOnLineJoint;

            //*************  First Arm   *************//
            arm = new Box(body.Position + new Vector3(-1.8m, -.5m, 1.5m), .5m, 3, .2m, 20);
            Space.Add(arm);

            shoulder = new Cylinder(body.Position + new Vector3(-1.8m, .3m, 1.25m), .1m, .7m, 10);
            shoulder.OrientationMatrix = Matrix3x3.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2);
            Space.Add(shoulder);

            //Connect the shoulder to the body.
            var axisJoint = new RevoluteJoint(body, shoulder, shoulder.Position, Vector3.Forward);

            //Motorize the connection.
            axisJoint.Motor.IsActive = true;
            axisJoint.Motor.Settings.VelocityMotor.GoalVelocity = 1;

            Space.Add(axisJoint);

            //Connect the arm to the shoulder.
            axisJoint = new RevoluteJoint(shoulder, arm, shoulder.Position + new Vector3(0, .6m, 0), Vector3.Forward);
            Space.Add(axisJoint);

            //Connect the arm to the body.
            pointOnLineJoint = new PointOnLineJoint(arm, body, arm.Position, Vector3.Up, arm.Position + new Vector3(0, -.4m, 0));
            Space.Add(pointOnLineJoint);


            shoulder.OrientationMatrix *= Matrix3x3.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi); //Force the walker's legs out of phase.

            //*************  Second Arm   *************//
            arm = new Box(body.Position + new Vector3(1.8m, -.5m, 1.5m), .5m, 3, .2m, 20);
            Space.Add(arm);

            shoulder = new Cylinder(body.Position + new Vector3(1.8m, .3m, 1.25m), .1m, .7m, 10);
            shoulder.OrientationMatrix = Matrix3x3.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2);
            Space.Add(shoulder);

            //Connect the shoulder to the body.
            axisJoint = new RevoluteJoint(body, shoulder, shoulder.Position, Vector3.Forward);

            //Motorize the connection.
            axisJoint.Motor.IsActive = true;
            axisJoint.Motor.Settings.VelocityMotor.GoalVelocity = 1;

            Space.Add(axisJoint);

            //Connect the arm to the shoulder.
            axisJoint = new RevoluteJoint(shoulder, arm, shoulder.Position + new Vector3(0, .6m, 0), Vector3.Forward);
            Space.Add(axisJoint);


            //Connect the arm to the body.
            pointOnLineJoint = new PointOnLineJoint(arm, body, arm.Position, Vector3.Up, arm.Position + new Vector3(0, -.4m, 0));
            Space.Add(pointOnLineJoint);

            //*************  Third Arm   *************//
            arm = new Box(body.Position + new Vector3(-1.8m, -.5m, -1.5m), .5m, 3, .2m, 20);
            Space.Add(arm);

            shoulder = new Cylinder(body.Position + new Vector3(-1.8m, .3m, -1.25m), .1m, .7m, 10);
            shoulder.OrientationMatrix = Matrix3x3.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2);
            Space.Add(shoulder);

            //Connect the shoulder to the body.
            axisJoint = new RevoluteJoint(body, shoulder, shoulder.Position, Vector3.Forward);

            //Motorize the connection.
            axisJoint.Motor.IsActive = true;
            axisJoint.Motor.Settings.VelocityMotor.GoalVelocity = 1;

            Space.Add(axisJoint);

            //Connect the arm to the shoulder.
            axisJoint = new RevoluteJoint(shoulder, arm, shoulder.Position + new Vector3(0, .6m, 0), Vector3.Forward);
            Space.Add(axisJoint);

            //Connect the arm to the body.
            pointOnLineJoint = new PointOnLineJoint(arm, body, arm.Position, Vector3.Up, arm.Position + new Vector3(0, -.4m, 0));
            Space.Add(pointOnLineJoint);


            shoulder.OrientationMatrix *= Matrix3x3.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi); //Force the walker's legs out of phase.

            //*************  Fourth Arm   *************//
            arm = new Box(body.Position + new Vector3(1.8m, -.5m, -1.5m), .5m, 3, .2m, 20);
            Space.Add(arm);

            shoulder = new Cylinder(body.Position + new Vector3(1.8m, .3m, -1.25m), .1m, .7m, 10);
            shoulder.OrientationMatrix = Matrix3x3.CreateFromAxisAngle(Vector3.Right, MathHelper.PiOver2);
            Space.Add(shoulder);

            //Connect the shoulder to the body.
            axisJoint = new RevoluteJoint(body, shoulder, shoulder.Position, Vector3.Forward);

            //Motorize the connection.
            axisJoint.Motor.IsActive = true;
            axisJoint.Motor.Settings.VelocityMotor.GoalVelocity = 1;

            Space.Add(axisJoint);

            //Connect the arm to the shoulder.
            axisJoint = new RevoluteJoint(shoulder, arm, shoulder.Position + new Vector3(0, .6m, 0), Vector3.Forward);
            Space.Add(axisJoint);

            //Connect the arm to the body.
            pointOnLineJoint = new PointOnLineJoint(arm, body, arm.Position, Vector3.Up, arm.Position + new Vector3(0, -.4m, 0));
            Space.Add(pointOnLineJoint);

            //Add some ground.
            Space.Add(new Box(new Vector3(0, -3.5m, 0), 20m, 1, 20m));

            game.Camera.Position = new Vector3(0, 2, 20);
        }

        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Dogbot"; }
        }
    }
}