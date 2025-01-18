using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class VictoryAnimation : MonoBehaviour
{
    public Transform victoryObject; // Ссылка на родительский объект (картинка + текст)
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1f); // Масштаб, до которого увеличится объект
    public float duration = 0.3f; // Длительность анимации

    public Transform[] starSlots; // Массив слотов для звезд (ссылки на трансформы слотов)
    public GameObject[] stars; // Массив объектов звезд (звуковые и визуальные элементы)
    public float moveDuration = 1f; // Длительность анимации перемещения
    public float rotateDuration = 1f; // Длительность анимации вращения
    public float arcHeight = 1f; // Высота дуги
    public float delayBetweenStars = 0.5f; // Задержка между анимациями каждой звезды
    public Vector3 offscreenPosition = new Vector3(-5f, 0f, 0f); // Начальная позиция звезд (вне экрана, слева)


    public GameObject panel; // Панель с текстом
    public TMP_Text panelText; // Текст на панели
    public Button button; // Кнопка

    public float panelFadeDuration = 1f; // Длительность анимации появления панели
    public float buttonJumpDuration = 0.3f; // Длительность анимации подпрыгивания кнопки
    public float buttonDelay = 0.5f; // Задержка перед подпрыгиванием кнопки


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


    // Метод для активации анимации
    public void ShowVictoryBounce()
    {
        // Увеличиваем размер объекта, создавая эффект подпрыгивания
        victoryObject.localScale = Vector3.zero; // Начинаем с маленького размера

        victoryObject.DOScale(targetScale, duration) // Увеличиваем до targetScale
            .SetEase(Ease.OutBounce) // Добавляем отскок
            .OnComplete(() =>
            {
                // После завершения увеличения возвращаем объект к нормальному размеру
                victoryObject.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBack); // Легкий эффект возвращения
            });
    }

    public void AnimateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            // Для каждой звезды, анимируем её вращение и движение по дуге
            int index = i;  // Копируем индекс для замыкания
            DOVirtual.DelayedCall(delayBetweenStars * i, () => AnimateStar(stars[index], starSlots[index].position, index));
        }
    }

    // Метод анимации одной звезды
    private void AnimateStar(GameObject star, Vector3 targetPosition, int index)
    {
        Vector3 startPosition = offscreenPosition; // Начальная позиция для звезды (вне экрана, слева)

        // Делаем звезду невидимой в начале (или, если нужно, удаляем с экрана)
        star.SetActive(true);
        star.transform.position = startPosition; // Размещаем звезду в начальной позиции

        // Получаем угол поворота родительского слота
        float targetRotation = starSlots[index].eulerAngles.z;

        // Анимация вращения и движения по дуге
        float arcTime = moveDuration * 0.5f; // Время для дуги
        Vector3 arcPoint = (startPosition + targetPosition) / 2 + Vector3.up * arcHeight; // Точка дуги (полу-средняя + высота)

        // Движение по дуге и вращение
        star.transform.DOLocalMove(arcPoint, arcTime).SetEase(Ease.OutQuad) // Двигаемся в точку дуги
            .OnUpdate(() =>
            {
                // Анимация вращения на каждом шаге
                star.transform.Rotate(Vector3.forward * (rotateDuration * 360 / moveDuration) * Time.deltaTime);
            })
            .OnComplete(() =>
            {
                // Завершаем анимацию и перемещаем звезду в слот
                star.transform.DOMove(targetPosition, moveDuration * 0.5f).SetEase(Ease.InQuad) // Перемещаем в слот
                    .OnComplete(() =>
                    {
                        // Устанавливаем угол поворота звезды такой же, как у слота
                        star.transform.rotation = Quaternion.Euler(0, 0, targetRotation);
                    });
            });
    }

    public void AnimatePanelAndButton()
    {
        // Панель с текстом будет плавно появляться
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero; // Начальный размер панели
        panel.GetComponent<CanvasGroup>().alpha = 0f; // Начальная прозрачность панели

        // Анимация появления панели
        panel.transform.DOScale(Vector3.one, panelFadeDuration).SetEase(Ease.OutBack);
        panel.GetComponent<CanvasGroup>().DOFade(1f, panelFadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // После того как панель полностью появилась, анимируем кнопку
            AnimateButton();
        });
    }

    // Метод для анимации подпрыгивания кнопки
    private void AnimateButton()
    {
        // Сначала делаем кнопку невидимой, чтобы она не была видна до начала анимации
        button.gameObject.SetActive(true);
        button.transform.localScale = Vector3.zero;

        // Анимация подпрыгивания кнопки
        button.transform.DOScale(Vector3.one * 1.2f, buttonJumpDuration)
            .SetEase(Ease.OutBounce) // Эффект подпрыгивания
            .OnComplete(() =>
            {
                // После подпрыгивания возвращаем кнопку в нормальный размер
                button.transform.DOScale(Vector3.one, buttonJumpDuration / 2).SetEase(Ease.InBack);
            });
    }
}