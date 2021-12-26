# Interactable System Tutorial

This tutorial will explain how to develop a simple interactable system to be used in Unity.

## 1. Download Unity Starter Assets and open the `Playground` scene

To begin with download Unity's [Starter Assets - Third Person Character Controller](https://assetstore.unity.com/packages/essentials/starter-assets-third-person-character-controller-196526).

We will be using the scene and player prefab included in this package to demonstrate this system's functionality.

After fully importing and setting up the package, open the `Playground` scene found inside `Assets/StarterAssets/ThirdPersonController/Scenes/` called `Playground.unity`. 


## 2. Setup the interactable object
1. Add an 3D Sphere named `PickupableBall`
- connect a `Box Collider` component with `Is Trigger` ticked, and resize the collider size 3 on all axis, this will act as an area where the interactable is avaliable to the player.

![Ball Setup](https://user-images.githubusercontent.com/79928221/147372733-85e9f7da-d52e-406f-83ce-3773dae722b9.PNG)


2. Create a script called `PickupItem` we will be using this to script behaviour on the interact of this object.
- Add the following code to the script, this will destroy the object when Pickup is called by the interact event
```
public class PickupItem : MonoBehaviour
{
    public void Pickup()
    {
        Destroy(gameObject);
    }
}
```
- Connect this script to the interactable object


## 3. Add the Interactable functionality

Create a new script called `Interactable`.

1. First we need to declare our variables 
- Add a `UnityEvent` called `interactFunction` for referencing functionality to be called on the interact action, **making sure you add _`using UnityEngine.Events;`_ to the beginning of your script for this to work.**
- A `string` called `interactActionMsg` for setting the action description.
- And a `bool` called `allowLimitedInteract` for setting whether the interactable object will accept interaction from limited interactors (more on this later)
```
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] UnityEvent interactFunction;
    [SerializeField] string interactActionMsg;
    [SerializeField] bool allowLimitedInteract = true;
```

2. Next we need to add some methods
- Add `public void CallInteractFunction(){}` with `interactFunction.Invoke();` inside, this method will be called to execute the referenced interact functionality
- Add `public string GetActionMsg(){}` with `return interactActionMsg;` inside, this can be called to get the interact action's description message, for example this can be used on UI to describe to the player what they will preforming on interaction.
```
public void CallInteractFunction()
    {
        interactFunction.Invoke();
    }
    public string GetActionMsg()
    {
        return interactActionMsg;
    }
```

3. Following we need to add some on trigger methods
- Add `private void OnTriggerStay(Collider other){}`, inside starting with an if statement checking whether the object/player inside the trigger has a `PlayerInteract` component/script attached (more on this script later)
  - inside the if statement starting by declaring a variable `PlayerInteract targetPlayer = other.GetComponent<PlayerInteract>();` referencing the `PlayerInteract` component of the object/player currently in the trigger
  - next another if checking whether allowLimitedInteract is false and the `targetPlayer.limitedInteract` is true, if true returning out of the trigger method, stopping further execution.
  - again an if checking the `targetPlayer.IsInteractableSet()` is false to prevent overwriting another interactable set to the object/player, if true calling `targetPlayer.SetInteractable(this)` to set the target object/player interactable to this instance/object 
- Then add `private void OnTriggerExit(Collider other){}`, inside starting with an if statement checking whether the object/player leaving the trigger again has a PlayerInteract component/script
  - inside the if `other.GetComponent<PlayerInteract>().SetInteractable(null);` setting the target object/player's current interact instance refernce to null (nothing)
```
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
```

4. Finally add the `Interactable` script to the interactable object we previously setup `PickupableBall`
- Also reference the `PickupBall` and the `Pickup` method listed under `PickupItem` under `Interact Function ()` found inside the `Interactable` inspector properties

Your inspector setup for the interactable should look similar to this

![Ball Inspector Setup](https://user-images.githubusercontent.com/79928221/147373102-44179f74-5462-464f-8189-94a59c364f36.PNG)


## 4. Setup interactable funtionality on the player

1. To begin with we need to add input for the interact action
- Find and open the starter asset's input map inside `Assets/StarterAssets/InputSystem` called `StarterAssets.inputactions`
  - Inside the input map goto the `Player` action map, then add a new action called `Interact`
  - Inside the new action add a binding set to the `E` key

![InputMap Setup](https://user-images.githubusercontent.com/79928221/147373383-39bc9323-7bf9-4f49-8467-1b2034350384.PNG)

- Next find and open the `StarterAssetsInputs` script again inside `Assets/StarterAssets/InputSystem`
  - Add a variable `public bool interact;`
```
public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool interact; <-----
```
  - Add a new method under `#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED`
```
public void OnInteract(InputValue value)
  {
    InteractInput(value.isPressed);
  }
```
  - Add another method...
```
public void InteractInput(bool newInteractState)
  {
    interact = newInteractState;
  }
```

2. Next we need to create a script for the player side interactable functionality called `PlayerInteract`
- Starting off this script we need to declare our variables
  - `Interactable currInteractable;` this will hold the current interactable the player/object is nearby (inside its trigger)
  - `[SerializeField] float interactTimeout;` this set as a SerializeField to allow inspector adjustment of the player's interact timeout
  - `public bool limitedInteract;` this can be set in the inspector and utilised by other scritps to enable/disable limited interact capabilities on the player (for example if the script is used on a drone this can be set to limit what the drone can interact with)
  - `float interactTimer;` used for a counter on the interact timeout

- Next we need to add our methods
  - To begin with ad an Update method with an if statement inside checking if the interactTimer is bigger than 0, if true counting down the timer, this will be used for the timeout functionality
```
private void Update()
  {
    if (interactTimer > 0) interactTimer -= Time.deltaTime;
  }
```
  - Next `public bool IsInterctableSet()`, inside returning true if there is an Interactable referenced in `currInteractable`, if there is not returning false. This will be used by the `Interactable` script to check if the player has another Interactable already refernced to use
```
public bool IsInteractableSet()
  {
    return currInteractable != null ? true : false;
  }
```
  - Following `public void SetInteractable(Interactable interactable)`, inside setting the argument `interactable` to `currInteractable`. This will refernece an `Interactable` the player can use
```
public void SetInteractable(Interactable interactable)
  {
    currInteractable = interactable;
  }
```
  - Finally `public void Interact()`, inside an if statement checking if there is an `Interactable` referenced in `currInteractable` and if the interact timeout timer is finished, if true adding the set timeout time to the timeout timer and calling the referenced `Interactable`'s `CallInteractFunction()` to perform the interact behaviour. This will be called by a player controller to execute an interact action
```
public void Interact()
  {
    if(currInteractable != null && interactTimer <= 0)
      {
        interactTimer = interactTimeout;
        currInteractable.CallInteractFunction();
      }
        
   }
 ```
 3. Following we need to modify the ThirdPersonController player controller script to allow the player to make use of the Interactable system
- First add a new variable `public UnityEvent interactableMethod`. This will be used in the inspector to reference the Interact method we created in the `PlayerInteract` script
  - Again make sure to add 'using UnityEngine.Events;' to the top of the script to be able to use UnityEvents
- Next add a new method `void Interact()`, inside adding `interactableMethod.Invoke()` to call its referenced method


4. Finally we need to apply our newly created functionality to our player
- First add the PlayerInteract script to our player game object
- Next under the ThirdPersonController inspector properties on our player, find the InteractableMethod unity event property/variable we added to the component/script, select + and reference the Player object along with the Interact function/method

![Player Setup](https://user-images.githubusercontent.com/79928221/147373583-3ba8aff1-0857-41bb-a393-03951b6c14aa.png)

Test by running the game, moving the player near the ball and pressing E to interact with it
