using System.Collections.Generic;
using UnityEngine;

public class WeaponController
{
    private EquippedWeaponData m_currentEquippedWeaponData;
    private List<WeaponTypeModleMap> m_weaponTypeModleMap;
    private Dictionary<WeaponStates, WeaponState> m_weaponStatesMap;
    private WeaponState m_currentWeaponState;
    private WeaponTypeModleMap m_currenWeaponTypeModleMap;
    
    public EquippedWeaponData CurrentEquippedWeaponData => m_currentEquippedWeaponData;

    public WeaponController(List<WeaponTypeModleMap>weaponTypeModleMap)
    {
        m_weaponTypeModleMap = weaponTypeModleMap;
        m_weaponStatesMap = new()
        {
            { WeaponStates.IDLE , new IdleWeaponState(this)},
            { WeaponStates.FIRE , new FireWeaponState(this)},
            { WeaponStates.RELOAD , new ReloadWeaponState(this)},
            { WeaponStates.OUT_OF_AMMO , new OutOfAmmoWeaponState(this)},
        };
    }
    
    public void Fire()
    {
        SwitchWeaponState(WeaponStates.FIRE);
    }

    public void Reload()
    {
        SwitchWeaponState(WeaponStates.RELOAD);
    }

    public void SwitchWeapon(EquippedWeaponData newEquippedWeaponData)
    {
        m_currentWeaponState?.HandleWeaponSwitch();
        SwitchWeaponState(WeaponStates.IDLE);
        m_currentEquippedWeaponData = newEquippedWeaponData;
        WeaponTypes nextWeaponType = newEquippedWeaponData.currentWeaponData.weaponType;
        if (m_currenWeaponTypeModleMap.weaponType == nextWeaponType)
        {
            return;
        }
        WeaponTypeModleMap nextWeaponTypeModleMap = m_weaponTypeModleMap.Find(data => data.weaponType == nextWeaponType);
        m_currenWeaponTypeModleMap.weaponModle?.SetActive(false);
        nextWeaponTypeModleMap.weaponModle?.SetActive(true);
        m_currenWeaponTypeModleMap = nextWeaponTypeModleMap;
    }
    
    private void SwitchWeaponState(WeaponStates weaponState)
    {
        if (m_currentWeaponState.WeaponStates == weaponState)
        {
            return;
        }
        Debug.Log($"New Weapon State : {weaponState}");
        m_currentWeaponState?.OnStateExit();
        WeaponState newWeaponState = m_weaponStatesMap.GetValueOrDefault(weaponState);
        newWeaponState.OnStateEnter();
        m_currentWeaponState = newWeaponState;
    }
}