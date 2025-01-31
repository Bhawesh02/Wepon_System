public abstract class WeaponState
{
    protected WeaponStates m_weaponState;
    protected WeaponController m_weaponController;

    public WeaponStates WeaponStates => m_weaponState; 
    
    protected WeaponState(WeaponController weaponController, WeaponStates weaponState)
    {
        m_weaponController = weaponController;
        m_weaponState = weaponState;
    }

    public abstract void OnStateEnter();
    public abstract void OnUpdate();
    public abstract void HandleWeaponSwitch();
    public abstract void OnStateExit();

}