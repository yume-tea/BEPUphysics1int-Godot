using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class Global : Node
{
	public InputHandler inputHandler;
	public PhysicsBody Player;

	public override void _Ready()
	{
		inputHandler = GetNode<InputHandler>("/root/InputHandler");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (inputHandler.IsActionPressed("quit")) {
			GetTree().Quit();
		}
	}
}










