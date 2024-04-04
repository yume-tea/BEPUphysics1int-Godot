using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


// Basic code that every level should run to maintain determinism
public partial class Level : Node
{
	protected PhysicsHandler physicsHandler;
	protected InputHandler inputHandler;

	public override void _Ready()
	{
		physicsHandler = GetNode<PhysicsHandler>("/root/PhysicsHandler");
		inputHandler = GetNode<InputHandler>("/root/InputHandler");
		physicsHandler.StartSim();
		inputHandler.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
	}
}

