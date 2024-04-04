using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;

public partial class Util : Node3D
{
	// Convertes degrees to radians
	public static Fix64 DegToRad(Fix64 degrees)
	{
		return degrees * (Fix64)0.0174533m;
	}

	// Converts a float value to a Fix64 raw value (Used for animation conversion EditorScript)
	public static long Float64ToFix64Raw(double value)
	{
		long convertedValue = ((Fix64)((decimal)value)).RawValue;

		return convertedValue;
	}
}

