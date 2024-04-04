using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class cameraRigPlayer : Node3D
{
	InputHandler inputHandler;
	BEPUutilities.Vector2 inputDirRight = new BEPUutilities.Vector2();

	public BEPUutilities.Vector2 cameraRotation = new BEPUutilities.Vector2();
	public BEPUutilities.Vector2 cameraDir = new BEPUutilities.Vector2();

	Fix64 sensitivity = 1.6m;
	BEPUutilities.Vector2 rotationLimit = new BEPUutilities.Vector2(90, 360);

	Node3D Pivot;

	public override void _Ready()
	{
		inputHandler = GetNode<InputHandler>("/root/InputHandler");
		Pivot = GetNode<Node3D>("Pivot");
	}

	public override void _PhysicsProcess(double delta)
	{
		inputDirRight = inputHandler.GetJoyAxisRight();
		cameraDir.X = (Fix64)Pivot.GlobalTransform.Basis.Z.X;
		cameraDir.Y = (Fix64)Pivot.GlobalTransform.Basis.Z.Y;

		MoveCamera();
	}

	public void MoveCamera()
	{
		BEPUutilities.Vector2 rotAmount;

		rotAmount.X = -inputDirRight.Y * sensitivity;
		rotAmount.Y = -inputDirRight.X * sensitivity;

		cameraRotation += rotAmount;

		// Correct rotation if it goes past 360 degrees
		if (cameraRotation.X >= (Fix64)360) {
			cameraRotation.X -= (Fix64)360;
		}
		if (cameraRotation.X <= (Fix64)(-360)) {
			cameraRotation.X += (Fix64)360;
		}
		if (cameraRotation.Y >= (Fix64)360) {
			cameraRotation.Y -= (Fix64)360;
		}
		if (cameraRotation.Y <= (Fix64)(-360)) {
			cameraRotation.Y += (Fix64)360;
		}

		// Limit rotation
		if (cameraRotation.X > rotationLimit.X) {
			cameraRotation.X = rotationLimit.X;
		}
		if (cameraRotation.X < -rotationLimit.X) {
			cameraRotation.X = -rotationLimit.X;
		}
		if (cameraRotation.Y > rotationLimit.Y) {
			cameraRotation.Y = rotationLimit.Y;
		}
		if (cameraRotation.Y < -rotationLimit.Y) {
			cameraRotation.Y = -rotationLimit.Y;
		}

		Set("rotation", new Godot.Vector3(0,(float)(cameraRotation.Y * (Fix64)0.0174533m),0));
		Pivot.Set("rotation", new Godot.Vector3((float)(cameraRotation.X * (Fix64)0.0174533m),0,0));
	}
}


