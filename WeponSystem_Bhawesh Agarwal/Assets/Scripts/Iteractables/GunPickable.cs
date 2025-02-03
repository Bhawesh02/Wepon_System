using System.Collections.Generic;
using UnityEngine;

public class GunPickable : PickableInteractable
{
    [SerializeField] private WeaponData m_weaponData;
    [SerializeField] private List<WeaponTypeModleMap> m_weaponTypeModleMaps;

    private void Start()
    {
        m_weaponTypeModleMaps.Find(map => map.weaponType == m_weaponData.weaponType)
            .weaponModle?.SetActive(true);
    }

    protected override void HandleOnPlayerSelected(PickableInteractable pickableInteractable)
    {
        if (pickableInteractable != this)
        {
            return;
        }
        GameplayEvents.SendOnWeaponPickedUp(m_weaponData);
    }
}