using Godot;
using System;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class DebugUI : Control
{
	Label FPS;
	Label PhysicsBodyCount;

	public override void _Ready()
	{
		FPS = (Label) GetNode("VBoxContainer/FPS");
		PhysicsBodyCount = (Label) GetNode("VBoxContainer/PhysicsBodyCount");
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateFPS();
	}

	public void UpdateFPS()
	{
		FPS.Text = "FPS: " + Godot.Engine.GetFramesPerSecond().ToString();
	}

	public void UpdatePhysicsBodyCount(int count)
	{
		PhysicsBodyCount.Text = "Physics Bodies: " + count.ToString();
	}
}

