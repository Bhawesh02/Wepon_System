public class IdleWeaponStateController : WeaponStateController
{
    public IdleWeaponStateController(WeaponController weaponController, WeaponStates weaponState = WeaponStates.IDLE) : base(weaponController, weaponState)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if (m_equippedWeaponData.ammoAvailableInMagazine <= 0)
        {
            m_weaponController.Reload();
        }
    }

    public override void OnUpdate()
    {
    }

    public override void HandleWeaponSwitch()
    {
    }

    public override void OnStateExit()
    {
    }
}