using Godot;

namespace RogueliteGame.scripts.game.player.state_machine;

public partial class FallState : LimboState
{
    private MovementController _moveController;

    public override void _Enter()
    {
        _moveController = Blackboard.GetVar("MovementController").As<MovementController>();
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDir = Input.GetAxis("MoveLeft", "MoveRight");

        _moveController.ApplyGravity(delta);

        if (inputDir != 0)
        {
            _moveController.MoveHorizontal(inputDir, delta);
        }
        else
        {
            _moveController.ApplyFriction(delta, 0.5f);
        }

        _moveController.Move();

        if (_moveController.Actor.IsOnFloor())
        {
            Dispatch("landed");
        }
        else if (Input.IsActionJustPressed("Jump"))
        {
            Dispatch("air_jump_used");
        }
        else if (Input.IsActionPressed("Dash"))
        {
            Dispatch("dash_pressed");
        }
    }
}
