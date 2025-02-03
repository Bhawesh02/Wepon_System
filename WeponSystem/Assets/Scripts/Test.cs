using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private WeaponData m_weaponDataToEquip;
    [SerializeField] private KeyCode m_equipWeaponKeyCode;

    private void Update()
    {
        if (Input.GetKeyDown(m_equipWeaponKeyCode))
        {
            GameplayEvents.SendOnWeaponPickedUp(m_weaponDataToEquip);
        }
    }
}