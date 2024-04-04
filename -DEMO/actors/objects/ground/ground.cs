using Godot;
using System;


[Tool]
public partial class ground : StaticBody
{
	public override void _Ready()
	{
		if (Engine.IsEditorHint()) return;
		
		base._Ready();
	}
}
