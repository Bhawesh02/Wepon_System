using UnityEngine;

public class InputConfig : GenericConfig<InputConfig>
{
    [Header("Player Movement Input")]
    public string horizontalMovementAxis = "Horizontal";
    public string verticalMovementAxis = "Vertical";
    public string mouseXAxis = "Mouse X";
    public string mouseYAxis = "Mouse Y";
    
    [Header("Weapon Input")] 
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode firstPrimarySwitchKey = KeyCode.Alpha1;
    public KeyCode secondPrimarySwitchKey = KeyCode.Alpha2;
    public KeyCode firstSecondarySwitchKey = KeyCode.Alpha3;

    [Header("Interactable")] 
    public KeyCode interactableKey = KeyCode.F;
}