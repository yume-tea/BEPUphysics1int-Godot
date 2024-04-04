using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class Walk : StatePlayer
{
	public override void _Enter()
	{

	}

	public override void _Exit()
	{

	}

	public override void _Update()
	{
		Move();

		if (owner.inputHandler.IsActionPressed("jump")) {
			ChangeState("Jump");
			return;
		}

		if (owner.Body.LinearVelocity.X == 0 && owner.Body.LinearVelocity.Z == 0) {
			ChangeState("Idle");
			return;
		}
	}

	public override void _OnAnimationFinished(StringName animName)
	{

	}

	public void Move()
	{
		// Calculate movement velocity
		BEPUutilities.Vector2 direction = new BEPUutilities.Vector2(0,0);
		BEPUutilities.Vector3 velocity = new BEPUutilities.Vector3(0,0,0);
		
		if (owner.inputDirLeft.Length() > 0) {
			direction.X = owner.inputDirLeft.X;
			direction.Y = owner.inputDirLeft.Y;
		}

		direction.Rotate(-owner.cameraRotation.Y * (Fix64)0.0174533m);
		velocity = (new BEPUutilities.Vector3(direction.X, 0, direction.Y)) * owner.Speed;

		// Calculate Y Velocity
		if (owner.IsGrounded && !owner.IsJumping) {
			velocity.Y = -owner.physicsHandler.Gravity.Y * (Fix64)(1/60m); // Don't push the character towards the ground if they are grounded
		}
		else {
			velocity.Y = owner.Body.LinearVelocity.Y + (owner.Weight * owner.physicsHandler.Gravity.Y * (Fix64)(1/60m) - owner.physicsHandler.Gravity.Y * (Fix64)(1/60m));
		}

		// Apply velocity
		owner.Body.LinearVelocity = velocity;
	}
}


