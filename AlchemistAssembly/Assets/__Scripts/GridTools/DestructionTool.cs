using UnityEngine;
using UnityEngine.InputSystem;

public class DestructionTool : GridTool
{
    private Destructable _currentObject;
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_currentObject != null)
            {
                Destroy(_currentObject.gameObject);
                _currentObject = null;
            }
        }
    }
    public void ExtraMouseCycle()
    {
        CheckForObject();
    }

    private Destructable CheckForObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(PlacementController.Instance.MousePosInput);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f))
        {
            return hit.collider.GetComponentInParent<Destructable>();
        }
        return null;
    }


    private void FixedUpdate()
    {
        Destructable newDestructable = CheckForObject();
        if (newDestructable != _currentObject)
        {
            if (_currentObject != null)
                _currentObject.UnHighlight();
            _currentObject = newDestructable;
            if (_currentObject != null)
                _currentObject.Highlight();
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