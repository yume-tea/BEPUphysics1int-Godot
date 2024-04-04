using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;

// Collision libraries
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


[Tool]
public partial class area_test : Area
{
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void OnAreaEntered(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
		GD.Print("entered area body");
    }

	public override void OnAreaExited(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
        GD.Print("exited area body");
    }
}

