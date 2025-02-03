using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct EquippedWeaponData
{
    public WeaponData currentWeaponData;
    public int ammoAvailableInMagazine;
    public int extraAmmoAvailable;
    public WeaponEquipTypes weaponEquipType;
    public int equipIndex;
}