namespace Game.Player.StateMachine;

using Godot;

public partial class FallState : LimboState
{
    private MovementController MC;

    public override void _Enter()
    {
        MC = GetNode<MovementController>("../../MovementController");
    }

    public override void _PhysicsProcess(double delta)
    {
        float inputDir = Input.GetAxis("MoveLeft", "MoveRight");

        MC.ApplyGravity(delta);
        if (Input.IsActionPressed("Jump"))
        {
            MC.QueueJump();
        }

        if (inputDir != 0)
        {
            MC.MoveHorizontal(inputDir, delta);
        }
        else
        {
            MC.ApplyFriction(delta, 0.5f);
        }

        if (MC.Actor.IsOnFloor())
        {
            Dispatch("landed");
        }
    }
}
