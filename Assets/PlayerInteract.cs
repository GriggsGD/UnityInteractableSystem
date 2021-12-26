using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Interactable currInteractable;

    [Header("Interact Properties")]
    [SerializeField] float interactTimeout = .3f;
    public bool limitedInteract = false;

    float interactTimer;
    private void Update()
    {
        if (interactTimer > 0) interactTimer -= Time.deltaTime;
    }
    public bool IsInteractableSet()
    {
        return currInteractable != null ? true : false;
    }
    public void SetInteractable(Interactable interactable)
    {
        currInteractable = interactable;
    }
    public void Interact()
    {
        if(currInteractable != null && interactTimer <= 0)
        {
            interactTimer = interactTimeout;
            currInteractable.CallInteractFunction();
        }
        
    }
}
