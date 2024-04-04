using Godot;
using System;

using FixMath.NET;
using BEPUutilities;


public partial class Shape : MeshInstance3D
{
    public override void _Notification(int what)
	{
		switch (what) {
			case((int)NotificationTransformChanged):
				// Prevent accidental clicks from moving the shape
				if (Engine.IsEditorHint()) {
					Position = new Godot.Vector3();
					Rotation = new Godot.Vector3();
				}
				break;
		}
	}

	public override void _EnterTree() {
        if (Engine.IsEditorHint()) {
		    SetNotifyTransform(true);
        }
	}
}
