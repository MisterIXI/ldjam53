using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultTool : GridTool
{
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GridTile currentTile = PlacementController.HoveredTile;
            Debug.Log("Trying to interact with tile" + currentTile);
            if (currentTile != null && currentTile.Placeable != null)
            {
                if (currentTile.Placeable is IInteractable interactable)
                {
                    interactable.OnInteract();
                }
            }
        }
    }
    protected override void SubscribeToActions()
    {
        InputManager.OnInteract += OnInteractInput;
    }

    protected override void UnsubscribeFromActions()
    {
        InputManager.OnInteract -= OnInteractInput;
    }
    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}