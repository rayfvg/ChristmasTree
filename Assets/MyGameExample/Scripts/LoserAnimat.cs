using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoserAnimat : MonoBehaviour
{
    public Transform LoseObject;
    private void OnEnable()
    {
        Bootstrap bootstrap = FindObjectOfType<Bootstrap>();
        NextButtonMake but = GetComponentInChildren<NextButtonMake>();
        Button button = but.gameObject.GetComponent<Button>();

        bootstrap.SubscribeLoseButton(button);

        ShowVictoryBounce();
    }


    public void ShowVictoryBounce()
    {
        // Увеличиваем размер объекта, создавая эффект подпрыгивания
        LoseObject.localScale = Vector3.zero; // Начинаем с маленького размера

        LoseObject.DOScale(new Vector3 (1.2f, 1.2f, 1.2f), 0.5f) // Увеличиваем до targetScale
            .SetEase(Ease.OutBounce) // Добавляем отскок
            .OnComplete(() =>
            {
                // После завершения увеличения возвращаем объект к нормальному размеру
                LoseObject.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack); // Легкий эффект возвращения
            });
    }
}
