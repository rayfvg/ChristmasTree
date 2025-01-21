using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToysMover : MonoBehaviour
{
    public float moveDistance = 3f; // ����������, �� ������� ������� ��������� �����
    public float moveDuration = 0.5f; // ������������ �����������
    public LayerMask layerMask;

    private bool isMoving = false; // ����, ����� �������� ������������� ��������
    private DragDrop _dragDrop;
    public Slot _targetSlot; // ����, � ������� ������������ ������

    private void Start()
    {
       _dragDrop = GetComponent<DragDrop>();
        _dragDrop.IsEnableDrag = false;
    }

    void Update()
    {
        // ���� ������� �� ���������, ��������� ��� � �������� ��������
        if (!isMoving)
        {
            Ray rayDragAndDrop = new Ray(transform.position, -transform.forward);
            Ray raySlot = new Ray(transform.position, -transform.forward);
            RaycastHit hit;

            Debug.DrawRay(transform.position, -transform.forward * moveDistance);

            // ���������, ���� �� ������ � DragDrop �������
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
                       // ��������� ������ �� ����
                        StartCoroutine(DelayedSlotCheck());
                    }
                }
            }
        }


    }
    private System.Collections.IEnumerator DelayedSlotCheck()
    {
        yield return new WaitForSeconds(0.5f);

        // ���������, ��� ���� ������ � ���������� Drop �� ��� true
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
        Debug.Log("� ����� � �����");
        // ������� ������ � ������� ����� � ������� DoTween
        transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            isMoving = false; // ���������� ���� �� ���������� ��������
            StartPositionForItem(_targetSlot.GetComponent<Collider>()); // ���������� ������ � �����
        });
    }

    private void MoveForward()
    {
        isMoving = true;
        Vector3 targetPosition = _targetSlot.transform.position;
        Debug.Log("� ����� � �����");
        // ������� ������ � ������� ����� � ������� DoTween
        transform.DOMove(targetPosition, moveDuration).OnComplete(() =>
        {
            isMoving = false; // ���������� ���� �� ���������� ��������
            StartPositionForItem(_targetSlot.GetComponent<Collider>()); // ���������� ������ � �����
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
            Debug.Log("���������� � �����");
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = collider.transform.position; // ������������� ������ �������
            transform.parent = collider.transform; // ������������� ��������
            GetComponent<DragDrop>().IsEnableDrag = true;
        }
    }
}
