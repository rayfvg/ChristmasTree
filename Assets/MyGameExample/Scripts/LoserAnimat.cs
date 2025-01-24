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
        // ����������� ������ �������, �������� ������ �������������
        LoseObject.localScale = Vector3.zero; // �������� � ���������� �������

        LoseObject.DOScale(new Vector3 (1.2f, 1.2f, 1.2f), 0.5f) // ����������� �� targetScale
            .SetEase(Ease.OutBounce) // ��������� ������
            .OnComplete(() =>
            {
                // ����� ���������� ���������� ���������� ������ � ����������� �������
                LoseObject.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack); // ������ ������ �����������
            });
    }
}
