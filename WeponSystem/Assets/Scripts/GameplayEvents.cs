using System;

public static class GameplayEvents
{
    public static Action<EquippedWeaponData> OnWeaponSwitched;
    public static Action<EquippedWeaponData> OnWeaponReloaded;
    public static Action OnWeaponFired;

    public static void SendOnWeaponSwitched(EquippedWeaponData equippedWeaponData)
    {
        OnWeaponSwitched?.Invoke(equippedWeaponData);
    }
    
    public static void SendOnWeaponReloaded(EquippedWeaponData equippedWeaponData)
    {
        OnWeaponReloaded?.Invoke(equippedWeaponData);
    }
    
    public static void SendOnWeaponFired()
    {
        OnWeaponFired?.Invoke();
    }
}