using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


[Tool]
public partial class statueDemo : StaticBody
{
	public override void _Ready()
	{
		if (Godot.Engine.IsEditorHint()) return;

		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Godot.Engine.IsEditorHint()) return;

		base._PhysicsProcess(delta);
	}
}


