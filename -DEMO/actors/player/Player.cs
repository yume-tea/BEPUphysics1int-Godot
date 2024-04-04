using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Character;
using BEPUphysics.Entities;
using BEPUutilities;
//using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;


[Tool]
public partial class Player : CharacterBody
{
	public InputHandler inputHandler;
	public BEPUutilities.Vector2 inputDirLeft = new BEPUutilities.Vector2();

	public cameraRigPlayer cameraRig;
	public BEPUutilities.Vector2 cameraRotation;

	public Fix64 Speed = 14;
	public Fix64 JumpVelocity = 20m;
	public Fix64 Weight = 4.0m;  // m specifies that this is a decimal value

	public override void _Ready()
	{
		if (Engine.IsEditorHint()) {
			return;
		}

		base._Ready();

		Global global = GetNode<Global>("/root/Global");
		global.Player = (PhysicsBody) this;

		inputHandler = GetNode<InputHandler>("/root/InputHandler");
		cameraRig = GetNode<cameraRigPlayer>("CameraRigPlayer");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) {
			return;
		}
		
		// Update player values
		inputDirLeft = inputHandler.GetJoyAxisLeft();
		cameraRotation = cameraRig.cameraRotation;

		base._PhysicsProcess(delta);
	}

	// Used for applying velocity from an animation using raw value keys (Int64 values)
	public void Move(long rawValueX, long rawValueY, long rawValueZ)
	{
		BEPUutilities.Vector3 valueNew = new BEPUutilities.Vector3(Fix64.FromRaw(rawValueX), Fix64.FromRaw(rawValueY), Fix64.FromRaw(rawValueZ));

		BEPUutilities.Vector2 direction = new BEPUutilities.Vector2(valueNew.X, valueNew.Z);
		BEPUutilities.Vector3 velocity;

		direction.Rotate(-cameraRotation.Y * (Fix64)0.0174533m);

		velocity = new BEPUutilities.Vector3(direction.X, valueNew.Y, direction.Y);

		Body.LinearVelocity = velocity;
	}
}

