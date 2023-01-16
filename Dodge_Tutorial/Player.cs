using Godot;
using System;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

	[Export]
	public int Speed = 350;
	public Vector2 ScreenSize; //Size of game window
    private Vector2 deceleration = new Vector2(0, 0);
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
        Hide();
	}

    public void Start(Vector2 pos)
    {
        Position= pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2d").Disabled = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var velocity = getInput();      
        Position = getPosition(velocity, delta);
        animateSprite(velocity);
    }//End _Process

    public void _on_player_body_entered(PhysicsBody2D body)
    {
        Hide();
        EmitSignal(nameof(Hit));
        GetNode<CollisionShape2D>("CollisionShape2d").SetDeferred("disabled", true);
    }

    //**********************************************************************************HELPER FUNCTIONS

    /// <summary>
    /// This processes character velocity and deceleration. Note that the deceleration vector can be commented out to prevent gradual slowdown.
    /// </summary>
    /// <returns>Velocity vector</returns>
    private Vector2 getInput()
    {
        
        var velocity = Vector2.Zero; // The player's movement vector
        if (Input.IsActionPressed("move_right"))
        {
            velocity.x += 1;
            deceleration.x = 1; //Can be removed for instant character stop
            
        }
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x += -1;
            deceleration.x = -1; //Can be removed for instant character stop
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.y += 1;
            deceleration.y = 1; //Can be removed for instant character stop
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.y += -1;
            deceleration.y = -1; //Can be removed for instant character stop
        }

        velocity = Vector2.Zero; //Setting velocity to zero for the deceleration event to take place (below)
        //Deceleration
        if (velocity.Length() == 0)
        {
            if (deceleration.Length() > .1)
            {
                deceleration *= .95f; //Can be removed for instant character stop
                velocity = (velocity + deceleration) * Speed;
                
                
            }
        }
        return velocity;
    }

    /// <summary>
    /// This assigns the position value of the character and also clamps the position preventing the player from going out of bounds
    /// </summary>
    /// <param name="vel">Velocity vector</param>
    /// <param name="delta">delta vector (from _Process class)</param>
    /// <returns>Returns the Position vector</returns>
    private Vector2 getPosition(Vector2 vel, double delta)
    {
        Position += vel * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
        );
        return Position;
    }

    /// <summary>
    /// Animates the sprite based on which button was just pressed. Note: The velocity argument is from a previous iteration
    /// </summary>
    /// <param name="velocity"></param>
    private void animateSprite(Vector2 velocity)
    {
        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2d");

        if (velocity.Length() > .1f)
        {
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        if (Input.IsActionJustPressed("move_right"))
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipH = false;
        }
        if (Input.IsActionJustPressed("move_left"))
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipH = true;
        }
        if (Input.IsActionJustPressed("move_up"))
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = false;
        }
        if (Input.IsActionJustPressed("move_down"))
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = true;
        }

        //if (Math.Abs(velocity.x) > 75)
        //{
        //    animatedSprite.Animation = "walk";
        //    animatedSprite.FlipV = false;
        //    animatedSprite.FlipH = velocity.x < 0;
        //    GD.Print("X value: " + velocity.x);
        //}
        //else if (Math.Abs(velocity.y) > 5)
        //{
        //    animatedSprite.Animation = "up";
        //    animatedSprite.FlipV = velocity.y > 0;
        //    GD.Print("Y value: " + velocity.y);
        //}
    }

}