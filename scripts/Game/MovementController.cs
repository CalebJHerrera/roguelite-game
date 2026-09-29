namespace Game;

using Godot;

// In case I forget, the player leaf states / AI BT node call the functions inside here. No logic is to be handled in this file, logic is actually in the states/tree nodes.
// The only stuff in here is the modification of the velocities and some other random weird platformer stuff.
[GlobalClass]
public partial class MovementController : Node
{
    [ExportGroup("Actor")]
    [Export] CharacterBody2D actor;

    [ExportGroup("Physics")]
    [Export] int baseSpeed = 200;
    [Export] int jumpVelocity = 400;
    [Export] int dashVelocity = 300;
    [Export] float acceleration = 0.1f;
    [Export] float friction = 0.1f;
    [Export] float speedMultiplier = 1.0f;

    [ExportGroup("Jump Timers")]
    [Export] float coyoteTimer = 0.15f;
    [Export] float bufferTimer = 0.15f;

    public void MoveHorizontal(int xDir, double delta)
    {
    }

    public void ApplyGravity(double delta)
    {
    }

    public void ApplyFriction(double delta)
    {
    }

    public void Dash()
    {
    }

    public void Jump()
    {
    }

    public void CheckJump()
    {
    }

    public void UpdateTimers()
    {
    }

    public void ClearTimers()
    {
    }

    public void Move()
    {
        actor.MoveAndSlide();
    }
}
