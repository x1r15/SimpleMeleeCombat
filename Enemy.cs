using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;

    [SerializeField] private float _immunityTime = 0.5f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void Hit()
    {
        _animator.SetTrigger("Hit");
        _collider.enabled = false;
        StartCoroutine(ReenableCollider());
    }

    private IEnumerator ReenableCollider()
    {
        yield return new WaitForSeconds(_immunityTime);
        _collider.enabled = true;
    }
}
