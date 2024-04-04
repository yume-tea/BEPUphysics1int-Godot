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
public partial class KinematicBody : PhysicsBody
{
	public override void _Ready()
	{
        if (Godot.Engine.IsEditorHint()) return;

        physicsHandler = (PhysicsHandler) GetTree().Root.GetNode("PhysicsHandler");

		// Set the type of body this will use
		SetPhysicsBodyType(PhysicsBodyType.KinematicBody);

        // Allow the base PhysicsBody to initialize
		base._Ready();

        // Connect collision events to local function calls
        Body.CollisionInformation.Events.DetectingInitialCollision += OnBodyEntered;
        Body.CollisionInformation.Events.CollisionEnded += OnBodyExited;
	}

    public override void _PhysicsProcess(double delta)
    {
        if (Godot.Engine.IsEditorHint()) return;
        
        base._PhysicsProcess(delta);
    }

    public void Move(BEPUutilities.Vector3 value)
    {
        Body.LinearVelocity = value;
    }

    public virtual void OnBodyEntered(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
    }

    public virtual void OnBodyExited(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
	}
}
