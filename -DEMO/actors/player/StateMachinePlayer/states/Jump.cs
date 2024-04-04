using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class Jump : StatePlayer
{
	public override void _Enter()
	{
		owner.IsJumping = true;
		owner.Body.LinearVelocity += new BEPUutilities.Vector3(0, owner.JumpVelocity, 0);
	}

	public override void _Exit()
	{
		owner.IsJumping = false;
	}

	public override void _Update()
	{
		if (owner.Body.LinearVelocity.Y <= 0) {
			owner.IsJumping = false;
		}

		Move();

		if (owner.IsGrounded && !owner.IsJumping) {
			ChangeState("Idle");
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

		// Calculate horizontal velocity
		direction.Rotate(-owner.cameraRotation.Y * (Fix64)0.0174533m);
		velocity = (new BEPUutilities.Vector3(direction.X, 0, direction.Y)) * owner.Speed;

		// Calculate Y Velocity
		velocity.Y = owner.Body.LinearVelocity.Y + (owner.Weight * owner.physicsHandler.Gravity.Y * (Fix64)(1/60m) - owner.physicsHandler.Gravity.Y * (Fix64)(1/60m));

		// Apply velocity
		owner.Body.LinearVelocity = velocity;
	}
}


