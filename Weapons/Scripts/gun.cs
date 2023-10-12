using Godot;
using System;
using System.Buffers.Text;
using System.Transactions;

public partial class gun : Node3D
{
    private Vector3 initialPosition;
    private Vector3 initalRotation;

    private Vector3 initalCamPosition;
    [Export]private float recoilDistance = 10.0f;
    [Export] private float _cameraRecoilVertical = 10.0f;
    [Export] private float _cameraRecoilHorizontal = 10.0f;
    [Export] private Node3D _camera;

    [Export] private float _verticalRecoil;

    [Export] private float _maxXRecoil;
    [Export] private float _maxYRecoil;
    [Export] private float _maxZRecoil;

    [Export] CharacterBody3D _char;

    [Export] Vector3 _aimDownSightOffset;
    [Export] RayCast3D _rayCast;
    [Export] PackedScene _decal;
    [Export] PackedScene _muzzleFlash;
    [Export] Node3D _muzzlePoint;
    [Export] AudioStream _audioFile;
    [Export] PackedScene _audioInstance;
    [Export] PackedScene _bullet;

    [Export] float _maxShootTimer;
    private float _shootTimer;
    

    private bool _isAdsing;

    public override void _Ready()
    {
        initialPosition = Position;
        initalRotation = Rotation;
        initalCamPosition = _camera.Rotation;

    }

    public override void _Process(double delta)
    {
        if (_shootTimer > 0) _shootTimer -= (float)delta;


        var vertiRecoil = 0f;
        var horiRecoil = 0f;



        if (Input.IsActionPressed("ads"))
        {
            AimDownSight();
            _isAdsing = true;
        }
        else
        {
            Hip();
            _isAdsing = false;
        }


        // Your shooting logic here
        if (Input.IsActionPressed("fire") && _shootTimer <= 0)
        {
            var randomY = (float)GD.RandRange(-_maxYRecoil, _maxYRecoil);
            var randomZ = (float)GD.RandRange(-_maxZRecoil, _maxZRecoil);
            var vertRecoil = (float)GD.RandRange(-_verticalRecoil, 0);
            Tween tween = CreateTween().SetParallel(true);
            _shootTimer = _maxShootTimer;

            if (_isAdsing)
            {
                tween.TweenProperty(this, "position", _aimDownSightOffset - new Vector3(0, 0, -recoilDistance), .2f);
                tween.TweenProperty(this, "rotation", Rotation - new Vector3(-_maxXRecoil, randomY, randomZ), 0.05f);
            }
            else
            {
                tween.TweenProperty(this, "position", initialPosition - new Vector3(0, 0, -recoilDistance), 0.2f);
                tween.TweenProperty(this, "rotation", Rotation -  new Vector3(-_maxXRecoil, randomY, randomZ), 0.05f);
            }

            // Apply recoil using a Tween


            var muzzleFlashInstance = _muzzleFlash.Instantiate() as GpuParticles3D;
            _muzzlePoint.AddChild(muzzleFlashInstance);
            muzzleFlashInstance.Emitting = true;
            var audio = _audioInstance.Instantiate() as AudioStreamPlayer;
            GetTree().Root.AddChild(audio);
            audio.Stream = _audioFile;
            audio.Play();

            var bulletInstance = _bullet.Instantiate() as Node3D;

            GetTree().Root.AddChild(bulletInstance);
            bulletInstance.Position = _muzzlePoint.GlobalPosition;

            var tmpBasisis = bulletInstance.Transform;

            tmpBasisis.Basis = _muzzlePoint.GlobalTransform.Basis;

            bulletInstance.Transform = tmpBasisis;









            vertiRecoil = _cameraRecoilVertical;
            horiRecoil = _cameraRecoilHorizontal;

            var target = _rayCast.GetCollider();
            if (target != null && target is Node3D targetNode)
            {
                var decal = _decal.Instantiate() as Node3D;

                GetTree().Root.AddChild(decal);
                decal.GlobalPosition = _rayCast.GetCollisionPoint();

                var surface_dir_up = new Vector3(0, 1, 0);
                var surface_dir_down = new Vector3(0, -1, 0);

                if (_rayCast.GetCollisionNormal() == surface_dir_up)
                {
                    decal.LookAt(_rayCast.GetCollisionPoint() + _rayCast.GetCollisionNormal(), Vector3.Right);
                }
                else if (_rayCast.GetCollisionNormal() == surface_dir_down)
                {
                    decal.LookAt(_rayCast.GetCollisionPoint() + _rayCast.GetCollisionNormal(), Vector3.Right);
                }
                else
                {
                    decal.LookAt(_rayCast.GetCollisionPoint() + _rayCast.GetCollisionNormal(), Vector3.Down);
                }


             


            }




        }

        if (Input.IsActionJustReleased("fire") || _shootTimer > 0)
        {
            Tween tween = CreateTween().SetParallel(true);




            // Reset position after recoil
            if (_isAdsing)
            {
                tween.TweenProperty(this, "position", _aimDownSightOffset, 0.2f);
            }
            else
            {
                tween.TweenProperty(this, "position", initialPosition, 0.2f);
            }

       
            tween.TweenProperty(this, "rotation", Vector3.Zero, 0.4f);



        }

        if (vertiRecoil > 0) vertiRecoil -= (float)delta;
        if (horiRecoil > 0) horiRecoil -= (float)delta;

        _char.SetVerticalRecoil(vertiRecoil, horiRecoil);
    }

    public override void _PhysicsProcess(double delta)
    {




        
    }

    public void AimDownSight()
    {
        var tmpPos = Position.Lerp( _aimDownSightOffset,0.5f);
        Position = tmpPos;
    }
    public void Hip()
    {
        var tmpPos = Position.Lerp(initialPosition, 0.5f);
        Position = tmpPos;
    }
}








