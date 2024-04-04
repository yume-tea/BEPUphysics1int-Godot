using Godot;
using System;

using FixMath.NET;
using BEPUutilities;

[Tool]
public partial class ColShapeSphere : Shape
{
	public Fix64 Radius = ((Fix64)1 / (Fix64)2);
	[Export]
	private float radius {
		get => (float)Fix64.FromRaw(RadiusRaw);
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				RadiusRaw = ((Fix64)value).RawValue;
			}

			((SphereMesh)Mesh).Radius = (float)radius;
			((SphereMesh)Mesh).Height = (float)radius * 2;
		}
	}

	private long RadiusRaw = ((Fix64)1 / (Fix64)2).RawValue;
	[Export]
	private long radiusRaw {
		get => RadiusRaw;
		set {
			RadiusRaw = value;
			Radius = Fix64.FromRaw(RadiusRaw);
			((SphereMesh)Mesh).Radius = (float)radius;
			((SphereMesh)Mesh).Height = (float)radius * 2;
		}
	}
}
