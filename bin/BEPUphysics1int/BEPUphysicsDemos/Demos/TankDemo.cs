using System.Collections.Generic;
using System.Diagnostics;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Constraints.TwoEntity.Motors;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using BEPUphysics.Entities;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.TwoEntity.JointLimits;
using Microsoft.Xna.Framework.Input;
using System;
using FixMath.NET;

namespace BEPUphysicsDemos.Demos
{

    public class Tank
    {
        public Tread LeftTread { get; private set; }
        public Tread RightTread { get; private set; }
        public Turret Turret { get; private set; }

        public Entity Body { get; private set; }

        public Tank(Vector3 position)
        {
            Body = new Box(position, 4, (Fix64).5m, 5, 20);
            Body.CollisionInformation.LocalPosition = new Vector3(0, (Fix64).8m, 0);

            var treadDescription = new TreadSegmentDescription
            {
                Width = (Fix64)0.5m,
                Radius = (Fix64)0.5m,
                Mass = 1,
                Friction = (Fix64)2f,
                MotorSoftness = 0,//0.3m,
                MotorMaximumForce = 30,
                SuspensionDamping = 70,
                SuspensionStiffness = 300,
                SuspensionLength = 1
            };

            RightTread = new Tread(Body, new Vector3((Fix64)(-1.8m), (Fix64)(-.2m), (Fix64)(-2.1m)), 5, 1, treadDescription);
            LeftTread = new Tread(Body, new Vector3((Fix64)1.8m, (Fix64)(-.2m), (Fix64)(-2.1m)), 5, 1, treadDescription);

            Turret = new Turret(Body, new Vector3(0, (Fix64)1.5m, (Fix64)0.5m));
        }

        public void AddToSpace(Space space)
        {
            LeftTread.AddToSpace(space);
            RightTread.AddToSpace(space);
            Turret.AddToSpace(space);
            space.Add(Body);
        }

        public void RemoveFromSpace(Space space)
        {
            LeftTread.RemoveFromSpace(space);
            RightTread.RemoveFromSpace(space);
            Turret.RemoveFromSpace(space);
            space.Remove(Body);
        }
    }

    public class Turret
    {
        /// <summary>
        /// Gets the main body of the turret. This is connected to the tank body directly.
        /// </summary>
        public Cylinder Body { get; private set; }

        /// <summary>
        /// Gets the turret arm. This is connected to the turret body.
        /// </summary>
        public Cylinder Barrel { get; private set; }

        /// <summary>
        /// Gets the constraint which binds the turret body to the tank body. 
        /// </summary>
        public RevoluteJoint TankToTurretJoint { get; private set; }

        /// <summary>
        /// Gets the constraint which binds the turret arm to the body.
        /// </summary>
        public RevoluteJoint TurretBodyToBarrelJoint { get; private set; }

        /// <summary>
        /// Gets or sets the goal of the yaw motor. Convenience property; accesses TurretToTankJoint.Motor.Settings.Servo.Goal.
        /// </summary>
        public Fix64 YawGoal
        {
            get { return TankToTurretJoint.Motor.Settings.Servo.Goal; }
            set { TankToTurretJoint.Motor.Settings.Servo.Goal = value; }
        }

        /// <summary>
        /// Gets or sets the goal of the pitch motor. Convenience property; accesses TurretToTankJoint.Motor.Settings.Servo.Goal.
        /// </summary>
        public Fix64 PitchGoal
        {
            get { return TurretBodyToBarrelJoint.Motor.Settings.Servo.Goal; }
            set { TurretBodyToBarrelJoint.Motor.Settings.Servo.Goal = MathHelper.Clamp(value, MinimumPitch, MaximumPitch); }
        }

        /// <summary>
        /// Gets or sets the minimum allowed pitch of the tank turret arm. The servo goal will be clamped between the minimum and maximum.
        /// </summary>
        public Fix64 MinimumPitch { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed pitch of the tank turret arm. The servo goal will be clamped between the minimum and maximum.
        /// </summary>
        public Fix64 MaximumPitch { get; set; }


        private Fix64 TimeBetweenFiring = (Fix64)1.8m;
        private Fix64 lastFireTime;

        private const int MaximumShellCount = 3;
        private Queue<Sphere> shellPool;

        public Turret(Entity tankBody, Vector3 offset)
        {
            var position = offset + tankBody.Position;
            Body = new Cylinder(position, (Fix64)0.7m, (Fix64)0.8m, 8);
            //Position the center of the arm a bit further forward since it will be laying down.
            position.Z -= 2;
            Barrel = new Cylinder(position, 3, (Fix64)0.2m, 3);
            //Rotate the arm so that it points straight forward (that is, along {0, 0, -1}).
            Barrel.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), MathHelper.PiOver2);

            TankToTurretJoint = new RevoluteJoint(tankBody, Body, Body.Position, Vector3.Up);
            TurretBodyToBarrelJoint = new RevoluteJoint(Body, Barrel, Body.Position + new Vector3(0, 0, (Fix64)(-0.5m)), Vector3.Left);

            //Turn on the control constraints. We'll put them into servo mode, but velocity mode would also work just fine.
            TankToTurretJoint.Motor.IsActive = true;
            TankToTurretJoint.Motor.Settings.Mode = MotorMode.Servomechanism;
            TankToTurretJoint.Motor.Settings.Servo.BaseCorrectiveSpeed = (Fix64)0.5m;
            TankToTurretJoint.Motor.Settings.MaximumForce = 500;
            TankToTurretJoint.Motor.Basis.SetLocalAxes(Vector3.Up, Vector3.Forward);

            TurretBodyToBarrelJoint.Motor.IsActive = true;
            TurretBodyToBarrelJoint.Motor.Settings.Mode = MotorMode.Servomechanism;
            TurretBodyToBarrelJoint.Motor.Settings.Servo.BaseCorrectiveSpeed = (Fix64)0.5m;
            //We take special care to limit this motor's force to stop the turret from smashing the tank body.
            TurretBodyToBarrelJoint.Motor.Settings.MaximumForce = 300;
            TurretBodyToBarrelJoint.Motor.Basis.SetLocalAxes(Vector3.Right, Vector3.Forward);

            //Don't let the directly connected objects generate collisions. The arm can still hit the tank body, though.
            CollisionRules.AddRule(Body, tankBody, CollisionRule.NoBroadPhase);
            CollisionRules.AddRule(Barrel, Body, CollisionRule.NoBroadPhase);

            //Don't have a lot of leeway downards. The turret will quickly bump the tank body.
            MinimumPitch = -MathHelper.Pi * (Fix64)0.05m;
            MaximumPitch = MathHelper.Pi * (Fix64)0.4m;

            //Build an ammunition pool.
            shellPool = new Queue<Sphere>(MaximumShellCount);

            for (int i = 0; i < MaximumShellCount; ++i)
            {
                var shell = new Sphere(new Vector3(10000, 0, 0), (Fix64)0.2m, 2);
                shell.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
                shellPool.Enqueue(shell);
            }


        }




        public void AddToSpace(Space space)
        {
            space.Add(Body);
            space.Add(Barrel);
            space.Add(TankToTurretJoint);
            space.Add(TurretBodyToBarrelJoint);
        }

        public void RemoveFromSpace(Space space)
        {
            space.Remove(Body);
            space.Remove(Barrel);
            space.Remove(TankToTurretJoint);
            space.Remove(TurretBodyToBarrelJoint);
        }

        /// <summary>
        /// Attempts to fire a projectile.
        /// </summary>
        /// <param name="shell">The shell fired by the tank. Null if the firing did not succeed.</param>
        /// <returns>True if a shell was fired, false otherwise.</returns>
        public bool TryToFire(out Sphere shell)
        {
            if (Barrel.Space != null)
            {
                var currentTime = (Fix64)(Stopwatch.GetTimestamp() / (decimal)Stopwatch.Frequency);
                if (currentTime - lastFireTime > TimeBetweenFiring)
                {
                    lastFireTime = currentTime;

                    //Re-use an old shell.
                    shell = shellPool.Dequeue();
                    shellPool.Enqueue(shell);
                    if (shell.Space == null)
                    {
                        //This shell has never been added before.
                        Barrel.Space.Add(shell);
                    }

                    //Reset the angular state.
                    shell.Orientation = Quaternion.Identity;
                    shell.AngularVelocity = Barrel.AngularVelocity;

                    //Position the shell at the end of the turret.
                    var firingDirection = Barrel.OrientationMatrix.Down;
                    shell.Position = Barrel.Position + firingDirection * (Barrel.Height * (Fix64)0.5m + (Fix64)(0.2m * 1.1m));

                    //Shoot it!
                    shell.LinearVelocity = Barrel.LinearVelocity + firingDirection * 100;
                    Barrel.LinearMomentum += -firingDirection * 20 * shell.Mass;

                    return true;
                }

            }
            shell = null;
            return false;
        }
    }

    public class Tread
    {
        /// <summary>
        /// Gets the segments composing this tread.
        /// </summary>
        public List<TreadSegment> Segments { get; private set; }

        /// <summary>
        /// Gets the constraints which bind the segments together, stopping them from rotating separately.
        /// </summary>
        public List<NoRotationJoint> SegmentAngularBindings { get; private set; }

        public Tread(Entity tankBody, Vector3 offsetToFrontOfTread, int segmentCount, Fix64 spacing, TreadSegmentDescription treadSegmentDescription)
        {
            Segments = new List<TreadSegment>();
            Vector3 nextSegmentPosition = tankBody.Position + offsetToFrontOfTread;
            //The front of the tread includes the radius of the first segment.
            nextSegmentPosition.Z += treadSegmentDescription.Radius * (Fix64)0.5m;
            for (int i = 0; i < segmentCount; ++i)
            {
                Segments.Add(new TreadSegment(nextSegmentPosition, tankBody, treadSegmentDescription));

                //The tread offset starts at the front of the vehicle and moves backward.
                nextSegmentPosition.Z += spacing;
            }


            //Don't let the tread segments rotate relative to each other.
            SegmentAngularBindings = new List<NoRotationJoint>();
            for (int i = 1; i < segmentCount; ++i)
            {
                //Create constraints linking the segments together to ensure that the power of one motor is felt by other segments.
                SegmentAngularBindings.Add(new NoRotationJoint(Segments[i - 1].Entity, Segments[i].Entity));
                //Don't let the tread segments collide.
                CollisionRules.AddRule(Segments[i - 1].Entity, Segments[i].Entity, CollisionRule.NoBroadPhase);
            }

            //Note: You can organize this in different ways. For example, you could have one motor which drives one wheel, which
            //in turn drives other wheels through these NoRotationJoints.

            //In such a one-motor model, it may be a good idea for stability to bind all wheels directly to the drive wheel with 
            //NoRotationJoints rather than using a chain of one wheel to the next.

            //Per-wheel drive motors are used in this example just because it is slightly more intuitive at a glance.
            //Each segment is no different than the others.
        }

        public void SetTreadGoalVelocity(Fix64 goal)
        {
            foreach (var segment in Segments)
            {
                segment.GoalSpeed = goal;
            }
        }


        public void AddToSpace(Space space)
        {
            foreach (var segment in Segments)
            {
                segment.AddToSpace(space);
            }
            foreach (var binding in SegmentAngularBindings)
            {
                space.Add(binding);
            }
        }

        public void RemoveFromSpace(Space space)
        {
            foreach (var segment in Segments)
            {
                segment.RemoveFromSpace(space);
            }
            foreach (var binding in SegmentAngularBindings)
            {
                space.Remove(binding);
            }
        }
    }

    public struct TreadSegmentDescription
    {
        public Fix64 Mass;
        public Fix64 Width;
        public Fix64 Radius;
        public Fix64 Friction;
        public Fix64 SuspensionLength;
        public Fix64 SuspensionStiffness;
        public Fix64 SuspensionDamping;
        public Fix64 MotorSoftness;
        public Fix64 MotorMaximumForce;
    }

    public class TreadSegment
    {
        /// <summary>
        /// Gets the entity body associated with this tread segment.
        /// </summary>
        public Entity Entity { get; private set; }

        /// <summary>
        /// Gets the driving motor associated with the segment.
        /// </summary>
        public RevoluteMotor Motor { get; private set; }

        /// <summary>
        /// Gets the constraint which keeps the segment along the suspension axis.
        /// </summary>
        public PointOnLineJoint SuspensionAxisJoint { get; private set; }

        /// <summary>
        /// Gets the constraint that stops the segment from going too high or too low along the suspension.
        /// </summary>
        public LinearAxisLimit SuspensionLengthLimit { get; private set; }

        /// <summary>
        /// Gets the constraint which tries to push the segment away to support the tank.
        /// </summary>
        public LinearAxisMotor SuspensionSpring { get; private set; }

        /// <summary>
        /// Gets the constraint which keeps the wheel angularly aligned with the body. Allows rotation around the driving axis.
        /// </summary>
        public RevoluteAngularJoint SuspensionAngularJoint { get; private set; }

        /// <summary>
        /// Gets or sets the goal angular velocity for this segment.
        /// </summary>
        public Fix64 GoalSpeed
        {
            get { return Motor.Settings.VelocityMotor.GoalVelocity; }
            set { Motor.Settings.VelocityMotor.GoalVelocity = value; }
        }

        public TreadSegment(Vector3 segmentPosition, Entity body, TreadSegmentDescription treadSegmentDescription)
        {
            Entity = new Cylinder(segmentPosition, treadSegmentDescription.Width, treadSegmentDescription.Radius, treadSegmentDescription.Mass);

            Entity.Material.KineticFriction = treadSegmentDescription.Friction;
            Entity.Material.StaticFriction = treadSegmentDescription.Friction;
            Entity.Orientation = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.PiOver2);

            //Preventing the occasional pointless collision pair can speed things up.
            CollisionRules.AddRule(Entity, body, CollisionRule.NoBroadPhase);

            //Connect the wheel to the body.
            SuspensionAxisJoint = new PointOnLineJoint(body, Entity, Entity.Position, Vector3.Down, Entity.Position);
            SuspensionLengthLimit = new LinearAxisLimit(body, Entity, Entity.Position, Entity.Position, Vector3.Down, -treadSegmentDescription.SuspensionLength, 0);
            //This linear axis motor will give the suspension its springiness by pushing the wheels outward.
            SuspensionSpring = new LinearAxisMotor(body, Entity, Entity.Position, Entity.Position, Vector3.Down);
            SuspensionSpring.Settings.Mode = MotorMode.Servomechanism;
            SuspensionSpring.Settings.Servo.Goal = 0;
            SuspensionSpring.Settings.Servo.SpringSettings.Stiffness = treadSegmentDescription.SuspensionStiffness;
            SuspensionSpring.Settings.Servo.SpringSettings.Damping = treadSegmentDescription.SuspensionDamping;

            SuspensionAngularJoint = new RevoluteAngularJoint(body, Entity, Vector3.Right);
            //Make the joint extremely rigid.  There are going to be extreme conditions when the wheels get up to speed;
            //we don't want the forces involved to torque the wheel off the frame!
            SuspensionAngularJoint.SpringSettings.Damping *= Entity.Mass * 50;
            SuspensionAngularJoint.SpringSettings.Stiffness *= Entity.Mass * 50;
            //Motorize the wheel.
            Motor = new RevoluteMotor(body, Entity, Vector3.Left);
            Motor.Settings.VelocityMotor.Softness = treadSegmentDescription.MotorSoftness;
            Motor.Settings.MaximumForce = treadSegmentDescription.MotorMaximumForce;

        }

        public void AddToSpace(Space space)
        {
            space.Add(Entity);
            space.Add(Motor);
            space.Add(SuspensionAxisJoint);
            space.Add(SuspensionLengthLimit);
            space.Add(SuspensionSpring);
            space.Add(SuspensionAngularJoint);
        }

        public void RemoveFromSpace(Space space)
        {
            space.Remove(Entity);
            space.Remove(Motor);
            space.Remove(SuspensionAxisJoint);
            space.Remove(SuspensionLengthLimit);
            space.Remove(SuspensionSpring);
            space.Remove(SuspensionAngularJoint);
        }
    }

    /// <summary>
    /// A tank built from constraints and entities (as opposed to using the Vehicle class) drives around on a terrain and 
    /// sometimes a box. Also, there's other dudes driving around.
    /// </summary>
    /// <remarks>
    /// This demo type is initially excluded from the main list in the DemosGame.
    /// To access it while playing the demos, add an entry to the demoTypes array for this TestDemo.
    /// </remarks>
    public class TankDemo : StandardDemo
    {
        private Tank playerTank;
        private List<Tank> autoTanks;


        /// <summary>
        /// Constructs a new demo.
        /// </summary>
        /// <param name="game">Game owning this demo.</param>
        public TankDemo(DemosGame game)
            : base(game)
        {
            game.Camera.Position = new Vector3(0, 2, 15);

            Space.Add(new Box(new Vector3(0, -5, 0), 20, 1, 20));

            playerTank = new Tank(new Vector3(0, 0, 0));
            playerTank.AddToSpace(Space);



            //Create a bunch of other tanks.
            autoTanks = new List<Tank>();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    var autoTank = new Tank(new Vector3(-30 + i * 10, 30, -30 + j * 10));
                    autoTanks.Add(autoTank);
                    autoTank.AddToSpace(Space);
                }
            }


            //x and y, in terms of heightmaps, refer to their local x and y coordinates.  In world space, they correspond to x and z.
            //Setup the heights of the terrain.
            int xLength = 180;
            int zLength = 180;

			Fix64 xSpacing = 8;
			Fix64 zSpacing = 8;
            var heights = new Fix64[xLength, zLength];
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < zLength; j++)
                {
					Fix64 x = i - xLength / 2;
					Fix64 z = j - zLength / 2;
                    //heights[i,j] = (Fix64)(x * y / 1000f);
                    heights[i, j] = 20 * (Fix64.Sin(x / 8) + Fix64.Sin(z / 8));
                    //heights[i,j] = 3 * (Fix64)Math.Sin(x * y / 100f);
                    //heights[i,j] = (x * x * x * y - y * y * y * x) / 1000f;
                }
            }
            //Create the terrain.
            var terrain = new Terrain(heights, new AffineTransform(
                    new Vector3(xSpacing, 1, zSpacing),
                    Quaternion.Identity,
                    new Vector3(-xLength * xSpacing / 2, -10, -zLength * zSpacing / 2)));

            //terrain.Thickness = 5; //Uncomment this and shoot some things at the bottom of the terrain! They'll be sucked up through the ground.

            Space.Add(terrain);

            game.ModelDrawer.Add(terrain);
        }



        /// <summary>
        /// Gets the name of the simulation.
        /// </summary>
        public override string Name
        {
            get { return "Tank Demo"; }
        }

        public override void Update(Fix64 dt)
        {
			Fix64 driveSpeed = 20;
            if (Game.KeyboardInput.IsKeyDown(Keys.Up))
            {
                if (Game.KeyboardInput.IsKeyDown(Keys.Right))
                {
                    //Turn right while still allowing forward motion
                    playerTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(0);
                }
                else if (Game.KeyboardInput.IsKeyDown(Keys.Left))
                {
                    //Turn left while still allowing forward motion
                    playerTank.RightTread.SetTreadGoalVelocity(0);
                    playerTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                }
                else
                {
                    //Go forward
                    playerTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                }
            }
            else if (Game.KeyboardInput.IsKeyDown(Keys.Down))
            {

                if (Game.KeyboardInput.IsKeyDown(Keys.Right))
                {
                    //Turn right while still allowing backward motion
                    playerTank.RightTread.SetTreadGoalVelocity(-driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(0);
                }
                else if (Game.KeyboardInput.IsKeyDown(Keys.Left))
                {
                    //Turn left while still allowing backward motion
                    playerTank.RightTread.SetTreadGoalVelocity(0);
                    playerTank.LeftTread.SetTreadGoalVelocity(-driveSpeed);
                }
                else
                {
                    //Go backward
                    playerTank.RightTread.SetTreadGoalVelocity(-driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(-driveSpeed);
                }
            }
            else
            {
                if (Game.KeyboardInput.IsKeyDown(Keys.Right))
                {
                    //Turn right
                    playerTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(-driveSpeed);
                }
                else if (Game.KeyboardInput.IsKeyDown(Keys.Left))
                {
                    //Turn left
                    playerTank.RightTread.SetTreadGoalVelocity(-driveSpeed);
                    playerTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                }
                else
                {
                    //Stop
                    playerTank.RightTread.SetTreadGoalVelocity(0);
                    playerTank.LeftTread.SetTreadGoalVelocity(0);
                }
            }

            //Control the turret
            if (Game.KeyboardInput.IsKeyDown(Keys.NumPad8))
                playerTank.Turret.PitchGoal += (Fix64)1.5m * dt;
            if (Game.KeyboardInput.IsKeyDown(Keys.NumPad5))
                playerTank.Turret.PitchGoal -= (Fix64)1.5m * dt;
            if (Game.KeyboardInput.IsKeyDown(Keys.NumPad4))
                playerTank.Turret.YawGoal += (Fix64)1.5m * dt;
            if (Game.KeyboardInput.IsKeyDown(Keys.NumPad6))
                playerTank.Turret.YawGoal -= (Fix64)1.5m * dt;

            if (Game.KeyboardInput.IsKeyDown(Keys.NumPad0))
            {
                Sphere firedShell;
                if (playerTank.Turret.TryToFire(out firedShell))
                {
                    Game.ModelDrawer.Add(firedShell);
                }
            }



            //Now tell the AI to do things.
            foreach (var autoTank in autoTanks)
            {
                if (aiRandom.Next(0, 100) >= 98)
                {
                    //Small chance to change state.
                    var stateRandom = aiRandom.Next(0, 6);
                    switch (stateRandom)
                    {
                        case 0:
                            //Go forward.                   
                            autoTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                            autoTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                            break;
                        case 1:
                            //Go backward.
                            autoTank.RightTread.SetTreadGoalVelocity(-driveSpeed);
                            autoTank.LeftTread.SetTreadGoalVelocity(-driveSpeed);
                            break;
                        case 2:
                            //Turn one way.
                            autoTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                            autoTank.LeftTread.SetTreadGoalVelocity(0);
                            break;
                        case 3:
                            //Turn the other way.
                            autoTank.RightTread.SetTreadGoalVelocity(0);
                            autoTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                            break;
                        case 4:
                            //Turn one way.
                            autoTank.RightTread.SetTreadGoalVelocity(driveSpeed);
                            autoTank.LeftTread.SetTreadGoalVelocity(-driveSpeed);
                            break;
                        case 5:
                            //Turn the other way.
                            autoTank.RightTread.SetTreadGoalVelocity(-driveSpeed);
                            autoTank.LeftTread.SetTreadGoalVelocity(driveSpeed);
                            break;
                    }
                }

                //Now aim the turret.
                var random = aiRandom.Next(0, 100);
                if (random >= 98)
                    autoTank.Turret.YawGoal += (Fix64)0.2m;
                if (random <= 2)
                    autoTank.Turret.YawGoal -= (Fix64)0.2m;
                random = aiRandom.Next(0, 100);
                if (random >= 98)
                    autoTank.Turret.PitchGoal += (Fix64)0.1m;
                if (random <= 2)
                    autoTank.Turret.PitchGoal -= (Fix64)0.1m;
                if (aiRandom.Next(0, 100) == 0)
                {
                    Sphere firedShell;
                    if (autoTank.Turret.TryToFire(out firedShell))
                    {
                        Game.ModelDrawer.Add(firedShell);
                    }
                }
            }


            base.Update(dt);
        }

        public override void DrawUI()
        {           
            Game.DataTextDrawer.Draw("Use arrows/numpad to control your tank.", new Microsoft.Xna.Framework.Vector2(50, 50));
            base.DrawUI();
        }

        private Random aiRandom = new Random(5);
    
    }
}