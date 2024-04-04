using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;


[Tool]
public partial class RigidBody : PhysicsBody
{
	public override void _Ready()
	{
        if (Godot.Engine.IsEditorHint()) return;

		// Set the type of body this will use
		SetPhysicsBodyType(PhysicsBodyType.RigidBody);

        // Allow the base PhysicsBody to initialize
		base._Ready();
	}

    public override void _PhysicsProcess(double delta)
    {
        if (Godot.Engine.IsEditorHint()) return;

        base._PhysicsProcess(delta);
    }
}
