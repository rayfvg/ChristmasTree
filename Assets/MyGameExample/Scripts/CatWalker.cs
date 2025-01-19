using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWalker : MonoBehaviour
{
    public Transform[] patrolPoints; // ����� ��������������
    public float speed = 2f; // �������� ��������
    public float rotationSpeed = 5f; // �������� ��������
    public float speedBoost = 1.5f; // ���������� �������� ��� �����

    private int currentPointIndex = 0; // ������� �����
    private bool isPatrolling = true; // ���� ��������������

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

        // ��������� � ������� �����
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // �������������� � ����������� ��������
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ���� �������� ������� �����, ��������� � ���������
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // ����� ��� ��������� ��������������
    public void StopPatrolling()
    {
        _animationCat.StopWalk();
        isPatrolling = false;
    }

    // ����� ��� ����������� ��������������
    public void StartPatrolling()
    {
        _animationCat.StartWalk();
        isPatrolling = true;
    }

    private void OnMouseDown()
    {
        // ����������� �������� ��� ����� �� ����
        speed *= speedBoost;
        StartCoroutine(ResetSpeedAfterDelay(2f)); // ���������� �������� ����� 2 �������
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speed /= speedBoost;
    }
}