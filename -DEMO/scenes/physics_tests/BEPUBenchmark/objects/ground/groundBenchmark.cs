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
public partial class groundBenchmark : StaticBody  // Change inheirited class to desired PhysicsBody (CharacterBody, KinematicBody, RigidBody, StaticBody)
{
	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;

		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint()) return;

		base._PhysicsProcess(delta);
	}
}


