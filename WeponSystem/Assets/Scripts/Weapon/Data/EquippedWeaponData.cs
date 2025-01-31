using System;

[Serializable]
public struct EquippedWeaponData
{
    public WeaponData currentWeaponData;
    public int ammoAvailableInMagazine;
    public int extraAmmoAvailable;
}