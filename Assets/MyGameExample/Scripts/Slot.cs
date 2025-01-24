using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public delegate void SlotChanged();
    public event SlotChanged OnSlotChanged;

    public DragDrop Item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DragDrop>().IsDragging == false)
        {
            Item = other.GetComponent<DragDrop>();
            OnSlotChanged?.Invoke(); // Вызываем событие при изменении содержимого
            Item.transform.localScale = Vector3.one;
        }
    }

    private void Update()
    {
        CheckSlot();
    }

    private void CheckSlot()
    {
        if (Item != null)
        { 
            if (Item.GetComponent<Collider>().enabled == false)
            {
                Item = null;
            }
        }
        return;
    }

    public bool IsEmptySlot()
    {
        if (Item)
            return false;
        else
            return true;
    }
}