using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class Idle : StatePlayer
{
	public override void _Enter()
	{
		
	}

	public override void _Exit()
	{

	}

	public override void _Update()
	{
		owner.inputDirLeft = owner.inputHandler.GetJoyAxisLeft();
		owner.cameraRotation = owner.cameraRig.cameraRotation;

		Move();

		if (owner.inputHandler.IsActionPressed("jump") && owner.IsGrounded) {
			ChangeState("Jump");
			return;
		}
		if (owner.inputDirLeft.Length() > 0) {
			ChangeState("Walk");
			return;
		}
		if (owner.inputHandler.IsActionPressed("dodge_right")) {
			ChangeState("DodgeRight");
			return;
		}
		if (owner.inputHandler.IsActionPressed("dodge_left")) {
			ChangeState("DodgeLeft");
			return;
		}
	}

	public override void _OnAnimationFinished(StringName animName)
	{

	}

	public void Move()
	{
		// Calculate movement velocity
		BEPUutilities.Vector3 velocity = new BEPUutilities.Vector3(0,0,0);

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


