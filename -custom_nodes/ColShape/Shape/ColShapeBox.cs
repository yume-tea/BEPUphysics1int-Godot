using Godot;
using System;

using FixMath.NET;
using BEPUutilities;

[Tool]
public partial class ColShapeBox : Shape
{
	public BEPUutilities.Vector3 Size = new BEPUutilities.Vector3(1,1,1);
	[Export]
	private Godot.Vector3 size {
		get => new Godot.Vector3((float)Fix64.FromRaw(SizeX), (float)Fix64.FromRaw(SizeY), (float)Fix64.FromRaw(SizeZ));
		set {
			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				SizeX = ((Fix64)value.X).RawValue;
				SizeY = ((Fix64)value.Y).RawValue;
				SizeZ = ((Fix64)value.Z).RawValue;
				((BoxMesh)Mesh).Size = size;
			}
		}
	}

	private long SizeX = ((Fix64)1).RawValue;
	[Export]
	private long sizeX {
		get => SizeX;
		set {
			SizeX = value;
			Size = new BEPUutilities.Vector3(Fix64.FromRaw(SizeX), Fix64.FromRaw(SizeY), Fix64.FromRaw(SizeZ));

			// Resize the visual of this body's mesh
			((BoxMesh)Mesh).Size = size;
		}
	}
	private long SizeY = ((Fix64)1).RawValue;
	[Export]
	private long sizeY {
		get => SizeY;
		set {
			SizeY = value;
			Size = new BEPUutilities.Vector3(Fix64.FromRaw(SizeX), Fix64.FromRaw(SizeY), Fix64.FromRaw(SizeZ));
						
			// Resize the visual of this body's mesh
			((BoxMesh)Mesh).Size = size;
		}
	}
	private long SizeZ = ((Fix64)1).RawValue;
	[Export]
	private long sizeZ {
		get => SizeZ;
		set {
			SizeZ = value;
			Size = new BEPUutilities.Vector3(Fix64.FromRaw(SizeX), Fix64.FromRaw(SizeY), Fix64.FromRaw(SizeZ));
						
			// Resize the visual of this body's mesh
			((BoxMesh)Mesh).Size = size;
		}
	}
}
