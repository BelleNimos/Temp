using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NextLevelUI : MonoBehaviour
{
    private const string Open = "Open";
    private const string Close = "Close";

    [SerializeField] private AudioSource _departure;

    private Animator _animator;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenPanel()
    {
        _departure.Play();
        _animator.SetTrigger(Open);
        IsActive = true;
    }

    public void ClosePanel()
    {
        _animator.SetTrigger(Close);
        IsActive = false;
    }
}
