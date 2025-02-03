public class IdleWeaponStateController : WeaponStateController
{
    public IdleWeaponStateController(WeaponController weaponController, WeaponStates weaponState = WeaponStates.IDLE) : base(weaponController, weaponState)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //TODO
    }

    public override void OnUpdate()
    {
        //TODO
    }

    public override void HandleWeaponSwitch()
    {
        //TODO
    }

    public override void OnStateExit()
    {
        //TODO
    }
}