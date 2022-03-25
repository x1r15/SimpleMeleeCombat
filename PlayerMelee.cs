using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private Transform _hands;
    [SerializeField] private Transform _headWrapper;
    [SerializeField] private Transform _character;

    private Vector3 _handsPosition;
    private Animator _handsAnimator;
    private bool _isAttacking;

    private float _verticalInput;
    private bool _attackInputDown;

    private void Start()
    {
        _handsAnimator = _hands.GetComponent<Animator>();
        _handsPosition = _hands.localPosition;
    }

    private void Update()
    {
        CaptureInput();
        RotateHead();
        AdjustHandsPositionAndRotation();
        HandleAttacking();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HitEnemy(other);
    }

    private void HitEnemy(Collider2D other)
    {
        if (!_isAttacking) return;
        
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Hit();
        }
    }

    private void CaptureInput()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _attackInputDown = Input.GetButtonDown("Fire1");
    }

    private void AdjustHandsPositionAndRotation()
    {
        _hands.eulerAngles = new Vector3(0, 0, 90 * _verticalInput * _character.localScale.x);
        _hands.localPosition = _handsPosition + new Vector3(0, _verticalInput, 0);
    }

    private void RotateHead()
    {
        var target = 
            Quaternion.Euler(
                new Vector3(0, 0, _verticalInput * 17 * _character.localScale.x));
        _headWrapper.rotation = Quaternion.Lerp(_headWrapper.rotation, target, Time.deltaTime * 5);
    }

    private void HandleAttacking()
    {
        var clipsInfo = _handsAnimator.GetCurrentAnimatorClipInfo(0);
        if (clipsInfo.Length == 0 && _isAttacking)
        {
            _isAttacking = false;
            _hands.gameObject.SetActive(false);
        }
        if (_attackInputDown && !_isAttacking)
        {
            _hands.gameObject.SetActive(true);
            _handsAnimator.SetTrigger("Attack");
            _isAttacking = true;
        }
    }
}
