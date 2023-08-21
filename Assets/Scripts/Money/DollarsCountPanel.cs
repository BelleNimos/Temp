using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class DollarsCountPanel : MonoBehaviour
{
    private const string AddMoneyText = "AddMoney";

    [SerializeField] private TMP_Text _countDollarsText;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationAddMoney()
    {
        _animator.SetTrigger(AddMoneyText);
    }

    public void SetCountDollars(int count)
    {
        _countDollarsText.text = count.ToString();
    }
}
