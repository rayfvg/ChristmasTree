using DG.Tweening;
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

    private bool _working = true;


    void Start()
    {
        initialRotation = transform.rotation;

        // Находим все Rigidbody, прикрепленные к дочерним объектам
        ornaments = GetComponentsInChildren<CatchToys>();
    }

    private void Update()
    {
        if(CounterCatchToy >= 7)
        {
            _working = false;
            FinishUi.SetActive(true);
        }
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

    public void AddGab()
    {
        CounterCatchToy++;
        Debug.Log(CounterCatchToy);
    }
}
