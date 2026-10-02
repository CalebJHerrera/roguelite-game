namespace Game.Player;

using Godot;

public partial class Camera2d : Camera2D
{
    private float SmoothFactor { get; set; }
    private CharacterBody2D Actor { get; set; }

    public override void _EnterTree()
    {
        Actor = GetParent<CharacterBody2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        SmoothFactor = Mathf.Max(5.0f, Actor.Velocity.Y / 2.0f);
    }
}
