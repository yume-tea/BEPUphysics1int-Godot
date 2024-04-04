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
public partial class Area : PhysicsBody
{
    string materialAreaPath = "res://-custom_nodes/ColShape/Shape/materials_collision/debug_area.material";

	public override void _Ready()
	{
        // Apply area material to collision visual
        if (HasNode("ColShape/Shape")) {
            if (FileAccess.FileExists(materialAreaPath)) {
                MeshInstance3D colShapeMeshInstance = GetNode<MeshInstance3D>("ColShape/Shape");
                Material material = GD.Load<Material>(materialAreaPath);
                Mesh mesh = colShapeMeshInstance.Mesh;
                mesh.SurfaceSetMaterial(0, material);
            }
        }

        if (Godot.Engine.IsEditorHint()) return;

		// Set the type of body this will use
		SetPhysicsBodyType(PhysicsBodyType.Area);

        // Allow the base PhysicsBody to initialize
		base._Ready();

        // Connect collision events to local function calls
        Body.CollisionInformation.Events.DetectingInitialCollision += OnAreaEntered;
        Body.CollisionInformation.Events.CollisionEnded += OnAreaExited;
	}

    public override void _PhysicsProcess(double delta)
    {
        if (Godot.Engine.IsEditorHint()) {
            return;
        }
        
        base._PhysicsProcess(delta);
    }

    public void Move(BEPUutilities.Vector3 value)
    {
        Body.LinearVelocity = value;
    }

    public virtual void OnAreaEntered(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
        GD.Print("area entered");
    }

    public virtual void OnAreaExited(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
        GD.Print("area exited");
    }
}
