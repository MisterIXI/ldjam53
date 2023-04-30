using UnityEngine;

public class PathTool : GridTool
{

    
    protected override void SubscribeToActions()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnsubscribeFromActions()
    {
        throw new System.NotImplementedException();
    }
    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }
}