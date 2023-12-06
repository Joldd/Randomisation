using UnityEngine;

public class OrthographicSizeScaler : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Vector2 _referenceResolution;
        
    void Awake()
    {
        var referenceRatio = _referenceResolution.x / _referenceResolution.y;
        var currentRatio = (float) Screen.width / Screen.height;

        if (currentRatio >= referenceRatio)
            return;
            
        _camera.orthographicSize = referenceRatio / currentRatio * _camera.orthographicSize;
    }
}