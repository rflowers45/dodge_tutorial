using Godot;
using System;

public partial class Main : Node
{
#pragma warning disable 649
    //We assign this in the editor so we don't need the warning about not being assigned
    [Export]
    public PackedScene MobScene;
#pragma warning restore 649
    public int Score;

    public override void _Ready()
    {
        //GD.Randomize();
        NewGame();
    }

    public void game_over()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
    }

    public void NewGame()
    {
        Score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
    }

    public void _on_score_timer_timeout()
    {
        Score++;
    }

    public void _on_start_timer_timeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    public void _on_mob_timer_timeout()
    {
        var mob = (mob)MobScene.Instantiate();

        // Choose a random location on Path2D.
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Progress = GD.Randi();//mobSpawnLocation.HOffset = GD.RandRange(0, 480);
        

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mob.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity.
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);

    }

}
