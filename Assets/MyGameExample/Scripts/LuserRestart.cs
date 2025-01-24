using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LuserRestart : MonoBehaviour
{
    public List<Slot> slots;
    public GameObject LoseObj;
    public AudioSource LoseSOund;

    private bool _isLose = false;


    private void Update()
    {
        if(_isLose)
            return;

        foreach (Slot slot in slots)
        {
            if (slot.IsEmptySlot() == true)
                return;
           
        }
        

        StartCoroutine(Delay());
    }

    private void Loser()
    {
        LoseObj.SetActive(true);
        LoseSOund.Play();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        _isLose = true;
        Invoke("Loser", 1f);
    }
}
