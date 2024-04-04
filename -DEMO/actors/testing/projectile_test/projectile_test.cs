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
//using BEPUphysics.BroadPhaseSystems;
//using BEPUphysics.CollisionTests;


[Tool]
public partial class projectile_test : KinematicBody
{
	PhysicsBody target;
	Fix64 speed = (Fix64)8;

	public override void _Ready()
	{
		base._Ready();

		if (Godot.Engine.IsEditorHint()) return;

		// Set target to player
		Global global = (Global) GetTree().Root.GetNode("Global");
		target = global.Player;
	}

    public override void _PhysicsProcess(double delta)
    {
		if (Godot.Engine.IsEditorHint()) return;

        base._PhysicsProcess(delta);

		// Move towards target
		BEPUutilities.Vector3 dir = target.Body.Position - Body.Position;
		dir.Normalize();
		Body.LinearVelocity = dir * speed;
    }

    public override void OnBodyEntered(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
        if (Godot.Engine.IsEditorHint()) return;

		if (other.Owner.Body != target.Body) {
			return;
		}

		GD.Print("projectile hit target");

		QueueFree();
    }

    public override void OnBodyExited(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
		if (Godot.Engine.IsEditorHint()) return;
	}
}

