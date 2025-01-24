using DG.Tweening;
using System.Collections;
using UnityEngine;

public class OrnamentLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float minForce = 5f; // Минимальная сила броска
    public float maxForce = 10f; // Максимальная сила броска
    public float minAngle = 30f; // Минимальный угол броска в градусах
    public float maxAngle = 60f; // Максимальный угол броска в градусах

    public CatchToys[] ornaments; // Массив Rigidbody игрушек
    private int currentIndex = 0; // Индекс текущей игрушки


    public float shakeDuration = 0.5f; // Длительность тряски
    public float shakeStrength = 10f; // Сила тряски
    public float tiltAngle = 10f; // Угол наклона
    public int vibrato = 10; // Количество колебаний

    private Quaternion initialRotation; // Начальный поворот ёлки

    public int CounterCatchToy;

    public GameObject FinishUi;
    public AudioSource WinSound;

    private bool _working = true;

    private void Awake()
    {
        ornaments = GetComponentsInChildren<CatchToys>();
    }
    void Start()
    {
        initialRotation = transform.rotation;

        // Находим все Rigidbody, прикрепленные к дочерним объектам
       
    }

    public void LaunchNextOrnament()
    {
        if (_working)
        {
            // Проверяем, есть ли ещё игрушки для запуска
            if (currentIndex < ornaments.Length)
            {
                CatchToys ornament = ornaments[currentIndex];
                ornament.EnabledTach();
                ornament.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                if (ornament != null)
                {
                    // Отключаем кинематик
                    Rigidbody rb = ornament.GetComponent<Rigidbody>();
                    rb.isKinematic = false;

                    ornament.transform.parent = null;

                    // Рассчитываем случайную силу
                    float force = Random.Range(minForce, maxForce);

                    // Генерируем случайное направление в пределах полусферы
                    Vector3 launchDirection = Random.insideUnitSphere;
                    launchDirection.z = Mathf.Abs(launchDirection.z); // Убедимся, что Z всегда положительный
                    launchDirection.y = Mathf.Abs(launchDirection.y); // Убедимся, что Y всегда положительный

                    // Нормализуем вектор направления и применяем силу
                    rb.AddForce(launchDirection.normalized * force, ForceMode.Impulse);
                    ShakeTree();
                }

                // Переходим к следующей игрушке
                currentIndex++;
            }

            else
            {
                StartCoroutine(WinDelay());
            }
        }
    }


    public void ShakeTree()
    {
        // Создаем последовательность
        Sequence shakeSequence = DOTween.Sequence();

        // Добавляем тряску в виде случайных наклонов
        shakeSequence.Append(transform.DOShakeRotation(shakeDuration, new Vector3(tiltAngle, tiltAngle, tiltAngle), vibrato, 90));

        // Возвращаем ёлку в исходное положение
        shakeSequence.Append(transform.DORotateQuaternion(initialRotation, 0.2f).SetEase(Ease.OutQuad));
    }

    private IEnumerator WinDelay()
    {
        // Если корутина уже выполняется, выходим
        if (!_working)
            yield break;

        yield return new WaitForSeconds(2);

        _working = false; // Останавливаем дальнейшие действия
        FinishUi.SetActive(true);
        foreach(CatchToys t in ornaments)
        {
            if(t != null)
                Destroy(t.gameObject);
        }
            

        // Проверяем, что звук не проигрывается, прежде чем запускать
        if (!WinSound.isPlaying)
        {
            WinSound.Play();
        }
    }
}
