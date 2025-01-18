using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabLvls;

    [SerializeField] private Camera _camera;

    private GameObject _lvlPrefab;
    private int _index;

    private void Awake()
    {
      //  _lvlPrefab = Instantiate(_prefabLvls[_index]);
        Initialize();
    }
    private void Initialize()
    {
        CanvasMake canvas = FindObjectOfType<CanvasMake>();

        canvas.GetComponent<Canvas>().worldCamera = _camera;
    }

    public void StartNextLvl()
    {
        Destroy(_lvlPrefab);
        _index++;
        Instantiate(_prefabLvls[_index]);
        Initialize();
    }

    public void SubscripesButton(Button but)
    {
        but.onClick.AddListener(StartNextLvl);
    }
}
