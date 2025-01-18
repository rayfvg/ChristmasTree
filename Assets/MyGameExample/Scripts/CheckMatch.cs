using System.Collections.Generic;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    public List<Slot> Slots = new List<Slot>();
  //  public int CountWinner;

    private bool _hasWon = false; // ����������, ����� ������ ���������� ������ ���� ���

    private void Awake()
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        Slots.AddRange(slots);

        // ������������� �� ������� ��������� �����
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
        if (_hasWon) return; // ���� ������ ��� ����, ������ �� ������

        // ���������, ��� �� ����� ���������
        foreach (Slot slot in Slots)
        {
            if (slot.IsEmptySlot()) return;
        }

        // ���������, ��������� �� ��� �������
        string firstItemName = Slots[0].Item.name;

        foreach (Slot slot in Slots)
        {
            if (slot.Item.name != firstItemName) return;
        }

        // ���� ��� ������� �������, ��������� ������
        Debug.Log("������! ��� ������� �������.");
        _hasWon = true;

        // ������������ ������
        foreach (Slot slot in Slots)
        {
           // CountWinner++;
            slot.Item.DeadProcess();
        }
    }
}

