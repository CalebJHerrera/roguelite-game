using Godot;

namespace RogueliteGame.scripts.game.player.state_machine;

public partial class JumpState : LimboState
{
    private MovementController _moveController;

    public override void _Enter()
    {
        _moveController = Blackboard.GetVar("MovementController").As<MovementController>();
        _moveController.QueueJump();
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDir = Input.GetAxis("MoveLeft", "MoveRight");

        if (inputDir != 0)
        {
            _moveController.MoveHorizontal(inputDir, delta);
        }
        else
        {
            _moveController.ApplyFriction(delta, 0.1f);
        }

        _moveController.ApplyGravity(delta);
        _moveController.Move(); // Don't change velocities after this.

        if (_moveController.Actor.Velocity.Y <= 0)
        {
            Dispatch("apex_reached");
        }
        else if (Input.IsActionPressed("Dash"))
        {
            Dispatch("dash_pressed");
        }
        else if (Input.IsActionJustPressed("Jump"))
        {
            Dispatch("air_jump_used");
        }
    }
}
