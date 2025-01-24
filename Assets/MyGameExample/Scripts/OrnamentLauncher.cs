using DG.Tweening;
using System.Collections;
using UnityEngine;

public class OrnamentLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public float minForce = 5f; // ����������� ���� ������
    public float maxForce = 10f; // ������������ ���� ������
    public float minAngle = 30f; // ����������� ���� ������ � ��������
    public float maxAngle = 60f; // ������������ ���� ������ � ��������

    public CatchToys[] ornaments; // ������ Rigidbody �������
    private int currentIndex = 0; // ������ ������� �������


    public float shakeDuration = 0.5f; // ������������ ������
    public float shakeStrength = 10f; // ���� ������
    public float tiltAngle = 10f; // ���� �������
    public int vibrato = 10; // ���������� ���������

    private Quaternion initialRotation; // ��������� ������� ����

    public int CounterCatchToy;

    public GameObject FinishUi;
    public AudioSource WinSound;

    private bool _working = true;

    private void Awake()
    {
        ornaments = GetComponentsInChildren<CatchToys>();
    }
    void Start()
    {
        initialRotation = transform.rotation;

        // ������� ��� Rigidbody, ������������� � �������� ��������
       
    }

    public void LaunchNextOrnament()
    {
        if (_working)
        {
            // ���������, ���� �� ��� ������� ��� �������
            if (currentIndex < ornaments.Length)
            {
                CatchToys ornament = ornaments[currentIndex];
                ornament.EnabledTach();
                ornament.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                if (ornament != null)
                {
                    // ��������� ���������
                    Rigidbody rb = ornament.GetComponent<Rigidbody>();
                    rb.isKinematic = false;

                    ornament.transform.parent = null;

                    // ������������ ��������� ����
                    float force = Random.Range(minForce, maxForce);

                    // ���������� ��������� ����������� � �������� ���������
                    Vector3 launchDirection = Random.insideUnitSphere;
                    launchDirection.z = Mathf.Abs(launchDirection.z); // ��������, ��� Z ������ �������������
                    launchDirection.y = Mathf.Abs(launchDirection.y); // ��������, ��� Y ������ �������������

                    // ����������� ������ ����������� � ��������� ����
                    rb.AddForce(launchDirection.normalized * force, ForceMode.Impulse);
                    ShakeTree();
                }

                // ��������� � ��������� �������
                currentIndex++;
            }

            else
            {
                StartCoroutine(WinDelay());
            }
        }
    }


    public void ShakeTree()
    {
        // ������� ������������������
        Sequence shakeSequence = DOTween.Sequence();

        // ��������� ������ � ���� ��������� ��������
        shakeSequence.Append(transform.DOShakeRotation(shakeDuration, new Vector3(tiltAngle, tiltAngle, tiltAngle), vibrato, 90));

        // ���������� ���� � �������� ���������
        shakeSequence.Append(transform.DORotateQuaternion(initialRotation, 0.2f).SetEase(Ease.OutQuad));
    }

    private IEnumerator WinDelay()
    {
        // ���� �������� ��� �����������, �������
        if (!_working)
            yield break;

        yield return new WaitForSeconds(2);

        _working = false; // ������������� ���������� ��������
        FinishUi.SetActive(true);
        foreach(CatchToys t in ornaments)
        {
            if(t != null)
                Destroy(t.gameObject);
        }
            

        // ���������, ��� ���� �� �������������, ������ ��� ���������
        if (!WinSound.isPlaying)
        {
            WinSound.Play();
        }
    }
}
