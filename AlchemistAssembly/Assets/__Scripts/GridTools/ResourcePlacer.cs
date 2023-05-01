using UnityEngine;

public class ResourcePlacer : GridTool
{
    protected override void SubscribeToActions()
    {
    }

    protected override void UnsubscribeFromActions()
    {
    }
    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}