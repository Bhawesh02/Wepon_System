using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameScreenUI : MonoBehaviour
{
    [Serializable]
    private struct ActiveGunReferences{
        public Image currentActiveGunIcon;
        public TextMeshProUGUI currentActiveGumAmmoInMag;
        public TextMeshProUGUI currentActiveExtraAmmo;
    }
    [Serializable]
    private struct EqiupGunReferences{
        public Image gunIcon;
        public TextMeshProUGUI switchKey;
    }
    
    private const string GUN_AMMO_IN_MAG_PREFIX = "Ammo in Mag : ";
    private const string EXTRA_AMMO_PREFIX = "Extra Ammo : ";

    [Header("Current Active Gun")] 
    [SerializeField]
    private ActiveGunReferences m_currentActiveGun;
    
    [Header("Equipped Weapon")] 
    [SerializeField] private EqiupGunReferences[] m_primaryEquippedWeaponHolders;
    [SerializeField] private EqiupGunReferences[] m_secondaryEquippedWeaponHolders;

    [Header("Misc")] 
    [SerializeField] private TextMeshProUGUI m_message;
    
    private Dictionary<WeaponEquipTypes, EqiupGunReferences[]> m_weaponEquipIconHolderMap;

    private void Awake()
    {
        m_weaponEquipIconHolderMap = new()
        {
            { WeaponEquipTypes.PRIMARY , m_primaryEquippedWeaponHolders},
            { WeaponEquipTypes.SECONDAY , m_secondaryEquippedWeaponHolders}
        };
        GameplayEvents.OnWeaponEquipped += HandleOnWeaponEquipped;
        GameplayEvents.OnWeaponSwitched += HandleOnWeaponSwitched;
        GameplayEvents.OnWeaponAmmoChange += HandleOnWeaponAmmoChange;
        GameplayEvents.OnShowMessage += HandleOnShowMessage;
        GameplayEvents.OnHideMessage += HandleOnHideMessage;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnWeaponEquipped -= HandleOnWeaponEquipped;
        GameplayEvents.OnWeaponSwitched -= HandleOnWeaponSwitched;
        GameplayEvents.OnWeaponAmmoChange -= HandleOnWeaponAmmoChange;
        GameplayEvents.OnShowMessage -= HandleOnShowMessage;
        GameplayEvents.OnHideMessage -= HandleOnHideMessage;
    }
    
    private void HandleOnWeaponEquipped(WeaponData weaponData, WeaponEquipTypes weaponEquipTypes, int weponIndex)
    {
        EqiupGunReferences[] iconHolder = m_weaponEquipIconHolderMap.GetValueOrDefault(weaponEquipTypes);
        iconHolder[weponIndex].gunIcon.sprite = weaponData.weaponIcon;
    }
    
    private void HandleOnWeaponSwitched(EquippedWeaponData equippedWeaponData)
    {
        m_currentActiveGun.currentActiveGunIcon.sprite = equippedWeaponData.currentWeaponData.weaponIcon;
        HandleOnWeaponAmmoChange(equippedWeaponData.ammoAvailableInMagazine, equippedWeaponData.extraAmmoAvailable);
    }
    
    private void HandleOnWeaponAmmoChange(int ammoAvailableInMagazine, int extraAmmoAvailable)
    {
        m_currentActiveGun.currentActiveGumAmmoInMag.text = GUN_AMMO_IN_MAG_PREFIX + ammoAvailableInMagazine;
        m_currentActiveGun.currentActiveExtraAmmo.text = EXTRA_AMMO_PREFIX + extraAmmoAvailable;
    }
    
    private void HandleOnShowMessage(string messgaeText)
    {
        m_message.gameObject.SetActive(true);
        m_message.text = messgaeText;
    }
    
    
    private void HandleOnHideMessage()
    {
        m_message.gameObject.SetActive(false);
    }
}
