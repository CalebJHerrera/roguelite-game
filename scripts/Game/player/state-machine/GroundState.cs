using Godot;

namespace RogueliteGame.scripts.game.player.state_machine;

public partial class GroundState : LimboState
{
    private MovementController _moveController;

    public override void _Enter()
    {
        _moveController = Blackboard.GetVar("MovementController").As<MovementController>();
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
            _moveController.ApplyFriction(delta);
        }

        _moveController.ApplyGravity(delta);
        _moveController.Move();

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
