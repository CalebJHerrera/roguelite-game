namespace Game.Player.StateMachine;

using Godot;

public partial class JumpState : LimboState
{
    private MovementController MC;

    public override void _Enter()
    {
        MC = Blackboard.GetVar("MovementController").As<MovementController>();
        MC.QueueJump();
    }

    public override void _PhysicsProcess(double delta)
    {
        float inputDir = Input.GetAxis("MoveLeft", "MoveRight");

        if (inputDir != 0)
        {
            MC.MoveHorizontal(inputDir, delta);
        }
        else
        {
            MC.ApplyFriction(delta, 0.1f);
        }

        MC.ApplyGravity(delta);
        MC.Move(); // Don't change velocities after this.

        if (MC.Actor.Velocity.Y == 0)
        {
            Dispatch("apex_reached");
        }
        else if (Input.IsActionPressed("Dash"))
        {
            Dispatch("dash_pressed");
        }
    }
}
