using DG.Tweening;
using UnityEngine;

public class ToysMover : MonoBehaviour
{
    public float moveDistance = 3f; // Расстояние, на которое игрушка двигается вперёд
    public float moveDuration = 0.5f; // Длительность перемещения

    public LayerMask layerMask;

    private bool isMoving = false; // Флаг, чтобы избежать одновременных движений

    private DragDrop _dragDrop;

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
                if (hit.collider.GetComponent<Slot>() == true)
                {
                    if (hit.collider.GetComponent<DragDrop>() == false)
                    {
                        if (_dragDrop.Drop == true && (hit.collider.GetComponent<Slot>().IsEmptySlot() == true))
                        {
                            MoveForward();
                        }


                    }
                }
            }

        }
    }

    private void MoveForward()
    {
        isMoving = true;
        Vector3 targetPosition = transform.position + -transform.forward * moveDistance;

        // Двигаем объект вперёд с помощью DoTween
        transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            isMoving = false; // Сбрасываем флаг по завершении движения
        });
    }


}
