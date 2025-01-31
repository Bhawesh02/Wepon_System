using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon/NewWeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("General Info")]
    public WeaponTypes weaponType;
    public Sprite weaponIcon;
    public string weaponName;

    [Header("Fire Properties")] 
    public float weaponDamage;
    public float fireDistance;
    public float fireRate;

    [Header("Ammo Properties")] 
    public AmmoTypes ammoType;
    [Min(0)]
    public int magazineSize;
    [Min(0)]
    public int maxMagnizeNumberToHold;
    
}