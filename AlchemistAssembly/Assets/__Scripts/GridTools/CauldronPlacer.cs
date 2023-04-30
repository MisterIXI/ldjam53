using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CauldronPlacer : GridTool
{
    [field: SerializeField] public PreviewEntity CauldronPreview { get; private set; }
    [field: SerializeField] public GameObject CauldronPrefab { get; private set; }

    private float _yVelocity;
    public override void Activate()
    {
        base.Activate();
        CauldronPreview.gameObject.SetActive(true);
        CauldronPreview.transform.eulerAngles = new Vector3(0, PlacementController.Instance.RotationAngle, 0);

    }
    private void Update()
    {
        float newYAngle = Mathf.SmoothDampAngle(CauldronPreview.transform.eulerAngles.y, PlacementController.Instance.RotationAngle, ref _yVelocity, _settings.PreviewRotationDuration);
        CauldronPreview.transform.eulerAngles = new Vector3(0, newYAngle, 0);
    }



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