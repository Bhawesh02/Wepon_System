using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_rotationSpeed = 5f;
    [SerializeField] private float m_interactableRayDistance = 15f;
    [SerializeField] private LayerMask m_interactableLayerMask;
    [SerializeField] private Transform m_head;
    [SerializeField] private Vector2 m_upDownRotationRange;
    [SerializeField] private List<WeaponTypeModleMap> m_weaponTypeModleMap;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private ParticleSystem m_fireParticel;
    
    private Vector2 m_movementInput;
    private Vector2 m_rotationInput;
    private WeaponController m_weaponController;
    private WeaponConfig m_weaponConfig;
    private EquippedWeaponData[] m_primaryEquippedWeapons;
    private EquippedWeaponData[] m_secondaryEquippedWeapons;
    private Dictionary<WeaponEquipTypes, EquippedWeaponData[]> m_equippedWeaponDatasMap;
    private InputConfig m_inputConfig;
    private Transform m_playerTransform;
    private Ray m_interactableRay;
    private RaycastHit m_interactableRayHit;
    private PickableInteractable m_interactable;

    public List<WeaponTypeModleMap> WeaponTypeModleMaps => m_weaponTypeModleMap;
    public ParticleSystem FireParticle => m_fireParticel;
    public Camera MainCamera => m_mainCamera;

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
        m_inputConfig = InputConfig.Instance;
        m_playerTransform = transform;
        GameplayEvents.OnWeaponPickedUp += EquipWeapon;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void OnDestroy()
    {
        GameplayEvents.OnWeaponPickedUp -= EquipWeapon;
    }

    private void Update()
    {
        HandleInput();
        m_weaponController.OnUpdate();
        HandleInteractable();
    }

    private void HandleInput()
    {
        HandleMovement();
        HandleRotation();
        HandleFirAndReload();
        CheckWeaponSwitch();
    }

    #region Movement
    private void HandleMovement()
    {
        m_movementInput.x = Input.GetAxis(m_inputConfig.horizontalMovementAxis);
        m_movementInput.y = Input.GetAxis(m_inputConfig.verticalMovementAxis);
        m_playerTransform.position += m_playerTransform.forward * (m_movementInput.y * m_movementSpeed * Time.deltaTime);
        m_playerTransform.position += m_playerTransform.right * (m_movementInput.x * m_movementSpeed * Time.deltaTime);
    }
    
    private void HandleRotation()
    {
        m_rotationInput.x += Input.GetAxis(m_inputConfig.mouseXAxis) * m_rotationSpeed * Time.deltaTime;
        m_rotationInput.y += Input.GetAxis(m_inputConfig.mouseYAxis) * m_rotationSpeed * Time.deltaTime;
        m_rotationInput.y = Mathf.Clamp(m_rotationInput.y,m_upDownRotationRange.x, m_upDownRotationRange.y);
        m_head.localRotation = Quaternion.Euler(-m_rotationInput.y, 0, 0f);
        m_playerTransform.localRotation = Quaternion.Euler(0f, m_rotationInput.x, 0f);
    }
    

    #endregion

    #region Weapon

    private void HandleFirAndReload()
    {
        if (Input.GetKey(m_inputConfig.fireKey))
        {
            m_weaponController.Fire();
        }
        if (Input.GetKeyUp(m_inputConfig.fireKey))
        {
            m_weaponController.StopFire();
        }
        if (Input.GetKeyDown(m_inputConfig.reloadKey))
        {
            m_weaponController.Reload();
        }
    }

    private void CheckWeaponSwitch()
    {
        HandleWeaponSwitch(m_primaryEquippedWeapons);
        HandleWeaponSwitch(m_secondaryEquippedWeapons);
    }

    private void HandleWeaponSwitch(EquippedWeaponData[] equippedWeaponDatas)
    {
        foreach (EquippedWeaponData equippedWeaponData in equippedWeaponDatas)
        {
            if (!equippedWeaponData.currentWeaponData)
            {
                continue;    
            }
            if (Input.GetKeyDown(GetKeyCode(equippedWeaponData)))
            {
                m_weaponController.SwitchWeapon(equippedWeaponData);
            }
        }
    }

    private KeyCode GetKeyCode(EquippedWeaponData equippedWeaponData)
    {
        switch (equippedWeaponData.weaponEquipType)
        {
            case WeaponEquipTypes.PRIMARY:
                return equippedWeaponData.equipIndex == 0 ? m_inputConfig.firstPrimarySwitchKey : m_inputConfig.secondPrimarySwitchKey;
            case WeaponEquipTypes.SECONDAY:
                return m_inputConfig.firstSecondarySwitchKey;
            default:
                return m_inputConfig.firstPrimarySwitchKey;
        }
    }

    private void EquipWeapon(WeaponData weaponData)
    {
        WeaponEquipTypes weaponEquipType = m_weaponConfig.GetWeaponEquipType(weaponData.weaponType);
        AddToEquippedWeapon(weaponEquipType, weaponData);
    }

    private void AddToEquippedWeapon(WeaponEquipTypes weaponEquipType, WeaponData weponDataToAdd)
    {
        EquippedWeaponData[] equippedWeaponDatas = m_equippedWeaponDatasMap.GetValueOrDefault(weaponEquipType);
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
            equippedWeaponDatas[equippedWeaponIndex].weaponEquipType = weaponEquipType;
            equippedWeaponDatas[equippedWeaponIndex].equipIndex = equippedWeaponIndex;
            if (!m_weaponController.CurrentEquippedWeaponData.currentWeaponData)
            {
                m_weaponController.SwitchWeapon(equippedWeaponDatas[equippedWeaponIndex]);
            }
            GameplayEvents.SendOnWeaponEquipped(equippedWeaponDatas[equippedWeaponIndex]);
            return;
        }

        SwitchGun(weaponEquipType, weponDataToAdd);
    }

    private void SwitchGun(WeaponEquipTypes weaponEquipType, WeaponData weponDataToAdd)
    {
        EquippedWeaponData equippedWeaponData = m_weaponController.CurrentEquippedWeaponData;
        equippedWeaponData.currentWeaponData = weponDataToAdd;
        equippedWeaponData.ammoAvailableInMagazine = weponDataToAdd.magazineSize;
        equippedWeaponData.extraAmmoAvailable =
            weponDataToAdd.magazineSize * weponDataToAdd.maxMagnizeNumberToHold;
        if (equippedWeaponData.weaponEquipType == weaponEquipType)
        {
            m_weaponController.SwitchWeapon(equippedWeaponData);
        }
        else
        {
            // Replace Gun at first index of any equip type
            equippedWeaponData.equipIndex = 0;
            equippedWeaponData.weaponEquipType = weaponEquipType;
        }
        UpdateWeaponData(equippedWeaponData);
        GameplayEvents.SendOnWeaponEquipped(equippedWeaponData);
    }

    public void UpdateWeaponData(EquippedWeaponData equippedWeaponData)
    {
        m_equippedWeaponDatasMap.GetValueOrDefault(equippedWeaponData.weaponEquipType)
                [equippedWeaponData.equipIndex] 
            =  equippedWeaponData;
    }

    #endregion

    #region Interactable

    private void HandleInteractable()
    {
        m_interactableRay = new Ray(m_mainCamera.transform.position, m_mainCamera.transform.forward);
        Debug.DrawLine(m_interactableRay.origin, m_interactableRay.direction * m_interactableRayDistance);
        if (!Physics.Raycast(m_interactableRay, out m_interactableRayHit, m_interactableRayDistance,
                m_interactableLayerMask))
        {
            GameplayEvents.SendOnInteractableHoverRemove();
            return;
        }
        m_interactable = m_interactableRayHit.transform.GetComponent<PickableInteractable>();
        if (m_interactable == null)
        {
            GameplayEvents.SendOnInteractableHoverRemove();
            return;
        }
        GameplayEvents.SendOnInteractableHover(m_interactable);
        if (Input.GetKeyDown(m_inputConfig.interactableKey))
        {
            GameplayEvents.SendOnInteractableSelect(m_interactable);
        }
    }

    #endregion
}