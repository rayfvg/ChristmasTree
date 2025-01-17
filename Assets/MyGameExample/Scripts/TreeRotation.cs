using UnityEngine;

public class TreeRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Скорость вращения

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
        // Обновляем текущую позицию мыши во время перетаскивания
        Vector2 currentMousePosition = Input.mousePosition;

        // Разница между текущей и последней позицией мыши
        Vector2 delta = currentMousePosition - lastMousePosition;

        // Вращаем ёлку вокруг оси Y на основании горизонтального движения мыши
        transform.Rotate(0, -delta.x * rotationSpeed * Time.deltaTime, 0f, Space.World);

        // Обновляем последнюю позицию мыши
        lastMousePosition = currentMousePosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
