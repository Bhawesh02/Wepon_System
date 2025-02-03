using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponController
{
    private EquippedWeaponData m_currentEquippedWeaponData;
    private List<WeaponTypeModleMap> m_weaponTypeModleMap;
    private Dictionary<WeaponStates, WeaponStateController> m_weaponStatesMap;
    private WeaponStateController m_currentWeaponStateController;
    private WeaponStates m_currentWeaponState;
    private WeaponTypeModleMap m_currenWeaponTypeModleMap;
    private Player m_player;
    
    public EquippedWeaponData CurrentEquippedWeaponData => m_currentEquippedWeaponData;

    public WeaponController(Player player)
    {
        m_weaponTypeModleMap = player.WeaponTypeModleMaps;
        m_player = player;
        m_weaponStatesMap = new()
        {
            { WeaponStates.IDLE , new IdleWeaponStateController(this)},
            { WeaponStates.FIRE , new FireWeaponStateController(this)},
            { WeaponStates.RELOAD , new ReloadWeaponStateController(this)},
            { WeaponStates.OUT_OF_AMMO , new OutOfAmmoWeaponStateController(this)},
        };
        m_currentWeaponStateController = m_weaponStatesMap.GetValueOrDefault(WeaponStates.IDLE);
        m_currentWeaponState = WeaponStates.IDLE;
    }

    
    public void OnUpdate()
    {
        m_currentWeaponStateController?.OnUpdate();
    }
    
    public void Fire()
    {
        if (CantStartFire())
        {
            return;
        }
        SwitchWeaponState(WeaponStates.FIRE);
    }

    private bool CantStartFire()
    {
        return !m_currentEquippedWeaponData.currentWeaponData || m_currentWeaponState == WeaponStates.RELOAD || m_currentWeaponState == WeaponStates.OUT_OF_AMMO;
    }

    public void StopFire()
    {
        if (!m_currentEquippedWeaponData.currentWeaponData || m_currentWeaponState != WeaponStates.FIRE)
        {
            return;
        }
        SwitchWeaponState(WeaponStates.IDLE);
    }

    public void Reload()
    {
        if (!m_currentEquippedWeaponData.currentWeaponData)
        {
            return;
        }
        SwitchWeaponState(WeaponStates.RELOAD);
    }

    public void HandleOutOfAmmo()
    {
        SwitchWeaponState(WeaponStates.OUT_OF_AMMO);
    }
    
    public void SwitchWeapon(EquippedWeaponData switchWeaponData)
    {
        m_currentWeaponStateController?.HandleWeaponSwitch();
        SwitchWeaponState(WeaponStates.IDLE);
        m_currentEquippedWeaponData = switchWeaponData;
        WeaponModleSwitch(m_currentEquippedWeaponData);
        GameplayEvents.SendOnWeaponSwitched(m_currentEquippedWeaponData);
    }

    public void SwitchWeaponState(WeaponStates weaponState)
    {
        if (m_currentWeaponState == weaponState)
        {
            return;
        }
        Debug.Log($"New Weapon State : {weaponState}");
        m_currentWeaponStateController?.OnStateExit();
        WeaponStateController newWeaponStateController = m_weaponStatesMap.GetValueOrDefault(weaponState);
        newWeaponStateController.OnStateEnter();
        m_currentWeaponStateController = newWeaponStateController;
        m_currentWeaponState = weaponState;
    }

    private void WeaponModleSwitch(EquippedWeaponData switchWeaponData)
    {
        WeaponTypes nextWeaponType = switchWeaponData.currentWeaponData.weaponType;
        if (switchWeaponData.currentWeaponData == null)
        {
            Debug.Break();
        }
        if (m_currenWeaponTypeModleMap.weaponType == nextWeaponType)
        {
            return;
        }
        WeaponTypeModleMap nextWeaponTypeModleMap = m_weaponTypeModleMap.Find(data => data.weaponType == nextWeaponType);
        m_currenWeaponTypeModleMap.weaponModle?.SetActive(false);
        nextWeaponTypeModleMap.weaponModle?.SetActive(true);
        m_currenWeaponTypeModleMap = nextWeaponTypeModleMap;
    }
    
    public void UpdateWeaponAmmo(EquippedWeaponData equippedWeaponData)
    {
        m_currentEquippedWeaponData = equippedWeaponData;
        m_player.UpdateWeaponData();
        GameplayEvents.SendOnWeaponAmmoChange(equippedWeaponData.ammoAvailableInMagazine, equippedWeaponData.extraAmmoAvailable);
    }

    public void RunCoroutine(IEnumerator enumerator)
    {
        m_player.StartCoroutine(enumerator);
    }
    
}