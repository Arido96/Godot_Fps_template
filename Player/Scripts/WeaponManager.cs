using Godot;
using System;

public partial class WeaponManager : Node3D
{
    //Weapon manager. Used to set refrences on equipped weapon
    //TODO: switch weapon

	[Export] private Node3D _camera;
	[Export] private CharacterBody3D _playerScript;
	[Export] private Node3D _recoilReciever;

	private GunController _gunController;

    public override void _EnterTree()
    {
        _gunController = GetNode("RecoilReciever/Gun") as GunController;

        _gunController.RecoilReciever = _recoilReciever;
        _gunController.PlayerController = _playerScript;
    }
  
}
