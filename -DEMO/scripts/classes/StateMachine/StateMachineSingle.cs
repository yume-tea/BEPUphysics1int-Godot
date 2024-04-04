using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class StateMachineSingle : Node
{
	private Node initialState = null;
	[Export]
	private Node InitialState {
		get => initialState;
		set {
			initialState = value;
		}
	}
	

	State currentState = null;
	Dictionary<string, State> states = new Dictionary<string, State>();
	// How should i get and store the states of a character?


	public override void _Ready()
	{
		Initialize();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (currentState != null) {
			currentState._Update();
		}
	}

	public void Initialize()
	{
		// Get child nodes of the state machine and store them in the local states dict
		foreach (Node child in GetChildren()) {
			if (child is State) {
				State state = (State)child;
				states.Add(child.Name, state);
				state.SetOwner(GetParent());  // Set the owner var of this state
				state.SetStateMachine(this);  // Set reference to this state machine so ChangeState can be called
			}
		}

		// Change state to initial state or the fist state in the states dict
		if (states.Keys.Count > 0) {
			if (initialState != null) {
				ChangeState(initialState.Name);
			}
			else {
				GD.Print("InitialState not set for " + GetParent().Name);
			}
		}
		else {
			GD.Print("states dict for " + GetParent().Name + " was empty on Initialize()");
		}
	}

	public void ChangeState(string state_name)
	{
		if (!states.ContainsKey(state_name)) {
			GD.Print("state " + state_name + " not found in " + GetParent().Name + "'s states");
			return;
		}

		if (currentState != null) {
			currentState._Exit();
		}

		currentState = states[state_name];

		currentState._Enter();
	}
}

