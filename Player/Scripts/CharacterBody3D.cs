using Godot;
using System;
using System.Diagnostics;

public partial class CharacterBody3D : Godot.CharacterBody3D
{
	public const float Speed = 10;
	public const float JumpVelocity = 10f;


	private float _verticalRecoil ;
	private float _horizontalRecoil;
	private bool _isShooting;


    [Export] private Camera3D _camera;


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = 20;

    public override void _UnhandledInput(InputEvent inputEvent)
    {

        if(inputEvent is InputEventMouseMotion mouseMotion)
		{
			RotateY(-mouseMotion.Relative.X * .005f);
            _camera.RotateX(-mouseMotion.Relative.Y * .005f );

			var camRotation = _camera.Rotation;	

			camRotation.X = Math.Clamp(camRotation.X,(float) -Mathf.Pi/2, (float)Mathf.Pi/2);

			_camera.Rotation = camRotation;
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
		AddCameraRecoil((float)delta);
    }



    private void AddCameraRecoil(float delta)
	{
        if (_verticalRecoil > 0)
        {
            var tmpTranslation = _camera.Rotation.Lerp(_camera.Rotation + new Vector3(_verticalRecoil, 0, 0 ), 0.5f * delta );
            tmpTranslation.X = Math.Clamp(tmpTranslation.X, (float)-Mathf.Pi / 2, (float)Mathf.Pi / 2);
            _camera.Rotation = tmpTranslation;
        }
    }

	public void SetVerticalRecoil(float verticalRecoil , float horizontalRecoil)
	{
        _verticalRecoil = verticalRecoil;
		_horizontalRecoil = horizontalRecoil;

        if (verticalRecoil == 0)
		{
			_verticalRecoil = 0;

		}
    }
}
