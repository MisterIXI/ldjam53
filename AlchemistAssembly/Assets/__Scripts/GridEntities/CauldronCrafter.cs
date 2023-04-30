using UnityEngine;

public class CauldronCrafter : Placeable, IInteractable
{
  public void OnInteract()
  {
    Debug.Log("Interacted with cauldron");
  }
}