using System;
using TMPro;
using UnityEngine;

public abstract class PickableInteractable : MonoBehaviour
{
    private const string MESSAGE_TEXT_PREFIX = "Press : ";
    private const string MESSAGE_TEXT_SUFFIX = " To pickup ";
    
    [SerializeField] 
    private TextMeshProUGUI m_message;

    private void Awake()
    {
        GameplayEvents.OnPickableInteracleHover += HandleOnPlayerHover;
        GameplayEvents.OnPickableInteracleHoverRemove += HandleOnPlayerHoverRemove;
        GameplayEvents.OnPickableInteracleSelect += HandleOnPlayerSelected;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnPickableInteracleHover -= HandleOnPlayerHover;
        GameplayEvents.OnPickableInteracleHoverRemove -= HandleOnPlayerHoverRemove;
        GameplayEvents.OnPickableInteracleSelect -= HandleOnPlayerSelected;
    }

    protected virtual void HandleOnPlayerHover(PickableInteractable pickableInteractable)
    {
        if (pickableInteractable != this)
        {
            HandleOnPlayerHoverRemove();
            return;
        }
        m_message.text = MESSAGE_TEXT_PREFIX + $"{InputConfig.Instance.interactableKey}" + MESSAGE_TEXT_SUFFIX;
        m_message.gameObject.SetActive(true);
    }

    protected virtual void HandleOnPlayerHoverRemove()
    {
        m_message.gameObject.SetActive(false);
    }

    protected abstract void HandleOnPlayerSelected(PickableInteractable pickableInteractable);
}