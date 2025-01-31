using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    private const int NUM_OF_PRIMARY_WEAPONS = 2;
    private const int NUM_OF_SECONDARY_WEAPONS = 1;

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private List<WeaponTypeModleMap> m_weaponTypeModleMap;
    
    private float m_horizontalInput;
    private float m_verticalInput;
    private WeaponController m_weaponController;
    private WeaponConfig m_weaponConfig;
    private EquippedWeaponData[] m_primaryEquippedWeapons = new EquippedWeaponData[NUM_OF_PRIMARY_WEAPONS];
    private EquippedWeaponData[] m_secondaryEquippedWeapons = new EquippedWeaponData[NUM_OF_SECONDARY_WEAPONS];

    private void Awake()
    {
        m_weaponController = new WeaponController(m_weaponTypeModleMap);
        m_weaponConfig = WeaponConfig.Instance;
    }

    private void Update()
    {
        m_horizontalInput = Input.GetAxis(HORIZONTAL_AXIS);
        m_verticalInput = Input.GetAxis(VERTICAL_AXIS);
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * (m_verticalInput * m_movementSpeed * Time.deltaTime);
        transform.position += transform.right * (m_horizontalInput * m_movementSpeed * Time.deltaTime);
    }

    private void EquipWeapon(WeaponData weaponData)
    {
        AddToEquippedWeapon(
            m_weaponConfig.primaryWeapons.Contains(weaponData.weaponType)
                ? m_primaryEquippedWeapons
                : m_secondaryEquippedWeapons, weaponData);
    }

    private void AddToEquippedWeapon(EquippedWeaponData[] equippedWeaponDatas, WeaponData weponDataToAdd)
    {
        for (int equippedWeaponIndex = 0; equippedWeaponIndex < equippedWeaponDatas.Length; equippedWeaponIndex++)
        {
            if (equippedWeaponDatas[equippedWeaponIndex].currentWeaponData != null)
            {
                continue;
            }
            equippedWeaponDatas[equippedWeaponIndex].currentWeaponData = weponDataToAdd;
            equippedWeaponDatas[equippedWeaponIndex].ammoAvailableInMagazine = weponDataToAdd.magazineSize;
            equippedWeaponDatas[equippedWeaponIndex].extraAmmoAvailable =
                weponDataToAdd.magazineSize * weponDataToAdd.maxMagnizeNumberToHold;
            if (!m_weaponController.CurrentEquippedWeaponData.currentWeaponData)
            {
                m_weaponController.SwitchWeapon(equippedWeaponDatas[equippedWeaponIndex]);
            }
            break;
        }
    }
}