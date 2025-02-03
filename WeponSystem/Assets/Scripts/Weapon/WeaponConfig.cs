using System.Collections.Generic;
using UnityEngine;

public class WeaponConfig : GenericConfig<WeaponConfig>
{
    
    
    [SerializeField]
    private List<WeaponTypes> m_primaryWeapons = new();
    [SerializeField]
    private List<WeaponTypes> m_secondaryWeapons = new();
    
    [Min(1)]
    public int numOfPrimaryWeapons = 2;
    [Min(1)]
    public int numOfSecondaryWeapons = 1;
    
    private Dictionary<WeaponEquipTypes, List<WeaponTypes>> weaponEquipMap;

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
