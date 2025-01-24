using UnityEngine;

public class TreeDropper : MonoBehaviour
{
    [SerializeField] private GameObject _uiFinish;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _treeDrop;
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
        _audioSource.Play();
    }

    public void CrashTree()
    {
        _treeDrop.Play();
    }
}
