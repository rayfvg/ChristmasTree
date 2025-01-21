using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperToys : MonoBehaviour
{
    public float initialInterval = 2f; // Начальный интервал между вызовами
    public float minimumInterval = 0.1f; // Минимальный интервал между вызовами
    public float intervalReductionRate = 0.05f; // Шаг уменьшения интервала
    private float currentInterval; // Текущий интервал
    private bool isLaunching = false; // Флаг, чтобы управлять запуском

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

            // Сокращаем интервал, чтобы запуск ускорялся
            currentInterval = Mathf.Max(currentInterval - intervalReductionRate, minimumInterval);
        }

        // Финальная стадия: запускаем с минимальным интервалом
        while (isLaunching)
        {
            OrnamentLauncher.LaunchNextOrnament();
            yield return new WaitForSeconds(minimumInterval);
        }
    }

    public void StopLaunchingOrnaments()
    {
        isLaunching = false; // Останавливает корутину
    }

}
