using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class State : Node
{
	public bool Stack = false;
	private StateMachineStack stateMachine = null;


	public virtual void _Enter()
	{

	}

	public virtual void _Exit()
	{

	}

	public virtual void _Update()
	{

	}

	public virtual void _OnAnimationFinished(StringName animName)
	{

	}

	protected virtual void ChangeState(string state_name)
	{
		stateMachine.ChangeState(state_name);
	}

	public virtual void SetOwner(Node node)
	{

	}

	public virtual void SetStateMachine(Node node)
	{
		stateMachine = (StateMachineStack)node;
	}
}

