using System.Collections;
using UnityEngine;

public class CatchToys : MonoBehaviour
{
    [SerializeField] private ParticleSystem _catch;
    [SerializeField] private ParticleSystem _destroy;

    [SerializeField] private AudioSource _source;

    public float TimeForEnabledTach;

    public bool Taching = false;

    public OrnamentLauncher OrnamentLauncher;

    private void OnMouseDown()
    {
        if (Taching == false)
            return;

        Instantiate(_catch, transform.position, Quaternion.identity);
        _source.Play();
        OrnamentLauncher.AddGab();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<GroundMake>() != null)
        {
            Instantiate(_destroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private IEnumerator Delay()
    {
        Debug.Log("Я оторвался и полетел");
        yield return new WaitForSeconds(TimeForEnabledTach);
        Debug.Log("лови меня");
        Taching = true;
    }

    public void EnabledTach()
    {
        StartCoroutine(Delay());
    }
}
