using UnityEngine;

public class FireWeaponStateController : WeaponStateController
{
    private float m_nextFireTime;
    
    public FireWeaponStateController(WeaponController weaponController, WeaponStates weaponState = WeaponStates.FIRE) : base(weaponController, weaponState)
    {
        m_nextFireTime = 0f;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        m_nextFireTime = 0f;
    }

    public override void OnUpdate()
    {
        Fire();
    }

    private void Fire()
    {
        if (!CanFire())
        {
            return;
        }
        m_equippedWeaponData.ammoAvailableInMagazine--;
        m_weaponController.PlayFireParticle();
        m_weaponController.UpdateWeaponAmmo(m_equippedWeaponData);
        m_nextFireTime = Time.time + 1 / m_equippedWeaponData.currentWeaponData.fireRate;
    }

    private bool CanFire()
    {
        return HasAmmo() && Time.time >= m_nextFireTime;
    }

    private bool HasAmmo()
    {
        if (m_equippedWeaponData.ammoAvailableInMagazine > 0)
        {
            return true;
        }
        m_weaponController.Reload();
        return false;

    }
    
    public override void HandleWeaponSwitch()
    {
        m_nextFireTime = 0f;
    }

    public override void OnStateExit()
    {
        //TODO
    }
}