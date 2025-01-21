using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToysMover : MonoBehaviour
{
    public float moveDistance = 3f; // Расстояние, на которое игрушка двигается вперёд
    public float moveDuration = 0.5f; // Длительность перемещения
    public LayerMask layerMask;

    private bool isMoving = false; // Флаг, чтобы избежать одновременных движений
    private DragDrop _dragDrop;
    public Slot _targetSlot; // Слот, в который перемещается объект

    private void Start()
    {
       _dragDrop = GetComponent<DragDrop>();
        _dragDrop.IsEnableDrag = false;
    }

    void Update()
    {
        // Если игрушка не двигается, проверяем луч и начинаем движение
        if (!isMoving)
        {
            Ray rayDragAndDrop = new Ray(transform.position, -transform.forward);
            Ray raySlot = new Ray(transform.position, -transform.forward);
            RaycastHit hit;

            Debug.DrawRay(transform.position, -transform.forward * moveDistance);

            // Проверяем, есть ли объект с DragDrop впереди
            if (Physics.Raycast(rayDragAndDrop, out hit, moveDistance))
            {
                if (hit.collider.GetComponent<DragDrop>() == true)
                {
                    _dragDrop = hit.collider.GetComponent<DragDrop>();
                }
            }

            if (Physics.Raycast(raySlot, out hit, moveDistance, layerMask))
            {
                Slot slot = hit.collider.GetComponent<Slot>();
                _targetSlot = slot;
                if (slot != null && _dragDrop != null)
                {
                    if (_dragDrop.Drop == true && slot.IsEmptySlot() == true)
                    {
                       // Сохраняем ссылку на слот
                        StartCoroutine(DelayedSlotCheck());
                    }
                }
            }
        }


    }
    private System.Collections.IEnumerator DelayedSlotCheck()
    {
        yield return new WaitForSeconds(0.5f);

        // Проверяем, что слот пустой и переменная Drop всё ещё true
        if (_dragDrop != null && _dragDrop.Drop == true && _targetSlot != null && _targetSlot.IsEmptySlot() == true)
        {
            MoveForward();
        }
    }

    private System.Collections.IEnumerator DelayedDeadMover()
    {
        yield return new WaitForSeconds(1);

        isMoving = true;
        Vector3 targetPosition = _targetSlot.transform.position;
        Debug.Log("я зашел в метод");
        // Двигаем объект в позицию слота с помощью DoTween
        transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            isMoving = false; // Сбрасываем флаг по завершении движения
            StartPositionForItem(_targetSlot.GetComponent<Collider>()); // Закрепляем объект в слоте
        });
    }

    private void MoveForward()
    {
        isMoving = true;
        Vector3 targetPosition = _targetSlot.transform.position;
        Debug.Log("я зашел в метод");
        // Двигаем объект в позицию слота с помощью DoTween
        transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            isMoving = false; // Сбрасываем флаг по завершении движения
            StartPositionForItem(_targetSlot.GetComponent<Collider>()); // Закрепляем объект в слоте
        });
    }

    public void MoveForwardDelay()
    {
        StartCoroutine(DelayedDeadMover());
    }

    private void StartPositionForItem(Collider collider)
    {
        if (collider.GetComponent<Slot>())
        {
            Debug.Log("Закрепляем в слоте");
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = collider.transform.position; // Устанавливаем точную позицию
            transform.parent = collider.transform; // Устанавливаем родителя
            GetComponent<DragDrop>().IsEnableDrag = true;
        }
    }
}
