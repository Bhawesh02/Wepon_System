using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponConfig : GenericConfig<WeaponConfig>
{
    public List<WeaponTypes> primaryWeapons = new();
    public List<WeaponTypes> secondaryWeapons = new();
    
    
    private void OnValidate()
    {
        for (int secondaryWeaponIndex = secondaryWeapons.Count - 1; secondaryWeaponIndex >= 0; secondaryWeaponIndex--)
        {
            if (secondaryWeapons[secondaryWeaponIndex] == WeaponTypes.NULL || !primaryWeapons.Contains(secondaryWeapons[secondaryWeaponIndex]))
            {
                continue;
            }
            Debug.LogWarning($"Primary weapon already contains {secondaryWeapons[secondaryWeaponIndex]}");
            secondaryWeapons[secondaryWeaponIndex] = WeaponTypes.NULL;
        }
    }
}
