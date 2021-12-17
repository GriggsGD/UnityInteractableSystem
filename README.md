# UnityInteractableSystem
A simple interact system for unity

## Contents

This system is made from the following components/scripts:
- ['Interactable']
- ['PlayerInteract']

### Interactable

This component/script handles the connected object's interact functionality

#### Setup
1. Add the Interactable component/script to your desired GameObject
2. Add a collider to the same GameObject with is Trigger ticked, scale as desired, this will act as a zone where the interact is avaliable
3. Under the Interactable component you can find "Interact Function", add a function item, connect your desired gameobject to control inside the (object) box, then select a function to call in the drop down to preform your desired action
4. In "Interact Ui Msg" text box you can set an action string that can be pulled from the script for a UI element, you can call GetActionMsg() to pull this value, for example you could code "Press E to " + interactable.GetActionMsg() on a UI element to display the current interactable action description
5. Limited Interact, can be set/ticked to only allow interacters with full interact capability to interact with your object
