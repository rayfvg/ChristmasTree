using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class VictoryAnimation : MonoBehaviour
{
    public Transform victoryObject; // ������ �� ������������ ������ (�������� + �����)
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1f); // �������, �� �������� ���������� ������
    public float duration = 0.3f; // ������������ ��������

    public Transform[] starSlots; // ������ ������ ��� ����� (������ �� ���������� ������)
    public GameObject[] stars; // ������ �������� ����� (�������� � ���������� ��������)
    public float moveDuration = 1f; // ������������ �������� �����������
    public float rotateDuration = 1f; // ������������ �������� ��������
    public float arcHeight = 1f; // ������ ����
    public float delayBetweenStars = 0.5f; // �������� ����� ���������� ������ ������
    public Vector3 offscreenPosition = new Vector3(-5f, 0f, 0f); // ��������� ������� ����� (��� ������, �����)


    public GameObject panel; // ������ � �������
    public TMP_Text panelText; // ����� �� ������
    public Button button; // ������

    public float panelFadeDuration = 1f; // ������������ �������� ��������� ������
    public float buttonJumpDuration = 0.3f; // ������������ �������� ������������� ������
    public float buttonDelay = 0.5f; // �������� ����� �������������� ������


    private void OnEnable()
    {
        Bootstrap bootstrap = FindObjectOfType<Bootstrap>();
        NextButtonMake but = GetComponentInChildren<NextButtonMake>();
        Button Button = but.gameObject.GetComponent<Button>();

        bootstrap.SubscripesButton(Button);

        offscreenPosition = new Vector3(-5f, 0f, 0f);
        foreach (var slot in stars)
            slot.SetActive(false);

        panel.SetActive(false);
        // panelText.gameObject.SetActive(false);
        button.gameObject.SetActive(false);

        StartCoroutine(AnimStart());
    }

    private IEnumerator AnimStart()
    {
        ShowVictoryBounce();
        yield return new WaitForSeconds(0.5f);
        AnimateStars();
        yield return new WaitForSeconds(0.5f);
        AnimatePanelAndButton();
    }


    // ����� ��� ��������� ��������
    public void ShowVictoryBounce()
    {
        // ����������� ������ �������, �������� ������ �������������
        victoryObject.localScale = Vector3.zero; // �������� � ���������� �������

        victoryObject.DOScale(targetScale, duration) // ����������� �� targetScale
            .SetEase(Ease.OutBounce) // ��������� ������
            .OnComplete(() =>
            {
                // ����� ���������� ���������� ���������� ������ � ����������� �������
                victoryObject.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack); // ������ ������ �����������
            });
    }

    public void AnimateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            // ��� ������ ������, ��������� � �������� � �������� �� ����
            int index = i;  // �������� ������ ��� ���������
            DOVirtual.DelayedCall(delayBetweenStars * i, () => AnimateStar(stars[index], starSlots[index].position, index));
        }
    }

    // ����� �������� ����� ������
    private void AnimateStar(GameObject star, Vector3 targetPosition, int index)
    {
        Vector3 startPosition = offscreenPosition; // ��������� ������� ��� ������ (��� ������, �����)

        // ������ ������ ��������� � ������ (���, ���� �����, ������� � ������)
        star.SetActive(true);
        star.transform.position = startPosition; // ��������� ������ � ��������� �������

        // �������� ���� �������� ������������� �����
        float targetRotation = starSlots[index].eulerAngles.z;

        // �������� �������� � �������� �� ����
        float arcTime = moveDuration * 0.5f; // ����� ��� ����
        Vector3 arcPoint = (startPosition + targetPosition) / 2 + Vector3.up * arcHeight; // ����� ���� (����-������� + ������)

        // �������� �� ���� � ��������
        star.transform.DOLocalMove(arcPoint, arcTime).SetEase(Ease.OutQuad) // ��������� � ����� ����
            .OnUpdate(() =>
            {
                // �������� �������� �� ������ ����
                star.transform.Rotate(Vector3.forward * (rotateDuration * 360 / moveDuration) * Time.deltaTime);
            })
            .OnComplete(() =>
            {
                // ��������� �������� � ���������� ������ � ����
                star.transform.DOMove(targetPosition, moveDuration * 0.5f).SetEase(Ease.InQuad) // ���������� � ����
                    .OnComplete(() =>
                    {
                        // ������������� ���� �������� ������ ����� ��, ��� � �����
                        star.transform.rotation = Quaternion.Euler(0, 0, targetRotation);
                    });
            });
    }

    public void AnimatePanelAndButton()
    {
        // ������ � ������� ����� ������ ����������
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero; // ��������� ������ ������
        panel.GetComponent<CanvasGroup>().alpha = 0f; // ��������� ������������ ������

        // �������� ��������� ������
        panel.transform.DOScale(Vector3.one, panelFadeDuration).SetEase(Ease.OutBack);
        panel.GetComponent<CanvasGroup>().DOFade(1f, panelFadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // ����� ���� ��� ������ ��������� ���������, ��������� ������
            AnimateButton();
        });
    }

    // ����� ��� �������� ������������� ������
    private void AnimateButton()
    {
        // ������� ������ ������ ���������, ����� ��� �� ���� ����� �� ������ ��������
        button.gameObject.SetActive(true);
        button.transform.localScale = Vector3.zero;

        // �������� ������������� ������
        button.transform.DOScale(Vector3.one * 1.2f, buttonJumpDuration)
            .SetEase(Ease.OutBounce) // ������ �������������
            .OnComplete(() =>
            {
                // ����� ������������� ���������� ������ � ���������� ������
                button.transform.DOScale(Vector3.one, buttonJumpDuration / 2).SetEase(Ease.InBack);
            });
    }
}