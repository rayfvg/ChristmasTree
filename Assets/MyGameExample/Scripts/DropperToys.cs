using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperToys : MonoBehaviour
{
    public float initialInterval = 2f; // ��������� �������� ����� ��������
    public float minimumInterval = 0.1f; // ����������� �������� ����� ��������
    public float intervalReductionRate = 0.05f; // ��� ���������� ���������
    private float currentInterval; // ������� ��������
    private bool isLaunching = false; // ����, ����� ��������� ��������

    public OrnamentLauncher OrnamentLauncher;

    void Start()
    {
        currentInterval = initialInterval;
        StartLaunchingOrnaments();
    }

    public void StartLaunchingOrnaments()
    {
        if (!isLaunching)
        {
            isLaunching = true;
            StartCoroutine(LaunchOrnamentsWithIncreasingSpeed());
        }
    }

    private IEnumerator LaunchOrnamentsWithIncreasingSpeed()
    {
        while (isLaunching && currentInterval > minimumInterval)
        {
            OrnamentLauncher.LaunchNextOrnament();
            yield return new WaitForSeconds(currentInterval);

            // ��������� ��������, ����� ������ ���������
            currentInterval = Mathf.Max(currentInterval - intervalReductionRate, minimumInterval);
        }

        // ��������� ������: ��������� � ����������� ����������
        while (isLaunching)
        {
            OrnamentLauncher.LaunchNextOrnament();
            yield return new WaitForSeconds(minimumInterval);
        }
    }

    public void StopLaunchingOrnaments()
    {
        isLaunching = false; // ������������� ��������
    }

}
