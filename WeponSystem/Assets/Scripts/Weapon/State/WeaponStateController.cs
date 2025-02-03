public abstract class WeaponStateController
{
    protected WeaponStates m_weaponState;
    protected WeaponController m_weaponController;
    protected EquippedWeaponData m_equippedWeaponData;

    
    public WeaponStates WeaponStates => m_weaponState; 
    
    protected WeaponStateController(WeaponController weaponController, WeaponStates weaponState)
    {
        m_weaponController = weaponController;
        m_weaponState = weaponState;
    }

    public virtual void OnStateEnter()
    {
        m_equippedWeaponData = m_weaponController.CurrentEquippedWeaponData;
    }
    public abstract void OnUpdate();
    public abstract void HandleWeaponSwitch();
    public abstract void OnStateExit();

}