using UnityEngine;

public class TreeDropper : MonoBehaviour
{
    [SerializeField] private GameObject _uiFinish;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DropTree()
    {
        _animator.SetTrigger("drop");
    }

    public void FinishLvl()
    {
        _uiFinish.SetActive(true);
    }
}
