using System.Collections.Generic;
using UnityEngine;

public class WeaponConfig : GenericConfig<WeaponConfig>
{
    private const int NUM_OF_PRIMARY_WEAPONS = 2;
    private const int NUM_OF_SECONDARY_WEAPONS = 1;
    
    [SerializeField]
    private List<WeaponTypes> m_primaryWeapons = new();
    [SerializeField]
    private List<WeaponTypes> m_secondaryWeapons = new();
    
    private Dictionary<WeaponEquipTypes, List<WeaponTypes>> weaponEquipMap;

    public int numOfPrimaryWeapons => NUM_OF_PRIMARY_WEAPONS;
    public int numOfSecondaryWeapons => NUM_OF_SECONDARY_WEAPONS;
    
    private void OnEnable()
    {
        weaponEquipMap = new()
        {
            { WeaponEquipTypes.PRIMARY , m_primaryWeapons},
            { WeaponEquipTypes.SECONDAY , m_secondaryWeapons}
        };
    }

    public List<WeaponTypes> GetWeaponListFromEquipType(WeaponEquipTypes weaponEquipType)
    {
        return weaponEquipMap.GetValueOrDefault(weaponEquipType);
    }

    public WeaponEquipTypes GetWeaponEquipType(WeaponTypes weaponType)
    {
        foreach (KeyValuePair<WeaponEquipTypes,List<WeaponTypes>> weaponEquipPair in weaponEquipMap)
        {
            if (weaponEquipPair.Value.Contains(weaponType))
            {
                return weaponEquipPair.Key;
            }
        }
        return WeaponEquipTypes.PRIMARY;
    }
    
    private void OnValidate()
    {
        for (int secondaryWeaponIndex = m_secondaryWeapons.Count - 1; secondaryWeaponIndex >= 0; secondaryWeaponIndex--)
        {
            if (m_secondaryWeapons[secondaryWeaponIndex] == WeaponTypes.NULL || !m_primaryWeapons.Contains(m_secondaryWeapons[secondaryWeaponIndex]))
            {
                continue;
            }
            Debug.LogWarning($"Primary weapon already contains {m_secondaryWeapons[secondaryWeaponIndex]}");
            m_secondaryWeapons[secondaryWeaponIndex] = WeaponTypes.NULL;
        }
    }
}
