using UnityEngine;

public class PathTool : GridTool
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