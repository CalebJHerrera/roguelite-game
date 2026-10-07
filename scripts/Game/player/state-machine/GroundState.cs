namespace Game.Player.StateMachine;

using Godot;

public partial class GroundState : LimboState
{
    private MovementController MC { get; set; }

    public override void _Enter()
    {
        MC = Blackboard.GetVar("MovementController").As<MovementController>();
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
            MC.ApplyFriction(delta);
        }

        MC.ApplyGravity(delta);
        MC.Move();

        if (Input.IsActionPressed("Jump"))
        {
            Dispatch("jump_pressed");
        }
        else if (Input.IsActionPressed("Dash"))
        {
            Dispatch("dash_pressed");
        }
    }
}
