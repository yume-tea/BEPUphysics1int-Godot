using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;


[Tool]
public partial class StaticBody : PhysicsBody
{
	public override void _Ready()
	{
		if (Godot.Engine.IsEditorHint()) return;
		
		// Set the type of body this will use
		SetPhysicsBodyType(PhysicsBodyType.StaticBody);

		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
    {
        if (Godot.Engine.IsEditorHint()) return;
    }
}
