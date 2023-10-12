using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayerFps.Weapons.Scripts
{
    public partial class ShootComponent : Node3D
    {

        [Export] float _maxShootTimer; //Adjust to have a higher fire rate (if == 0 sound will bug)
        private float _shootTimer;


        [Export] RayCast3D _rayCast;
        [Export] PackedScene _decal;
        [Export] PackedScene _muzzleFlash;
        [Export] Node3D _muzzlePoint;
        [Export] AudioStream _audioFile;
        [Export] PackedScene _audioInstance;
        [Export] PackedScene _bullet;


        [Export] GunController _gunController;

        public override void _Process(double delta)
        {

            if (_shootTimer > 0) _shootTimer -= (float)delta;

            if (Input.IsActionPressed("fire") && _shootTimer <= 0)
            {
                
                SpawnMuzzleFlash();
                SpawnAudioSource();
                SpawnBullet();
                SpawnBulletHoles();
                _gunController.RecoilComponent.AddRecoil();
                   _shootTimer = _maxShootTimer;
            }

            if (Input.IsActionJustReleased("fire") || _shootTimer > 0)
            {
                _gunController.RecoilComponent.ResetRecoil();
            }
        }



        private void SpawnMuzzleFlash()
        {
            var muzzleFlashInstance = _muzzleFlash.Instantiate() as GpuParticles3D;
            _muzzlePoint.AddChild(muzzleFlashInstance);
            muzzleFlashInstance.Emitting = true;
        }

        private void SpawnAudioSource()
        {
            var audio = _audioInstance.Instantiate() as AudioStreamPlayer;
            GetTree().Root.AddChild(audio);
            audio.Stream = _audioFile;
            audio.Play();
        }

        private void SpawnBullet()
        {
            var bulletInstance = _bullet.Instantiate() as Node3D;

            GetTree().Root.AddChild(bulletInstance);
            bulletInstance.Position = _muzzlePoint.GlobalPosition;

            var tmpBasisis = bulletInstance.Transform;
            tmpBasisis.Basis = _muzzlePoint.GlobalTransform.Basis;

            bulletInstance.Transform = tmpBasisis;
        }

        private void SpawnBulletHoles()
        {
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

    }
}
