using System.Collections.Generic;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    public List<Slot> Slots = new List<Slot>();
  //  public int CountWinner;

    private bool _hasWon = false; // Переменная, чтобы победа вызывалась только один раз

    private void Awake()
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        Slots.AddRange(slots);

        // Подписываемся на событие изменения слота
        foreach (Slot slot in Slots)
        {
            slot.OnSlotChanged += CheckWinners;
        }
    }

    private void OnDisable()
    {
        foreach (Slot slot in Slots)
        {
            slot.OnSlotChanged -= CheckWinners;
        }

    }

    private void CheckWinners()
    {
        _hasWon = false;
        if (_hasWon) return; // Если победа уже была, ничего не делаем

        // Проверяем, все ли слоты заполнены
        foreach (Slot slot in Slots)
        {
            if (slot.IsEmptySlot()) return;
        }

        // Проверяем, совпадают ли все объекты
        string firstItemName = Slots[0].Item.name;

        foreach (Slot slot in Slots)
        {
            if (slot.Item.name != firstItemName) return;
        }

        // Если все объекты совпали, фиксируем победу
        Debug.Log("Победа! Все объекты совпали.");
        _hasWon = true;

        // Обрабатываем победу
        foreach (Slot slot in Slots)
        {
           // CountWinner++;
            slot.Item.DeadProcess();
        }
    }
}

