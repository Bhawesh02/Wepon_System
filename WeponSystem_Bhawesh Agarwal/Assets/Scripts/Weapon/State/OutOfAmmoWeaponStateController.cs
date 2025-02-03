public class OutOfAmmoWeaponStateController : WeaponStateController
{
    private const string MESSAGE_TEXT = "Out Of Ammo";
    public OutOfAmmoWeaponStateController(WeaponController weaponController, WeaponStates weaponState = WeaponStates.OUT_OF_AMMO) : base(weaponController, weaponState)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        GameplayEvents.SendOnShowMessage(MESSAGE_TEXT);
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

