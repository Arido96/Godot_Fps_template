using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiPlayerFps.Weapons.Scripts.ShootComponent;

namespace MultiPlayerFps.Weapons.Scripts
{
    public partial class RecoilComponent : Node3D
    {
        private Vector3 initialPosition;
        private Vector3 initalRotation;
        private Vector3 initalCamPosition;
        private Node3D _recoilReciever;

        [Export]private CharacterBody3D _char;
        [Export] private GunController _gunController;
   
        #region CameraRecoil
        [Export] private float _cameraRecoilVertical = 10.0f; //Change to adjust of Camera recoil
        [Export] private float _cameraRecoilHorizontal = 10.0f; //not jet implementet
        [Export] private float _recoilResetSpeed; // Change to Adjust the Camera Recoil reset - less means Higher Camera Recoil

        private float _currentVerticalRecoil;
        private float _currentHorizontalRecoil;
        #endregion


        #region GunModelRecoil
        [Export] private float _kickBack = 10.0f; //change to adjust kickback of gun
        [Export] private float _maxXRecoil; // change to adjust the Gun model roation on X-Axisis(RED)
        [Export] private float _maxYRecoil;// change to adjust the Gun model roation on Y-Axisis(GREEN)
        [Export] private float _maxZRecoil;// change to adjust the Gun model roation on Z-Axisis(BLUE)
        #endregion


        public override void _Process(double delta)
        {
           
            if (_recoilReciever == null)
            {
                _recoilReciever = _gunController.RecoilReciever;
                GD.Print(_recoilReciever);
                initialPosition = _recoilReciever.Position;
            }




            if (_currentVerticalRecoil > 0) _currentVerticalRecoil -= (float)delta * _recoilResetSpeed ;
            if (_currentHorizontalRecoil > 0) _currentHorizontalRecoil -= (float)delta;

            _gunController.PlayerController.SetVerticalRecoil(_currentVerticalRecoil, _currentHorizontalRecoil);
        }


        public void  ResetRecoil()
        {
            Tween tween = CreateTween().SetParallel(true);

            if (_gunController.AdsController.isAdsing)
            {
                tween.TweenProperty(_recoilReciever, "position", _gunController.AdsController.AimDownSightOffset, 0.2f);
            }
            else
            {
                tween.TweenProperty(_recoilReciever, "position", initialPosition, 0.2f);
            }

            tween.TweenProperty(_recoilReciever, "rotation", Vector3.Zero, 0.4f);

        }

        
        

        public void  AddRecoil()
        {
            var randomY = (float)GD.RandRange(-_maxYRecoil, _maxYRecoil);
            var randomZ = (float)GD.RandRange(-_maxZRecoil, _maxZRecoil);
            Tween tween = CreateTween().SetParallel(true);

            if (_gunController.AdsController.isAdsing)
            {
                tween.TweenProperty(_recoilReciever, "position", _gunController.AdsController.AimDownSightOffset - new Vector3(0, 0, -_kickBack/2), .2f);
                tween.TweenProperty(_recoilReciever, "rotation", Rotation - new Vector3(-_maxXRecoil/2, randomY, randomZ), 0.05f);
            }
            else
            {
                tween.TweenProperty(_recoilReciever, "position", initialPosition - new Vector3(0, 0, -_kickBack), 0.2f);
                tween.TweenProperty(_recoilReciever, "rotation", Rotation - new Vector3(-_maxXRecoil, randomY, randomZ), 0.05f);
            }

            if(_gunController.AdsController.isAdsing) _currentVerticalRecoil = _cameraRecoilVertical / 1.25f;
            else _currentVerticalRecoil = _cameraRecoilVertical;

        }
    }
}
