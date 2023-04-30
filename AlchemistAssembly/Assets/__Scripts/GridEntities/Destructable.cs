using UnityEngine;

public class Destructable : MonoBehaviour
{
    private MeshRenderer[] _meshRenderer;
    private Color[][] _originalColors;

    private void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _originalColors = new Color[_meshRenderer.Length][];
        for (int i = 0; i < _meshRenderer.Length; i++)
        {
            var materials = _meshRenderer[i].materials;
            _originalColors[i] = new Color[materials.Length];
            for (int j = 0; j < materials.Length; j++)
            {
                _originalColors[i][j] = materials[j].color;
            }
        }
    }


    public void HighLight()
    {
        for (int i = 0; i < _meshRenderer.Length; i++)
        {
            var materials = _meshRenderer[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].color = Color.red;
            }
        }
    }

    public void UnHighlight()
    {
        for (int i = 0; i < _meshRenderer.Length; i++)
        {
            var materials = _meshRenderer[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j].color = _originalColors[i][j];
            }
        }
    }
}