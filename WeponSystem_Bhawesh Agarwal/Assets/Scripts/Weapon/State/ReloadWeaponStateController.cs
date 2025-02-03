using UnityEngine;

public class ReloadWeaponStateController : WeaponStateController
{
    private const string MESSAGE_TEXT = "Reloading..";
    
    public ReloadWeaponStateController(WeaponController weaponController, WeaponStates weaponState = WeaponStates.RELOAD) : base(weaponController, weaponState)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        GameplayEvents.SendOnShowMessage(MESSAGE_TEXT);
        m_weaponController.RunCoroutine(CoroutineUtils.Delay(m_equippedWeaponData.currentWeaponData.reloadTime, Reload));
    }

    private void Reload()
    {
        if (!HasExtraAmmo())
        {
            return;
        }
        int ammoToAdd = Mathf.Min(m_equippedWeaponData.extraAmmoAvailable, (m_equippedWeaponData.currentWeaponData.magazineSize - m_equippedWeaponData.ammoAvailableInMagazine));
        m_equippedWeaponData.ammoAvailableInMagazine += ammoToAdd;
        m_equippedWeaponData.extraAmmoAvailable -= ammoToAdd;
        m_weaponController.UpdateWeaponAmmo(m_equippedWeaponData);
        m_weaponController.SwitchWeaponState(WeaponStates.IDLE);
        
    }

    private bool HasExtraAmmo()
    {
        if (m_equippedWeaponData.extraAmmoAvailable > 0)
        {
            return true;
        }
        m_weaponController.HandleOutOfAmmo();
        return false;
    }

    public override void OnUpdate()
    {
    }

    public override void HandleWeaponSwitch()
    {
    }

    public override void OnStateExit()
    {
        GameplayEvents.SendOnHideMessage();
    }
}