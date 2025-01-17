using DG.Tweening;
using UnityEditor.PackageManager;
using UnityEngine;

public class DragDrop : MonoBehaviour
{

    [SerializeField] private ParticleSystem _deadParticle;
    [SerializeField] private AudioSource _take;
    [SerializeField] private AudioSource _drop;
    [SerializeField] private AudioSource _destroy;

    Vector3 offset;
    public string destinationTag = "DropArea";
    public bool IsDragging = false;

    private Vector3 originalPosition;  // ���������� ��� ����������� �������� ������� �������
    private Transform originalParent;  // ���������� ��� ����������� ��������� ��������

    private Vector3 startScale;
    private Vector3 _startPosition;

    public bool _isStart = true;


    private void Start()
    {
        startScale = transform.localScale;
        _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStart)
            StartPositionForItem(other);
    }

    void OnMouseDown()
    {
        // Debug.Log("� ���� " + name);
        _take.Play();
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);

        // ���������� �������� ������� � �������� ������� ����� ��� ������������
        originalPosition = transform.position;
        originalParent = transform.parent;

        transform.parent = null;
        offset = transform.position - MouseWorldPosition();
        transform.GetComponent<Collider>().enabled = false;

        // ������������� Z-���������� ������� � 0, ����� �� �������� ������ �� ���� X � Y
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

    }

    void OnMouseDrag()
    {
        Vector3 newPosition = MouseWorldPosition() + offset;

        // ������������� Z-���������� � 0, ����� ������ �� �������� �� ��� Z
        newPosition.z = 3f;

        transform.position = newPosition;
        IsDragging = true;
    }

    void OnMouseUp()
    {
        // Debug.Log("� �������� �����");
        _drop.Play();
        transform.localScale = startScale;
        IsDragging = false;

        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit hitInfo;

        // ��������� ��������� ������� � DropArea
        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo))
        {
            if (hitInfo.transform.tag == destinationTag)
            {
                // ���� ������ �������� � ������� ����, �� ������������� ��� ������� �� ������� ����
                if (hitInfo.collider.GetComponent<Slot>().IsEmptySlot() == true)
                {
                    transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, 3f);
                    transform.parent = hitInfo.transform;  // ������������� �������� � ������� ������
                    RestartPositionWithNod();
                }
                else if (hitInfo.collider.GetComponent<Slot>().IsEmptySlot() == false)
                {
                    Debug.LogError("���� �����");
                    RestartPosotion();
                }
            }
            else
            {
                // ���� ������ �� � ������� ����, ���������� ��� �� �������� ������� � ��������������� ��������
                RestartPosotion();
            }
        }
        else
        {
            // ���� ������ �� ����� � �����-�� �������, ���������� ��� �� �������� ������� � ��������������� ��������
            RestartPosotion();
        }

        // ��������������� collider �������
        transform.GetComponent<Collider>().enabled = true;
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
    private void RestartPosotion()
    {
        transform.position = originalPosition;
        transform.parent = originalParent;

        RestartPositionWithShake();
    }

    private void RestartPositionWithNod()
    {
        // ������� ������� �������
        Vector3 currentPosition = transform.position;

        // ������� ������������������ ��������
        DG.Tweening.Sequence nodSequence = DOTween.Sequence();

        // �������� ������ ������� ����
        nodSequence.Append(transform.DOMoveY(currentPosition.y - 0.6f, 0.1f).SetEase(Ease.InOutSine));

        // ��������� ����� � ������ �����
        nodSequence.AppendInterval(0.1f);

        // ����� ���������� ������ ������� �� ������� �������
        nodSequence.Append(transform.DOMoveY(currentPosition.y, 0.05f).SetEase(Ease.OutSine));
    }

    private void RestartPositionWithShake()
    {
        // ��������� ��������� �������
        Vector3 originalPosition = transform.position;

        // ������� ������������������ ��������
        DG.Tweening.Sequence shakeSequence = DOTween.Sequence();

        // ���������� ������ � ��������� �������
        shakeSequence.Append(transform.DOMove(originalPosition, 0).SetEase(Ease.OutSine));

        // ��������� ������ ����� �����������
        shakeSequence.Append(transform.DOShakePosition(0.3f, strength: 0.2f, vibrato: 10, randomness: 90, snapping: false, fadeOut: true));
    }

    public void DeadProcess()
    {
        // ��������� ��������� ������ �������
        Vector3 originalScale = transform.localScale;

        // ������� ������ ��� ���������� (�������� �� �������� �������)
        Vector3 reducedScale = originalScale * 0.2f;

        // ������� ������������������ ��������
        Sequence deadSequence = DOTween.Sequence();

        // ��������� �������� ����� ������� ��������
        deadSequence.AppendInterval(0.3f); // ���������� ����� ��� ���� �����

        // ��������� ������ �� �������� �������
        deadSequence.Append(transform.DOScale(reducedScale, 0.2f).SetEase(Ease.InOutSine));

        // ���� 0.1 ������� � ����������� ���������
        deadSequence.AppendInterval(0.1f);

        // ���������� ������ � ��������� �������
        deadSequence.Append(transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.InOutSine));


        deadSequence.OnComplete(() =>
        {
            // ������� �������
            Instantiate(_deadParticle, transform.position, Quaternion.identity);
            _destroy.Play();
            // ���������� ������
            Destroy(gameObject);
        });
    }

    private void StartPositionForItem(Collider collider)
    {
        if (collider.GetComponent<Slot>())
        {
            Debug.Log("y");
            transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, 3f);
            transform.parent = collider.transform;  // ������������� �������� � ������� ������
            _isStart = false;
        }
    }

}
