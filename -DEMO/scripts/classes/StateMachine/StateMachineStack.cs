using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


public partial class StateMachineStack : Node
{
	private Node initialState = null;
	[Export]
	private Node InitialState {
		get => initialState;
		set {
			initialState = value;
		}
	}
	
	List<State> currentState = new List<State>();
	Dictionary<string, State> states = new Dictionary<string, State>();
	// How should i get and store the states of a character?


	public override void _Ready()
	{
		Initialize();
	}

	public override void _PhysicsProcess(double delta)
	{
		List<State> statesToProcess = new List<State>(currentState);

		if (currentState.Count != 0) {
			foreach (State state in statesToProcess) {
				state._Update();
			}
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

	public void ChangeState(string stateName)
	{
		if (!states.ContainsKey(stateName)) {
			GD.Print("state " + stateName + " not found in " + GetParent().Name + "'s states");
			return;
		}

		State stateNext = states[stateName];

		
		if (!stateNext.Stack) {
			if (currentState.Count != 0) {  // Only attempt to exit state if there is a current state atm
				currentState[0]._Exit();
				currentState[0] = stateNext;
			}
			else {
				currentState.Add(stateNext);
			}

			currentState[0]._Enter();
			// GD.Print(stateName);
		}
		else if (stateNext.Stack && currentState.Count != 0) {
			// Make sure that state is not already in the stack
			if (currentState.Contains(stateNext)) {
				return;
			}
			currentState.Add(stateNext);

			stateNext._Enter();
		}



		
	}

	// For exiting states that stack
	public void ExitState(string stateName)
	{
		// Check that the stateName corresponds to a valid state
		if (!states.ContainsKey(stateName)) {
			GD.Print("state " + stateName + " not found in " + GetParent().Name + "'s states");
			return;
		}

		State stateExit = states[stateName];

		// Check that this state is even part of the current state, exit this state if it is
		if (!currentState.Contains(stateExit)) {
			GD.Print("attempted to exit stacking state " + stateExit.Name + " but state is not in currentState for " + GetParent().Name);
		}
		else {
			stateExit._Exit();

			currentState.Remove(stateExit);
		}

	}
}

