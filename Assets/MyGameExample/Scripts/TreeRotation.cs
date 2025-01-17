using UnityEngine;

public class TreeRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // �������� ��������

    private bool isDragging = false;
    private Vector2 lastMousePosition;

    void Update()
    {
        if (isDragging)
        {
            
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        // ��������� ������� ������� ���� �� ����� ��������������
        Vector2 currentMousePosition = Input.mousePosition;

        // ������� ����� ������� � ��������� �������� ����
        Vector2 delta = currentMousePosition - lastMousePosition;

        // ������� ���� ������ ��� Y �� ��������� ��������������� �������� ����
        transform.Rotate(0, -delta.x * rotationSpeed * Time.deltaTime, 0f, Space.World);

        // ��������� ��������� ������� ����
        lastMousePosition = currentMousePosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
