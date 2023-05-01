using UnityEngine;

public class PreviewEntity : MonoBehaviour
{
    public PreviewStatus Status { get; private set; }
    private MeshRenderer[] _meshRenderers;
    private PlacementToolSettings _placementToolSettings;

    private void Start()
    {
        _placementToolSettings = SettingsManager.PlacementToolSettings;
        _meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        SetPreviewStatus(Status);
    }
    public void SetPreviewStatus(PreviewStatus newStatus)
    {
        Status = newStatus;
        if (_meshRenderers == null)
            return;
        switch (Status)
        {
            case PreviewStatus.Default:
                SetDefaultMaterial();
                break;
            case PreviewStatus.Valid:
                SetValidMaterial();
                break;
            case PreviewStatus.Invalid:
                SetInvalidMaterial();
                break;
        }
    }


    private void SetDefaultMaterial()
    {
        foreach (var meshRenderer in _meshRenderers)
        {
            int materialCount = meshRenderer.materials.Length;
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < materialCount; i++)
            {
                materials[i] = _placementToolSettings.NeutralPreviewMaterial;
            }
            meshRenderer.materials = materials;
        }
    }

    private void SetValidMaterial()
    {
        foreach (var meshRenderer in _meshRenderers)
        {
            int materialCount = meshRenderer.materials.Length;
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < materialCount; i++)
            {
                materials[i] = _placementToolSettings.ValidPreviewMaterial;
            }
            meshRenderer.materials = materials;
        }
    }

    private void SetInvalidMaterial()
    {
        foreach (var meshRenderer in _meshRenderers)
        {
            int materialCount = meshRenderer.materials.Length;
            Material[] materials = meshRenderer.materials;
            for (int i = 0; i < materialCount; i++)
            {
                materials[i] = _placementToolSettings.InvalidPreviewMaterial;
            }
            meshRenderer.materials = materials;
        }
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, 60f);
    }

    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, -60f);
    }
}
public enum PreviewStatus
{
    Default,
    Valid,
    Invalid
}