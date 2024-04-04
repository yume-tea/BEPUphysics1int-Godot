using Godot;
using System;

using FixMath.NET;
using BEPUutilities;


[Tool]
public partial class ColShape : Node3D
{
	string materialPhysicsBodyPath = "res://-custom_nodes/ColShape/Shape/materials_collision/debug_physicsbody.material";

	public enum ColShapeType
	{
		None,
		ColShapeBox,
		ColShapeCapsule,
		ColShapeCylinder,
		ColShapeSphere,
		ColShapeMesh,
	}

	public ColShapeType Type;
	[Export]
	private ColShapeType type {
		get => Type;
		set {
			Type = value;

			// Avoid removing shape if new type and shape type already match (avoid export value resetting when code is changed)
			if (HasNode("Shape")) {
				Shape node = GetNode<Shape>("Shape");

				switch (Type) {
					case ColShapeType.None:
						break;
					case ColShapeType.ColShapeBox:
						if (node is ColShapeBox) {
							return;
						}
						break;
					case ColShapeType.ColShapeCapsule:
						if (node is ColShapeCapsule) {
							return;
						}
						break;
					case ColShapeType.ColShapeCylinder:
						if (node is ColShapeCylinder) {
							return;
						}
						break;
					case ColShapeType.ColShapeSphere:
						if (node is ColShapeSphere) {
							return;
						}
						break;
					case ColShapeType.ColShapeMesh:
						if (node is ColShapeMesh) {
							return;
						}
						break;
				}
			}

			foreach (Node node in GetChildren()) {
				if (node is Shape) {
					RemoveChild(node);
				}
			}

			switch (Type) {
				case ColShapeType.None:
					Shape = null;
					return;
					break;
				case ColShapeType.ColShapeBox:
					Shape = new ColShapeBox();
					Shape.Mesh = new BoxMesh();
					break;
				case ColShapeType.ColShapeCapsule:
					Shape = new ColShapeCapsule();
					Shape.Mesh = new CapsuleMesh();
					break;
				case ColShapeType.ColShapeCylinder:
					Shape = new ColShapeCylinder();
					Shape.Mesh = new CylinderMesh();
					break;
				case ColShapeType.ColShapeSphere:
					Shape = new ColShapeSphere();
					Shape.Mesh = new SphereMesh();
					break;
				case ColShapeType.ColShapeMesh:
					Shape = new ColShapeMesh();
					break;
			}

			// Apply default material to shape
			if (Shape.Mesh != null) {
				if (FileAccess.FileExists(materialPhysicsBodyPath)) {
					Material materialCollision = GD.Load<Material>(materialPhysicsBodyPath);
					Shape.Mesh.SurfaceSetMaterial(0, materialCollision);
				}
			}

			Shape.Name = "Shape";
			AddChild(Shape);
			Shape.Owner = this;
		}
	}

	public Shape Shape;
	[Export]
	private Shape shape {
		get => Shape;
		set {
			Shape = value;
			return;
		}
	}

	private bool _VisibleCollision = true;
	[Export]
	public bool VisibleCollision {
		get => _VisibleCollision;
		set {
			_VisibleCollision = value;

			if (HasNode("Shape")) {
				MeshInstance3D meshInstance = GetNode<MeshInstance3D>("Shape");
				if (meshInstance.Mesh != null) {
					meshInstance.Visible = value;
				}
			}

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

	public override void _Ready() {
		GetParent().SetEditableInstance(this, true);

		if (!Engine.IsEditorHint()) {
			if (Shape == null) {
				GD.Print("ColShape for " + '"' + GetParent().Name + '"' + " does not have a Shape");
			}
		}
	}

	public override void _Notification(int what)
	{
		switch (what) {
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
		SetNotifyTransform(true);  // Enables nofifications being recieved for this nodes' transform being changed
	}
}
