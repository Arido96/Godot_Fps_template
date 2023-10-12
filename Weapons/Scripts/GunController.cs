using Godot;
using MultiPlayerFps.Weapons.Scripts;
using System;

public partial class GunController : Node3D
{

    //Holds all components and get all Nodes from WeaponManager

	public Node3D RecoilReciever { get; set; }
	public CharacterBody3D PlayerController { get; set; }

    #region Components
    [Export] public RecoilComponent RecoilComponent { private set; get; }
    [Export] public ShootComponent ShootComponent { private set; get; }
    [Export] public AdsController AdsController { private set; get; }

    #endregion

}
