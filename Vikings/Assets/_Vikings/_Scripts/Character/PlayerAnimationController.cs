using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]private RuntimeAnimatorController _baseController;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _baseController;
        float speed = Random.Range(0.9f, 1.1f);
        _animator.SetFloat("Speed", speed);
    }

    public void ChangeAnimatorController(AnimatorOverrideController animatroController)
    {
        _animator.runtimeAnimatorController = animatroController;
    }

    public void ReturnBaseAnimator()
    {
        _animator.runtimeAnimatorController = _baseController;
    }

    public void Run()
    {
        _animator.SetBool("Work", false);
        _animator.SetBool("Collect", false);
        _animator.SetFloat("Move", 1);
    }

    public void Idle()
    {
        _animator.SetBool("Work", false);
        _animator.SetBool("Collect", false);
        _animator.SetFloat("Move", 0);
    }

    public void Collect()
    {
        _animator.SetBool("Work", false);
        _animator.SetBool("Collect", true);
        _animator.SetFloat("Move", 0);
    }

    public void Work()
    {
        _animator.SetBool("Work", true);
        _animator.SetBool("Collect", false);
        _animator.SetFloat("Move", 0);
    }

   
}
