using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatWalker : MonoBehaviour
{
    public TreeDropper tree;

    public float areaSize = 10f; // ������ ������� ��� ��������� �����
    public float speed = 2f; // ��������� �������� ��������
    public float speedBoost = 1.5f; // ���������� �������� ����� �����
    public int maxClicks = 3; // ���������� ������ �� ������
    public float stopTime = 3f; // ����� ��������� ����� �������������� ��������

    private Vector3 targetPoint; // ������� ������� �����
    private bool isMoving = true; // ���� ��������
    private int clickCount = 0; // ���������� ������ �� �����

    private CatAnimation catAnimator; // �������� �����

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

        // �������� � ������� �����
        catAnimator.StartWalk();

        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // ���������� ������� � ������� ��������
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // �������� ���������� ������� �����
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            GenerateNewTargetPoint();
        }
    }

    private void GenerateNewTargetPoint()
    {
        const float minDistance = 2f; // ����������� ���������� �� ������� ������� �� ����� �����
        int maxAttempts = 100; // ������������ ���������� ������� ����� ���������� �����

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(-areaSize / 2, areaSize / 2);
            float randomZ = Random.Range(-areaSize / 2, areaSize / 2);
            Vector3 potentialPoint = new Vector3(randomX, transform.position.y, randomZ);

            // ���������, ��������� �� ����� ����� ������ ������������ ����������
            if (Vector3.Distance(transform.position, potentialPoint) >= minDistance)
            {
                targetPoint = potentialPoint;
                return;
            }
        }

        // ���� ����� ���� ������� ����� �� �������, ���������� ������� �������� targetPoint
        Debug.LogWarning("�� ������� ����� ���������� �������� �����. ��������� ������� ��������.");
    }

    private void OnMouseDown()
    {
        if (!isMoving) return; // ���������� �����, ���� ����� ��� �����������
        if (EnabledClick == false) return;

        clickCount++;
        StopAllCoroutines(); // ������������� ������� ��������, ���� ����
        StartCoroutine(HandleClick());
    }

    private IEnumerator HandleClick()
    {
        // ��������� �����
        EnabledClick = false;
        isMoving = false;
        catAnimator.StopWalk();

        // ���� �������� �����
        yield return new WaitForSeconds(stopTime);

        // ���������� �������� � ������������� ��������
        speed += speedBoost;
        isMoving = true;

        // �������� �� ������
        if (clickCount >= maxClicks)
        {
            _finish = true;
        }


        yield return new WaitForSeconds(5);
        EnabledClick = true;
    }

    private void OnDrawGizmos()
    {
        // ������ �������, ��� ������������ �����
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize, 0.1f, areaSize));
    }

    public void DropTree() => tree.DropTree();

    private void MoveToFinish()
    {
        catAnimator.StartWalk();

        // �������� � �������� �����
        Vector3 direction = (FinishTarget.position - transform.position).normalized;
        transform.position += direction * 8 * Time.deltaTime;

        // ���������� ������� � ������� ��������
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // �������� ���������� �������� �����
        if (Vector3.Distance(transform.position, FinishTarget.position) < 0.2f)
        {
            Animator animator = GetComponent<Animator>();
            animator.applyRootMotion = false;

            // ���������� ������� ������� � ������� ��������
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

            _finish = false; // ������������� ��������
            isMoving = false;
            Debug.Log("����� � ��������");

          

            catAnimator.Attack();
        }
    }
}