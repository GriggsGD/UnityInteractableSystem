using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] UnityEvent interactFunction;
    [SerializeField] string interactActionMsg;
    [SerializeField] bool allowLimitedInteract = true;
    public void CallInteractFunction()
    {
        interactFunction.Invoke();
    }
    public string GetActionMsg()
    {
        return interactActionMsg;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerInteract>())
        {
            PlayerInteract targetPlayer = other.GetComponent<PlayerInteract>();
            if (!allowLimitedInteract && targetPlayer.limitedInteract) return;
            if(!targetPlayer.IsInteractableSet())
                targetPlayer.SetInteractable(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInteract>()) { other.GetComponent<PlayerInteract>().SetInteractable(null); }
    }
}
