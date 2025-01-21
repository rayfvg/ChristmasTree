using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatWalker : MonoBehaviour
{
    public TreeDropper tree;

    public float areaSize = 10f; // Размер области для случайных точек
    public float speed = 2f; // Начальная скорость движения
    public float speedBoost = 1.5f; // Увеличение скорости после клика
    public int maxClicks = 3; // Количество кликов до победы
    public float stopTime = 3f; // Время остановки перед возобновлением движения

    private Vector3 targetPoint; // Текущая целевая точка
    private bool isMoving = true; // Флаг движения
    private int clickCount = 0; // Количество кликов на кошку

    private CatAnimation catAnimator; // Анимации кошки

    public Transform FinishTarget;
    private bool _finish = false;

    public bool EnabledClick = true;

    private void Start()
    {
        catAnimator = GetComponent<CatAnimation>();
        GenerateNewTargetPoint();
    }

    private void Update()
    {
        if (isMoving)
            MoveToTarget();

        if (_finish)
            MoveToFinish();
    }

    private void MoveToTarget()
    {
        if (_finish == true)
            return;

        // Движение к целевой точке
        catAnimator.StartWalk();

        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Сглаженный поворот в сторону движения
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // Проверка достижения целевой точки
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            GenerateNewTargetPoint();
        }
    }

    private void GenerateNewTargetPoint()
    {
        const float minDistance = 2f; // Минимальное расстояние от текущей позиции до новой точки
        int maxAttempts = 100; // Максимальное количество попыток найти подходящую точку

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(-areaSize / 2, areaSize / 2);
            float randomZ = Random.Range(-areaSize / 2, areaSize / 2);
            Vector3 potentialPoint = new Vector3(randomX, transform.position.y, randomZ);

            // Проверяем, находится ли новая точка дальше минимального расстояния
            if (Vector3.Distance(transform.position, potentialPoint) >= minDistance)
            {
                targetPoint = potentialPoint;
                return;
            }
        }

        // Если после всех попыток точка не найдена, используем текущее значение targetPoint
        Debug.LogWarning("Не удалось найти достаточно удалённую точку. Использую текущее значение.");
    }

    private void OnMouseDown()
    {
        if (!isMoving) return; // Игнорируем клики, если кошка уже остановлена
        if (EnabledClick == false) return;

        clickCount++;
        StopAllCoroutines(); // Останавливаем текущие корутины, если есть
        StartCoroutine(HandleClick());
    }

    private IEnumerator HandleClick()
    {
        // Остановка кошки
        EnabledClick = false;
        isMoving = false;
        catAnimator.StopWalk();

        // Ждем заданное время
        yield return new WaitForSeconds(stopTime);

        // Увеличение скорости и возобновление движения
        speed += speedBoost;
        isMoving = true;

        // Проверка на победу
        if (clickCount >= maxClicks)
        {
            _finish = true;
        }


        yield return new WaitForSeconds(5);
        EnabledClick = true;
    }

    private void OnDrawGizmos()
    {
        // Рисуем область, где генерируются точки
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize, 0.1f, areaSize));
    }

    public void DropTree() => tree.DropTree();

    private void MoveToFinish()
    {
        catAnimator.StartWalk();

        // Движение к финишной точке
        Vector3 direction = (FinishTarget.position - transform.position).normalized;
        transform.position += direction * 8 * Time.deltaTime;

        // Сглаженный поворот в сторону движения
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // Проверка достижения финишной точки
        if (Vector3.Distance(transform.position, FinishTarget.position) < 0.2f)
        {
            Animator animator = GetComponent<Animator>();
            animator.applyRootMotion = false;

            // Совпадение текущей позиции с началом анимации
            animator.MatchTarget(
                transform.position,
                transform.rotation,
                AvatarTarget.Root,
                new MatchTargetWeightMask(Vector3.one, 1f),
                0f,
                0.1f
            );

            transform.position = animator.transform.position;
            transform.rotation = animator.transform.rotation;

            _finish = false; // Останавливаем движение
            isMoving = false;
            Debug.Log("Готов к анимации");

          

            catAnimator.Attack();
        }
    }
}