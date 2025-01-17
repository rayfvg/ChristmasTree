using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToysList : MonoBehaviour
{
   public List<DragDrop> Toys = new List<DragDrop>();

    [SerializeField] private GameObject _uiWinner;
    [SerializeField] private AudioSource _winnersMusic;

    private bool _isWorking = true;

    private void Awake()
    {
        DragDrop[] toys = FindObjectsOfType<DragDrop>();
        Toys.AddRange(toys);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _uiWinner.SetActive(true);
        }if (Input.GetKeyDown(KeyCode.G))
        {
            _uiWinner.SetActive(false);
        }

        if (_isWorking)
        {
            foreach (DragDrop toys in Toys)
            {
                if (toys != null)
                    return;
            }
            StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {
        _isWorking = false;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("WINNERS");
        _winnersMusic.Play();
        _uiWinner.SetActive(true);
    }
}
