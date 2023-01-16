using Godot;
using System;

public partial class mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2d");
		animSprite.Playing = true;
		string[] mobTypes = animSprite.Frames.GetAnimationNames();
		animSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
	}
	public void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		QueueFree();
	}
}
