using Godot;
using System;
using System.Collections.Generic; // Lists

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.CollisionRuleManagement;


public partial class PhysicsHandler : Node
{
	public bool run = false;
	public Space space = null;
	public BEPUutilities.Vector3 Gravity = new BEPUutilities.Vector3(0, (Fix64)(-9.81), 0);
	
	// Collision Groups
	public enum CollisionGroupLayer {
		DefaultGroup,
		PlayerGroup,
		EnemyGroup,
		ProjectilePlayerGroup,
		ProjectileEnemyGroup,
		AreaGroup,
	}

	public static CollisionGroup DefaultGroup = new CollisionGroup();
	public static CollisionGroup PlayerGroup = new CollisionGroup();
	public static CollisionGroup EnemyGroup = new CollisionGroup();
	public static CollisionGroup ProjectilePlayerGroup = new CollisionGroup();
	public static CollisionGroup ProjectileEnemyGroup = new CollisionGroup();
	public static CollisionGroup AreaGroup = new CollisionGroup();

	public Dictionary<CollisionGroupLayer, CollisionGroup> CollisionGroups = new Dictionary<CollisionGroupLayer, CollisionGroup>();


	public override void _Ready()
	{
		InitializeSpace();

		// Configure collision groups
		/*
		Here, we can define how objects of one CollisionGroup interact with another
		Refer to this page for possible CollisionRules:
		https://github.com/bepu/bepuphysics1/blob/master/Documentation/CollisionRules.md#2--user-defined-collision-rules

		Would be nice to have this work like Godot's collision layers in the future...
		*/
		// // AreaGroup
		CollisionGroupPair groupPairDefaultArea = new CollisionGroupPair(DefaultGroup, AreaGroup);
        CollisionRules.CollisionGroupRules.Add(groupPairDefaultArea, CollisionRule.NoSolver);  // Default/Area collisions

		CollisionGroupPair groupPairPlayerArea = new CollisionGroupPair(PlayerGroup, AreaGroup);
		CollisionRules.CollisionGroupRules.Add(groupPairPlayerArea, CollisionRule.NoSolver);  //Player/Area collisions

		CollisionGroupPair groupPairEnemyArea = new CollisionGroupPair(EnemyGroup, AreaGroup);
		CollisionRules.CollisionGroupRules.Add(groupPairEnemyArea, CollisionRule.NoSolver);  //Enemy/Area collisions

		CollisionGroupPair groupPairProjectilePlayerArea = new CollisionGroupPair(ProjectilePlayerGroup, AreaGroup);
		CollisionRules.CollisionGroupRules.Add(groupPairProjectilePlayerArea, CollisionRule.NoSolver);  //ProjectilePlayer/Area collisions

		CollisionGroupPair groupPairProjectileEnemyArea = new CollisionGroupPair(ProjectileEnemyGroup, AreaGroup);
		CollisionRules.CollisionGroupRules.Add(groupPairProjectileEnemyArea, CollisionRule.NoSolver);  //ProjectileEnemy/Area collisions

		// // ProjectileEnemyGroup
		CollisionGroupPair groupPairEnemyProjectilePlayer = new CollisionGroupPair(PlayerGroup, ProjectileEnemyGroup);
		CollisionRules.CollisionGroupRules.Add(groupPairEnemyProjectilePlayer, CollisionRule.NoSolver);

		// Initialize CollisionGroups dictionary
		CollisionGroups.Add(CollisionGroupLayer.DefaultGroup, DefaultGroup);
		CollisionGroups.Add(CollisionGroupLayer.PlayerGroup, PlayerGroup);
		CollisionGroups.Add(CollisionGroupLayer.EnemyGroup, EnemyGroup);
		CollisionGroups.Add(CollisionGroupLayer.ProjectilePlayerGroup, ProjectilePlayerGroup);
		CollisionGroups.Add(CollisionGroupLayer.ProjectileEnemyGroup, ProjectileEnemyGroup);
		CollisionGroups.Add(CollisionGroupLayer.AreaGroup, AreaGroup);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (run) {
			space.Update();
		}
	}
	
	public void AddBodyToSpace(ISpaceObject entity)
	{
		space.Add(entity);
	}

	public void RemoveBodyFromSpace(ISpaceObject entity)
	{
		space.Remove(entity);
	}
	
	public void SetRun(bool flag)
	{
		run = flag;
	}
	
	public void InitializeSpace()
	{
		space = null;

		// Create a new physics space
		space = new Space();
		
		// Set gravity value
		space.ForceUpdater.Gravity = Gravity;
	}

	public void ReloadScene()
	{
		HaltSim();
		InitializeSpace();
		GetTree().ReloadCurrentScene();
	}

	public void StartSim()
	{
		run = true;
	}

	public void HaltSim()
	{
		run = false;
	}
}

