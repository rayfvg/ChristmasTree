using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public List<Slot> slots = new List<Slot>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShuffleItemsInSlots(slots);
    }
    public void ShuffleItemsInSlots(List<Slot> allSlots)
    {
        // Сохраняем все предметы
        List<DragDrop> items = new List<DragDrop>();
        foreach (var slot in allSlots)
        {
            if (slot.Item != null)
            {
                items.Add(slot.Item);
            }
        }

        // Перемешиваем предметы
        System.Random rand = new System.Random();
        items.Sort((a, b) => rand.Next(-1, 2));

        // Перемещаем предметы в случайные слоты
        for (int i = 0; i < items.Count; i++)
        {
            Slot randomSlot = allSlots[rand.Next(allSlots.Count)];
            items[i].transform.DOMove(randomSlot.transform.position, 0.5f).SetEase(Ease.InOutSine);
            items[i].transform.parent = randomSlot.transform; // Устанавливаем родителя в новый слот
        }
    }
}
