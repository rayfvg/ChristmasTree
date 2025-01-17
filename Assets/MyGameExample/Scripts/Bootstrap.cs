using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject _prefabLvl1;


    [SerializeField] private Camera _camera;

    private CanvasMake _canvas;

    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        Instantiate(_prefabLvl1);

        _canvas = FindObjectOfType<CanvasMake>();
        _canvas.GetComponent<Canvas>().worldCamera = _camera;
    }
}
