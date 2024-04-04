using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class StatePlayer : State
{
	protected Player owner = null;

	public override void _Enter()
	{

	}

	public override void _Exit()
	{

	}

	public override void _Update()
	{

	}

	public override void _OnAnimationFinished(StringName animName)
	{

	}

	public override void SetOwner(Node node)
	{
		owner = (Player)node;
	}
}

