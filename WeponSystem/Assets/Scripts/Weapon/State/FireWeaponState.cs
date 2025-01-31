public class FireWeaponState : WeaponState
{

    private float m_nextFireTime;
    
    public FireWeaponState(WeaponController weaponController, WeaponStates weaponState = WeaponStates.FIRE) : base(weaponController, weaponState)
    {
        m_nextFireTime = 0f;
    }

    public override void OnStateEnter()
    {
        //TODO
    }

    public override void OnUpdate()
    {
        //TODO
    }

    private void Fire()
    {
        //TODO
    }

    private void CanFire()
    {
        
    }
    
    public override void HandleWeaponSwitch()
    {
        m_nextFireTime = 0f;
        //TODO
    }

    public override void OnStateExit()
    {
        //TODO
    }
}