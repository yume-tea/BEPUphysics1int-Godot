using Godot;
using System;

using FixMath.NET;
using BEPUutilities;

[Tool]
public partial class ColShapeCylinder : Shape
{
	public Fix64 Height = (Fix64)2;
	[Export]
	private float height {
		get => (float)Fix64.FromRaw(HeightRaw);
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				HeightRaw = ((Fix64)value).RawValue;
				((CylinderMesh)Mesh).Height = height;
			}
		}
	}
	
	public Fix64 Radius = ((Fix64)1 / (Fix64)2);
	[Export]
	private float radius {
		get => (float)Fix64.FromRaw(RadiusRaw);
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				RadiusRaw = ((Fix64)value).RawValue;
				((CylinderMesh)Mesh).TopRadius = radius;
				((CylinderMesh)Mesh).BottomRadius = radius;
			}
		}
	}

	private long HeightRaw = ((Fix64)2).RawValue;
	[Export]
	private long heightRaw {
		get => HeightRaw;
		set {
			HeightRaw = value;
			Height = Fix64.FromRaw(HeightRaw);

			((CylinderMesh)Mesh).Height = height;
		}
	}

	private long RadiusRaw = ((Fix64)1 / (Fix64)2).RawValue;
	[Export]
	private long radiusRaw {
		get => RadiusRaw;
		set {
			RadiusRaw = value;
			Radius = Fix64.FromRaw(RadiusRaw);

			((CylinderMesh)Mesh).TopRadius = radius;
			((CylinderMesh)Mesh).BottomRadius = radius;
		}
	}
}
