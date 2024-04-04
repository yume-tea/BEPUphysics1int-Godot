using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class DodgeRight : StatePlayer
{
	public override void _Enter()
	{
		AnimationPlayer animPlayer = owner.GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer.Play("DodgeRightFix64");
	}

	public override void _Exit()
	{

	}

	public override void _Update()
	{
		Move();
	}

	public override void _OnAnimationFinished(StringName animName)
	{
		if (animName == "DodgeRightFix64") {
			ChangeState("Idle");
		}
	}

	public void Move()
	{
		// Calculate movement velocity
		BEPUutilities.Vector3 velocity = new BEPUutilities.Vector3(0,0,0);

		// Calculate Y Velocity
		if (owner.IsGrounded) {
			velocity.Y = -owner.physicsHandler.Gravity.Y * (Fix64)(1/60m); // Don't push the character towards the ground if they are grounded
		}
		else {
			velocity.Y = (owner.Weight * owner.physicsHandler.Gravity.Y * (Fix64)(1/60m) - owner.physicsHandler.Gravity.Y * (Fix64)(1/60m));
		}

		// Apply velocity
		owner.Body.LinearVelocity += velocity;
	}
}
