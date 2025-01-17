using DG.Tweening;
using UnityEditor.PackageManager;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    [SerializeField] private ParticleSystem _deadParticle;
    [SerializeField] private AudioSource _take;
    [SerializeField] private AudioSource _drop;
    [SerializeField] private AudioSource _destroy;

    Vector3 offset;
    public string destinationTag = "DropArea";
    public bool IsDragging = false;

    private Vector3 originalPosition;  // Переменная для запоминания исходной позиции объекта
    private Transform originalParent;  // Переменная для запоминания исходного родителя

    private Vector3 startScale;
    private Vector3 _startPosition;

    public bool _isStart = true;


    private void Start()
    {
        startScale = transform.localScale;
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStart)
            StartPositionForItem(other);
    }

    void OnMouseDown()
    {
        // Debug.Log("Я взял " + name);
        _take.Play();
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);

        // Запоминаем исходную позицию и родителя объекта перед его перемещением
        originalPosition = transform.position;
        originalParent = transform.parent;

        transform.parent = null;
        offset = transform.position - MouseWorldPosition();
        transform.GetComponent<Collider>().enabled = false;

        // Устанавливаем Z-координату объекта в 0, чтобы он двигался только по осям X и Y
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

    }

    void OnMouseDrag()
    {
        Vector3 newPosition = MouseWorldPosition() + offset;

        // Устанавливаем Z-координату в 0, чтобы объект не двигался по оси Z
        newPosition.z = 3f;

        transform.position = newPosition;
        IsDragging = true;
    }

    void OnMouseUp()
    {
        // Debug.Log("Я отпустил шарик");
        _drop.Play();
        transform.localScale = startScale;
        IsDragging = false;

        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit hitInfo;

        // Проверяем попадание объекта в DropArea
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
        {
            if (hitInfo.transform.tag == destinationTag)
            {
                // Если объект попадает в целевую зону, то устанавливаем его позицию на позицию цели
                if (hitInfo.collider.GetComponent<Slot>().IsEmptySlot() == true)
                {
                    transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, 3f);
                    transform.parent = hitInfo.transform;  // Устанавливаем родителя в целевой объект
                    RestartPositionWithNod();
                }
                else if (hitInfo.collider.GetComponent<Slot>().IsEmptySlot() == false)
                {
                    Debug.LogError("Слот занят");
                    RestartPosotion();
                }
            }
            else
            {
                // Если объект не в целевой зоне, возвращаем его на исходную позицию и восстанавливаем родителя
                RestartPosotion();
            }
        }
        else
        {
            // Если объект не попал в какую-то область, возвращаем его на исходную позицию и восстанавливаем родителя
            RestartPosotion();
        }

        // Восстанавливаем collider объекта
        transform.GetComponent<Collider>().enabled = true;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
    private void RestartPosotion()
    {
        transform.position = originalPosition;
        transform.parent = originalParent;

        RestartPositionWithShake();
    }

    private void RestartPositionWithNod()
    {
        // Текущая позиция объекта
        Vector3 currentPosition = transform.position;

        // Создаем последовательность анимации
        DG.Tweening.Sequence nodSequence = DOTween.Sequence();

        // Опускаем объект немного вниз
        nodSequence.Append(transform.DOMoveY(currentPosition.y - 0.6f, 0.1f).SetEase(Ease.InOutSine));

        // Добавляем паузу в нижней точке
        nodSequence.AppendInterval(0.1f);

        // Резко возвращаем объект обратно на текущую позицию
        nodSequence.Append(transform.DOMoveY(currentPosition.y, 0.05f).SetEase(Ease.OutSine));
    }

    private void RestartPositionWithShake()
    {
        // Сохраняем начальную позицию
        Vector3 originalPosition = transform.position;

        // Создаем последовательность анимации
        DG.Tweening.Sequence shakeSequence = DOTween.Sequence();

        // Возвращаем объект в начальную позицию
        shakeSequence.Append(transform.DOMove(originalPosition, 0).SetEase(Ease.OutSine));

        // Добавляем тряску после возвращения
        shakeSequence.Append(transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10, randomness: 90, snapping: false, fadeOut: true));
    }

    public void DeadProcess()
    {
        // Сохраняем начальный размер объекта
        Vector3 originalScale = transform.localScale;

        // Целевой размер при уменьшении (половина от текущего размера)
        Vector3 reducedScale = originalScale * 0.2f;

        // Создаем последовательность анимации
        Sequence deadSequence = DOTween.Sequence();

        // Добавляем задержку перед началом анимации
        deadSequence.AppendInterval(0.3f); // Подстройте время под ваши нужды

        // Уменьшаем объект до половины размера
        deadSequence.Append(transform.DOScale(reducedScale, 0.2f).SetEase(Ease.InOutSine));

        // Ждем 0.1 секунды в уменьшенном состоянии
        deadSequence.AppendInterval(0.1f);

        // Возвращаем объект к исходному размеру
        deadSequence.Append(transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.InOutSine));


        deadSequence.OnComplete(() =>
        {
            // Создаем частицу
            Instantiate(_deadParticle, transform.position, Quaternion.identity);
            _destroy.Play();
            // Уничтожаем объект
            Destroy(gameObject);
        });
    }

    private void StartPositionForItem(Collider collider)
    {
        if (collider.GetComponent<Slot>())
        {
            Debug.Log("y");
            transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, 3f);
            transform.parent = collider.transform;  // Устанавливаем родителя в целевой объект
            _isStart = false;
        }
    }

}
