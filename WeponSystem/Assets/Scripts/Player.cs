using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private List<WeaponTypeModleMap> m_weaponTypeModleMap;
    [SerializeField] private KeyCode m_fireKeyCode = KeyCode.Mouse0;
    [SerializeField] private KeyCode m_reloadKeyCode = KeyCode.R;
    
    private float m_horizontalInput;
    private float m_verticalInput;
    private WeaponController m_weaponController;
    private WeaponConfig m_weaponConfig;
    private EquippedWeaponData[] m_primaryEquippedWeapons;
    private EquippedWeaponData[] m_secondaryEquippedWeapons;
    private Dictionary<WeaponEquipTypes, EquippedWeaponData[]> m_equippedWeaponDatasMap;

    public List<WeaponTypeModleMap> WeaponTypeModleMaps => m_weaponTypeModleMap;

    private void Awake()
    {
        m_weaponController = new WeaponController(this);
        m_weaponConfig = WeaponConfig.Instance;
        m_primaryEquippedWeapons = new EquippedWeaponData[m_weaponConfig.numOfPrimaryWeapons];
        m_secondaryEquippedWeapons = new EquippedWeaponData[m_weaponConfig.numOfSecondaryWeapons];
        m_equippedWeaponDatasMap = new()
        {
            { WeaponEquipTypes.PRIMARY , m_primaryEquippedWeapons},
            { WeaponEquipTypes.SECONDAY , m_secondaryEquippedWeapons},
        };
        GameplayEvents.OnWeaponPickedUp += EquipWeapon;
    }
    

    private void OnDestroy()
    {
        GameplayEvents.OnWeaponPickedUp -= EquipWeapon;
    }

    private void Update()
    {
        m_horizontalInput = Input.GetAxis(HORIZONTAL_AXIS);
        m_verticalInput = Input.GetAxis(VERTICAL_AXIS);
        Move();
        if (Input.GetKey(m_fireKeyCode))
        {
            m_weaponController.Fire();
        }
        else if (Input.GetKeyUp(m_fireKeyCode))
        {
            m_weaponController.StopFire();
        }

        if (Input.GetKeyDown(m_reloadKeyCode))
        {
            m_weaponController.Reload();
        }
        m_weaponController.OnUpdate();
    }

    private void Move()
    {
        transform.position += transform.forward * (m_verticalInput * m_movementSpeed * Time.deltaTime);
        transform.position += transform.right * (m_horizontalInput * m_movementSpeed * Time.deltaTime);
    }

    private void EquipWeapon(WeaponData weaponData)
    {
        WeaponEquipTypes weaponEquipType = m_weaponConfig.GetWeaponEquipType(weaponData.weaponType);
        
        int weaponEquipIndex = AddToEquippedWeapon(
            m_equippedWeaponDatasMap.GetValueOrDefault(weaponEquipType), weaponData);

        if (weaponEquipIndex != -1)
        {
            GameplayEvents.SendOnWeaponEquipped(weaponData, weaponEquipType, weaponEquipIndex);
        }    
    }

    private int AddToEquippedWeapon(EquippedWeaponData[] equippedWeaponDatas, WeaponData weponDataToAdd)
    {
        for (int equippedWeaponIndex = 0; equippedWeaponIndex < equippedWeaponDatas.Length; equippedWeaponIndex++)
        {
            if (equippedWeaponDatas[equippedWeaponIndex].currentWeaponData)
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
            return equippedWeaponIndex;
        }
        return -1;
    }
}