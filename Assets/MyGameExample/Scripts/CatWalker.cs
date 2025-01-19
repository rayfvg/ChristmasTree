using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWalker : MonoBehaviour
{
    public Transform[] patrolPoints; // Точки патрулирования
    public float speed = 2f; // Скорость движения
    public float rotationSpeed = 5f; // Скорость поворота
    public float speedBoost = 1.5f; // Увеличение скорости при клике

    private int currentPointIndex = 0; // Текущая точка
    private bool isPatrolling = true; // Флаг патрулирования

    private CatAnimation _animationCat;

    private void Awake()
    {
        _animationCat = GetComponent<CatAnimation>();
    }

    private void Update()
    {
        if (isPatrolling && patrolPoints.Length > 0)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        _animationCat.StartWalk();

        Transform targetPoint = patrolPoints[currentPointIndex];

        // Двигаемся к текущей точке
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Поворачиваемся в направлении движения
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Если достигли текущей точки, переходим к следующей
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // Метод для остановки патрулирования
    public void StopPatrolling()
    {
        _animationCat.StopWalk();
        isPatrolling = false;
    }

    // Метод для продолжения патрулирования
    public void StartPatrolling()
    {
        _animationCat.StartWalk();
        isPatrolling = true;
    }

    private void OnMouseDown()
    {
        // Увеличиваем скорость при клике на кота
        speed *= speedBoost;
        StartCoroutine(ResetSpeedAfterDelay(2f)); // Возвращаем скорость через 2 секунды
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speed /= speedBoost;
    }
}