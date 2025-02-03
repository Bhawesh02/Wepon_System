using System;

public static class GameplayEvents
{
    public static Action<WeaponData> OnWeaponPickedUp;
    public static Action<EquippedWeaponData> OnWeaponEquipped;
    public static Action<EquippedWeaponData> OnWeaponSwitched;
    public static Action<int, int> OnWeaponAmmoChange;
    public static Action<string> OnShowMessage;
    public static Action OnHideMessage;
    public static Action<PickableInteractable> OnPickableInteracleHover;
    public static Action OnPickableInteracleHoverRemove;
    public static Action<PickableInteractable> OnPickableInteracleSelect;

    public static void SendOnWeaponPickedUp(WeaponData weaponData)
    {
        OnWeaponPickedUp?.Invoke(weaponData);
    }

    public static void SendOnWeaponEquipped(EquippedWeaponData equippedWeaponData)
    {
        OnWeaponEquipped?.Invoke(equippedWeaponData);
    }
    
    public static void SendOnWeaponSwitched(EquippedWeaponData equippedWeaponData)
    {
        OnWeaponSwitched?.Invoke(equippedWeaponData);
    }
    
    public static void SendOnWeaponAmmoChange(int ammoInMagazine, int extraAmmoAvailable)
    {
        OnWeaponAmmoChange?.Invoke(ammoInMagazine, extraAmmoAvailable);
    }

    public static void SendOnShowMessage(string messageString)
    {
        OnShowMessage?.Invoke(messageString);
    }

    public static void SendOnHideMessage()
    {
        OnHideMessage?.Invoke();
    }

    public static void SendOnInteractableHover(PickableInteractable pickableInteractable)
    {
        OnPickableInteracleHover?.Invoke(pickableInteractable);
    }
    
    public static void SendOnInteractableSelect(PickableInteractable pickableInteractable)
    {
        OnPickableInteracleSelect?.Invoke(pickableInteractable);
    }
    
    public static void SendOnInteractableHoverRemove()
    {
        OnPickableInteracleHoverRemove?.Invoke();
    }
}