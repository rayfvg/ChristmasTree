using DG.Tweening;
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

    private bool _working = true;


    void Start()
    {
        initialRotation = transform.rotation;

        // ������� ��� Rigidbody, ������������� � �������� ��������
        ornaments = GetComponentsInChildren<CatchToys>();
    }

    private void Update()
    {
        if(CounterCatchToy >= 7)
        {
            _working = false;
            FinishUi.SetActive(true);
        }
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

    public void AddGab()
    {
        CounterCatchToy++;
        Debug.Log(CounterCatchToy);
    }
}
