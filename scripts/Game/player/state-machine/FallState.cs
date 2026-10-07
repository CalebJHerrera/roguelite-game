namespace Game.Player.StateMachine;

using Godot;

public partial class FallState : LimboState
{
    private MovementController MC;

    public override void _Enter()
    {
        MC = Blackboard.GetVar("MovementController").As<MovementController>();
    }

    public override void _PhysicsProcess(double delta)
    {
        float inputDir = Input.GetAxis("MoveLeft", "MoveRight");

        MC.ApplyGravity(delta);

        if (inputDir != 0)
        {
            MC.MoveHorizontal(inputDir, delta);
        }
        else
        {
            MC.ApplyFriction(delta, 0.5f);
        }

        MC.Move();

        if (MC.Actor.IsOnFloor())
        {
            Dispatch("landed");
        }
        else if (Input.IsActionJustPressed("Jump"))
        {
            Dispatch("air_jump_used");
        }
    }
}
