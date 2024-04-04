using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class PhysicsDemoSpace : Level
{
	Global global;

	public override void _Ready()
	{
		base._Ready();

		global = GetTree().Root.GetNode<Global>("Global");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (global.Player.Body.Position.Y < (Fix64)(-72)) {
			global.Player.Body.Position = (new BEPUutilities.Vector3(0,0,16)) + global.Player.colShape.PosOffset;
		}
	}
}

