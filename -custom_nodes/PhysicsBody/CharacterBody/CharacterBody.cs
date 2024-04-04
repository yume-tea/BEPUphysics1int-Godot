using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Character;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;


[Tool]
public partial class CharacterBody : PhysicsBody
{
    Fix64 GroundedRayLength = (Fix64)0.18m;  // How far CheckIfGrounded checks downward for valid ground to stand on
	Fix64 SnapRayLength = (Fix64)0.19m;  // How far to check for ground to snap the character to if grounded
	Fix64 SnapHeightOffset = (Fix64)0.3m;  // Snaps the character this high above the ground on ramps..would like a better solution in the future
	public bool SnapToFloor = true;

	// Flags
	public bool IsGrounded;
	public bool IsJumping;

	public override void _Ready()
	{
        if (Engine.IsEditorHint()) return;

        physicsHandler = GetNode<PhysicsHandler>("/root/PhysicsHandler");

		// Set the type of body this will use
		SetPhysicsBodyType(PhysicsBodyType.CharacterBody);

		// Allow the base PhysicsBody to initialize
		base._Ready();
	}

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;

		if (!IsJumping) {
			CheckIfGrounded();
		}
        
        base._PhysicsProcess(delta);
    }

	// Affects the velocity of the character on the next space update
    public virtual void Move(BEPUutilities.Vector3 value)
    {
        Body.LinearVelocity += value;
    }

	// Checks if the character is close enough to the ground to be considered grounded
    protected void CheckIfGrounded()
	{
		// BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape shape = (BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape) Body.CollisionInformation.Shape;
		BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape shape = new CylinderShape(((Cylinder)Body).Height, ((Cylinder)Body).Radius - (Fix64)0.5m);
		RigidTransform transform_start = new RigidTransform(Body.Position, Body.Orientation);
		BEPUutilities.Vector3 sweep = new BEPUutilities.Vector3(0,-GroundedRayLength - SnapHeightOffset,0);
		RayCastResult result = new RayCastResult();  // Stores raycast result

		// Perform cast
		physicsHandler.space.ConvexCast(shape, ref transform_start, ref sweep, IsSelf, out result);

		// Set grounded state
		BEPUutilities.Vector3 normal = result.HitData.Normal;
		normal.Normalize();

		Fix64 NormalDotUp = BEPUutilities.Vector3.Dot(normal, BEPUutilities.Vector3.Up);

		if (NormalDotUp <= (Fix64)(-0.5)) {
			IsGrounded = true;
		}
		else {
			IsGrounded = false;
		}

		if (SnapToFloor && NormalDotUp < (Fix64) 0) {
			SnapToGround(NormalDotUp);
		}

		// Only detect collisions with standable collision
		bool IsSelf(BroadPhaseEntry entry)
		{
			return entry.CollisionRules.Group == physicsHandler.CollisionGroups[PhysicsHandler.CollisionGroupLayer.DefaultGroup];
		}
	}

	// Snaps the character to the current surface that they are considered to be grounded to
	protected void SnapToGround(Fix64 NormalDotUp)
	{
		Fix64 shapeCastHeight = ((Cylinder)Body).Height / 2;  // Use a shorter shape than character collision incase character's bottom is inside geometry

		// BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape shape = (BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape) Body.CollisionInformation.Shape;
		BEPUphysics.CollisionShapes.ConvexShapes.ConvexShape shape = new CylinderShape(shapeCastHeight, ((Cylinder)Body).Radius - (Fix64)0.5m);
		RigidTransform transform_start = new RigidTransform(Body.Position + new BEPUutilities.Vector3(0,shapeCastHeight,0), Body.Orientation);
		BEPUutilities.Vector3 sweep = new BEPUutilities.Vector3(0,-SnapRayLength - (shapeCastHeight + (shapeCastHeight / 2)) - SnapHeightOffset,0);
		RayCastResult result = new RayCastResult();  // Stores raycast result

		// Perform cast
		bool snap = physicsHandler.space.ConvexCast(shape, ref transform_start, ref sweep, IsSelf, out result);

		// Set grounded state
		if (snap) {
			Fix64 snapPositionY = result.HitData.Location.Y + (((Cylinder)Body).Height / (Fix64)2);
			if (snapPositionY < Body.Position.Y + SnapHeightOffset) {  // Prevent snapping to floor that is above the character..will only snap below midpoint of cylinder
				if (NormalDotUp < (Fix64)(-0.98)) {  // Only apply SnapHeightOffset on angled ground (to avoid getting stuck on edges..would like a better solution later)
					Body.Position = new BEPUutilities.Vector3(Body.Position.X, snapPositionY, Body.Position.Z);
				}
				else {
					Body.Position = new BEPUutilities.Vector3(Body.Position.X, snapPositionY + SnapHeightOffset, Body.Position.Z);
				}
				
			}
		}

		// Only detect collisions with standable collision
		bool IsSelf(BroadPhaseEntry entry)
		{
			return entry.CollisionRules.Group == physicsHandler.CollisionGroups[PhysicsHandler.CollisionGroupLayer.DefaultGroup];
		}
	}

	// Changes the height of the character's collider
	protected void SetHeight(Fix64 height)
	{
		Controller.StanceManager.StandingHeight = height;
		// Update collision visual
		if (HasNode("ColShape/Shape")) {
			CylinderMesh mesh = (CylinderMesh)GetNode<MeshInstance3D>("ColShape/Shape").Mesh;
			mesh.Height = (float)height;
		}
	}
}
