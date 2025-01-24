using UnityEngine;
using UnityEngine.UI;
using YG;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabLvls; // Основной список уровней
    [SerializeField] private GameObject[] _loopLevels; // Список уровней для циклической загрузки
    [SerializeField] private Camera _camera;

    private GameObject _lvlPrefab;
    public int _index;

    private void Awake()
    {
        _index = PlayerPrefs.GetInt("id");
        _lvlPrefab = Instantiate(_prefabLvls[_index]);

        Initialize();
    }

    private void Initialize()
    {
        CanvasMake canvas = FindObjectOfType<CanvasMake>();
        MakeARestartButton restart = FindObjectOfType<MakeARestartButton>();

        restart.GetComponent<Button>().onClick.AddListener(RestartLevel);
        canvas.GetComponent<Canvas>().worldCamera = _camera;
    }

    public void StartNextLvl()
    {
        Destroy(_lvlPrefab);

        // Переход на следующий уровень из основного списка
        _index++;
        if(_index >= _prefabLvls.Length)
        {
            _index = 9;
            Debug.Log("NewGame");
        }

        PlayerPrefs.SetInt("id", _index);
        _lvlPrefab = Instantiate(_prefabLvls[_index]);

        Initialize();

        YandexGame.FullscreenShow();
    }

    public void SubscripesButton(Button but)
    {
        but.onClick.AddListener(StartNextLvl);
    }

    public void SubscribeLoseButton(Button but)
    {
        but.onClick.AddListener(RestartLevel);
    }

    public void RestartLevel()
    {
        Destroy(_lvlPrefab);

        _lvlPrefab = Instantiate(_prefabLvls[_index]);
        Initialize();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            StopAllSouds();
        }
        else
        {
            PlayAllSounds();
        }
    }

    public void StopAllSouds()
    {
        AudioListener.pause = true;
    }

    public void PlayAllSounds()
    {
        AudioListener.pause = false;
    }
}
