using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionShapes;
using BEPUphysics.BroadPhaseEntries;  // StaticMesh
using BEPUphysics.Character;
using BEPUphysics.CollisionRuleManagement;
using BEPUutilities;
using System.Collections.Generic;

/* Static Bodies are currently created as Kinematic Bodies */


[Tool]
public partial class PhysicsBody : Node3D
{
	public enum PhysicsBodyType
	{
		Unset,
		CharacterBody,
		KinematicBody,
		RigidBody,
		StaticBody,
		Area
	}

	private int Mass = 1;
    [Export]
    private int mass {
        get => Mass;
        set {
            Mass = value;
        }
    }

	public PhysicsHandler.CollisionGroupLayer CollisionGroup;
	[Export]
	private PhysicsHandler.CollisionGroupLayer _CollisionGroup {
		get => CollisionGroup;
		set {
			CollisionGroup = value;
		}
	}

	public BEPUutilities.Vector3 PosOffset = new BEPUutilities.Vector3();  // This is the value BEPU uses as the origin when creating this PhysicsBody
	[Export]
	private Godot.Vector3 posOffset {
		get => new Godot.Vector3((float)Fix64.FromRaw(PosOffsetX), (float)Fix64.FromRaw(PosOffsetY), (float)Fix64.FromRaw(PosOffsetZ));
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				PosOffsetX = ((Fix64)((decimal)value.X)).RawValue;
				PosOffsetY = ((Fix64)((decimal)value.Y)).RawValue;
				PosOffsetZ = ((Fix64)((decimal)value.Z)).RawValue;
			}
		}
	}
	private long PosOffsetX;
	[Export]
	private long posOffsetX {
		get => PosOffsetX;
		set {
			PosOffsetX = value;
			PosOffset = new BEPUutilities.Vector3(Fix64.FromRaw(PosOffsetX), Fix64.FromRaw(PosOffsetY), Fix64.FromRaw(PosOffsetZ));
		}
	}
	private long PosOffsetY;
	[Export]
	private long posOffsetY {
		get => PosOffsetY;
		set {
			PosOffsetY = value;
			PosOffset = new BEPUutilities.Vector3(Fix64.FromRaw(PosOffsetX), Fix64.FromRaw(PosOffsetY), Fix64.FromRaw(PosOffsetZ));
		}
	}
	private long PosOffsetZ;
	[Export]
	private long posOffsetZ {
		get => PosOffsetZ;
		set {
			PosOffsetZ = value;
			PosOffset = new BEPUutilities.Vector3(Fix64.FromRaw(PosOffsetX), Fix64.FromRaw(PosOffsetY), Fix64.FromRaw(PosOffsetZ));
		}
	}

	public BEPUutilities.Quaternion QuatOffset = new BEPUutilities.Quaternion(0,0,0,1);  // This is the value BEPU uses as the origin when creating this PhysicsBody
	[Export]
	private Godot.Quaternion quatOffset {
		get => new Godot.Quaternion((float)Fix64.FromRaw(QuatOffsetX), (float)Fix64.FromRaw(QuatOffsetY), (float)Fix64.FromRaw(QuatOffsetZ), (float)Fix64.FromRaw(QuatOffsetW));
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				QuatOffsetX = ((Fix64)((decimal)value.X)).RawValue;
				QuatOffsetY = ((Fix64)((decimal)value.Y)).RawValue;
				QuatOffsetZ = ((Fix64)((decimal)value.Z)).RawValue;
				QuatOffsetW = ((Fix64)((decimal)value.W)).RawValue;
			}
		}
	}
	private long QuatOffsetX;
	[Export]
	private long quatOffsetX {
		get => QuatOffsetX;
		set {
			QuatOffsetX = value;
			QuatOffset = new BEPUutilities.Quaternion(Fix64.FromRaw(QuatOffsetX), Fix64.FromRaw(QuatOffsetY), Fix64.FromRaw(QuatOffsetZ), Fix64.FromRaw(QuatOffsetW));
		}
	}
	private long QuatOffsetY;
	[Export]
	private long quatOffsetY {
		get => QuatOffsetY;
		set {
			QuatOffsetY = value;
			QuatOffset = new BEPUutilities.Quaternion(Fix64.FromRaw(QuatOffsetX), Fix64.FromRaw(QuatOffsetY), Fix64.FromRaw(QuatOffsetZ), Fix64.FromRaw(QuatOffsetW));
		}
	}
	private long QuatOffsetZ;
	[Export]
	private long quatOffsetZ {
		get => QuatOffsetZ;
		set {
			QuatOffsetZ = value;
			QuatOffset = new BEPUutilities.Quaternion(Fix64.FromRaw(QuatOffsetX), Fix64.FromRaw(QuatOffsetY), Fix64.FromRaw(QuatOffsetZ), Fix64.FromRaw(QuatOffsetW));
		}
	}
	private long QuatOffsetW;
	[Export]
	private long quatOffsetW {
		get => QuatOffsetW;
		set {
			QuatOffsetW = value;
			QuatOffset = new BEPUutilities.Quaternion(Fix64.FromRaw(QuatOffsetX), Fix64.FromRaw(QuatOffsetY), Fix64.FromRaw(QuatOffsetZ), Fix64.FromRaw(QuatOffsetW));
		}
	}

	public Entity Body = null;
	public StaticCollidable BodyStatic = null;
	public CharacterControllerCustom Controller;
	public ColShape colShape = null;
	public Godot.Vector3 ColShapeOrigin;  // Saves initial position offset of ColShape
	protected Godot.Quaternion BodyQuat;
	protected Godot.Quaternion ColShapeQuat;  // Saves initial orientation offset of ColShape
	public PhysicsBodyType type = PhysicsBodyType.Unset;

	public PhysicsHandler physicsHandler;


	public override void _Ready()
	{
		physicsHandler = GetNode<PhysicsHandler>("/root/PhysicsHandler");

		if (type == PhysicsBodyType.Unset) {
			GD.Print(Name + " was not set to one of CharacterBody, KinematicBody, RigidBody, StaticBody, or Area as it's inheirited class");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Update();
	}

	public override void _Notification(int what)
	{
		switch (what) {
			case((int)NotificationPredelete):
				// Delete BEPU entity when Godot node is deleted
				Delete();
				break;
			case((int)NotificationTransformChanged):
				// Change initial BEPU position if in editor gizmo is used
				if (Engine.IsEditorHint()) {
					posOffset = Position;
					quatOffset = Quaternion;
				}
				break;
		}
	}

	public override void _EnterTree() {
		if (Engine.IsEditorHint()) {
			SetNotifyTransform(true);
		}
	}

	protected void SetPhysicsBodyType(PhysicsBodyType value)
	{
		colShape = null;

		physicsHandler = (PhysicsHandler) GetNode("/root/PhysicsHandler");  // Set this because it might be called before entering SceneTree
		type = value;

		foreach (Node childNode in GetChildren()) {
			if (childNode is ColShape) {
				colShape = (ColShape)childNode;
			}
		}

		if (colShape == null) {
			GD.Print("PhysicsBody " + '"' + Name + '"' + " does not have a ColShape");
			return;
		}

		ColShapeOrigin = new Godot.Vector3((float)colShape.PosOffset.X, (float)colShape.PosOffset.Y, (float)colShape.PosOffset.Z);
		BodyQuat = new Godot.Quaternion((float)QuatOffset.X, (float)QuatOffset.Y, (float)QuatOffset.Z, (float)QuatOffset.W); 
		ColShapeQuat = new Godot.Quaternion((float)colShape.QuatOffset.X, (float)colShape.QuatOffset.Y, (float)colShape.QuatOffset.Z, (float)colShape.QuatOffset.W);

		BEPUutilities.Quaternion quatInitial = QuatOffset * colShape.QuatOffset;
		BEPUutilities.Vector3 posInitial = PosOffset + (BEPUutilities.Quaternion.Transform(colShape.PosOffset, QuatOffset));

		switch (colShape.Type) {
			// Box Collision
			case ColShape.ColShapeType.ColShapeBox:
				ColShapeBox boxShape = (ColShapeBox) colShape.Shape;

				switch (type) {
					case PhysicsBodyType.CharacterBody:
						GD.Print("CharacterBody may only use ColShapeCylinder for collision");
						break;
					case PhysicsBodyType.KinematicBody:
					case PhysicsBodyType.StaticBody:
					case PhysicsBodyType.Area:
						// Create a new physics sim body and configure its collision rules
						Body = new Box(posInitial, boxShape.Size.X, boxShape.Size.Y, boxShape.Size.Z);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
					case PhysicsBodyType.RigidBody:
						Body = new Box(posInitial, boxShape.Size.X, boxShape.Size.Y, boxShape.Size.Z, Mass);
						BEPUutilities.Quaternion Quat = quatInitial;
						Quat.Normalize();
						Body.orientation = Quat;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
				}
				break;

			// Capsule Collision
			case ColShape.ColShapeType.ColShapeCapsule:
				ColShapeCapsule capsuleShape = (ColShapeCapsule) colShape.Shape;

				switch (type) {
					case PhysicsBodyType.CharacterBody:
						GD.Print("CharacterBody may only use ColShapeCylinder for collision");
						break;
					case PhysicsBodyType.KinematicBody:
					case PhysicsBodyType.StaticBody:
					case PhysicsBodyType.Area:
						// Create a new physics sim body and configure its collision rules
						Body = new Capsule(posInitial, capsuleShape.Height, capsuleShape.Radius);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
					case PhysicsBodyType.RigidBody:
						Body = new Capsule(posInitial, capsuleShape.Height, capsuleShape.Radius, Mass);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
				}
				break;

			// Cylinder Collision
			case ColShape.ColShapeType.ColShapeCylinder:
				ColShapeCylinder cylinderShape = (ColShapeCylinder) colShape.Shape;

				switch (type) {
					case PhysicsBodyType.CharacterBody:
						// Create the physics body based on child CollisionShape if there is one
						Controller = new CharacterControllerCustom();
						Body = Controller.Body;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];

						Body.Position = posInitial;
						Body.orientation = quatInitial;
						Controller.StanceManager.StandingHeight = cylinderShape.Height;
						Controller.BodyRadius = cylinderShape.Radius;
						Body.CollisionInformation.SetOwner(this);
						// Mass must be set in the main CharacterController class script

						if (quatInitial.X != 0 || quatInitial.Y != 0 || quatInitial.Z != 0) {
							GD.Print(Name + "'s rotation was set, but rotating CharacterBody ColShape is not implemented");
						}
						break;
					case PhysicsBodyType.KinematicBody:
					case PhysicsBodyType.StaticBody:
					case PhysicsBodyType.Area:
						// Create a new physics sim body and configure its collision rules
						Body = new Cylinder(posInitial, cylinderShape.Height, cylinderShape.Radius);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
					case PhysicsBodyType.RigidBody:
						Body = new Cylinder(posInitial, cylinderShape.Height, cylinderShape.Radius, Mass);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
				}
				break;
			
			// Sphere Collision
			case ColShape.ColShapeType.ColShapeSphere:
				ColShapeSphere sphereShape = (ColShapeSphere) colShape.Shape;

				switch (type) {
					case PhysicsBodyType.CharacterBody:
						GD.Print("CharacterBody may only use ColShapeCylinder for collision");
						break;
					case PhysicsBodyType.KinematicBody:
					case PhysicsBodyType.StaticBody:
					case PhysicsBodyType.Area:
						// Create a new physics sim body and configure its collision rules
						Body = new Sphere(posInitial, sphereShape.Radius);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
					case PhysicsBodyType.RigidBody:
						Body = new Sphere(posInitial, sphereShape.Radius, Mass);
						Body.orientation = quatInitial;
						Body.CollisionInformation.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						Body.CollisionInformation.SetOwner(this);
						break;
				}
				break;
			
			// StaticMesh Collision
			case ColShape.ColShapeType.ColShapeMesh:
				ColShapeMesh staticMeshShape = (ColShapeMesh) colShape.Shape;

				switch (type) {
					case PhysicsBodyType.StaticBody:
						// Add position offset to mesh vertices
						BEPUutilities.Vector3[] verticesNew = new BEPUutilities.Vector3[staticMeshShape.Vertices.Length];
						for (int i = 0; i < staticMeshShape.Vertices.Length; i++) {
							verticesNew[i] = staticMeshShape.Vertices[i] + posInitial;
						}

						// Create StaticMesh
						BodyStatic = new StaticMesh(verticesNew, staticMeshShape.Indices, new AffineTransform(PosOffset));
						BodyStatic.CollisionRules.Group = physicsHandler.CollisionGroups[CollisionGroup];
						BodyStatic.SetOwner(this);

						if (quatInitial.X != 0 || quatInitial.Y != 0 || quatInitial.Z != 0) {
							GD.Print(Name + "'s rotation was set, but rotating StaticMeshes is not implemented");
						}
						break;
				}
				break;
		}

		// Add physics body to physics space
		if (!Godot.Engine.IsEditorHint()) {
			if (Body != null) {
				physicsHandler.AddBodyToSpace(Body);
			}
			else if (BodyStatic != null) {
				physicsHandler.AddBodyToSpace(BodyStatic);
			}
			else {
				GD.Print("failed to BEPU body for " + '"' + Name + '"');
			}
		}
	}

    private void Update()  // Updates the visual position of this physics body (does not affect position in BEPU space)
	{
		if (!Godot.Engine.IsEditorHint()){
			// This code applies the orientation and position properly and allows rotating both the PhysicsBody and the ColShape
			// Basis
			var newQuat = new Godot.Quaternion((float)Body.orientation.X, (float)Body.orientation.Y, (float)Body.orientation.Z, (float)Body.orientation.W) * ColShapeQuat.Inverse();
			var newBasis = new Godot.Basis(newQuat);

			// Origin
			var newOrigin = new Godot.Vector3((float)Body.Position.X, (float)Body.Position.Y, (float)Body.Position.Z) - (ColShapeOrigin * newQuat.Inverse());

			// Transform
			var newTransform3D = new Godot.Transform3D(newBasis, newOrigin);
			Transform = newTransform3D;
		}
	}

	public BEPUutilities.Vector3 GetPosition()
	{
		return Body.Position;
	}

	public void SetPosition(BEPUutilities.Vector3 pos)
	{
		Body.Position = pos;
	}

	private void Delete() {
		if(IsInsideTree()) {
			physicsHandler = (PhysicsHandler) GetNode("/root/PhysicsHandler");
			physicsHandler.RemoveBodyFromSpace(Body);
			// GD.Print("removed body from scene");
		}
	}
}
