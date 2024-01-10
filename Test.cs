using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;

using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;

public partial class Test : Node
{
	// Create a new space to perform a physics simulations within
	Space space = new Space();
	// Create a box to simulate
	Box box = new Box(new BEPUutilities.Vector3(1,1,1), (Fix64)1, (Fix64)1, (Fix64)1, (Fix64)8);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Add the box to the physics simulation space
		space.Add(box);
		// Set the value of gravity
		space.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, (Fix64)(-9.81), 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Update the space
		space.Update();
		// Print the position of the box through the Godot Editor
		GD.Print(new Godot.Vector3((float)box.Position.X, (float)box.Position.Y, (float)box.Position.Z));
	}
}
