using UnityEngine;

public class CatAnimation : MonoBehaviour
{
    public Animator animator;
    public void StartWalk() => animator.SetBool("walk", true);

    public void StopWalk() => animator.SetBool("walk", false);
}
