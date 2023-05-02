using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Component.Animating;

public class Move2D : NetworkBehaviour
{
    [SerializeField] private Transform m_main;
    
    [SerializeField] private float m_speed = 1.0f;
    [SerializeField] private Vector2 _userInput;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private NetworkAnimator _networkAnimator;

    private bool _flipped;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _networkAnimator = GetComponentInChildren<NetworkAnimator>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!base.IsOwner) return;
        
        if (context.started)
        {
            _networkAnimator.SetTrigger("attack");
        }

        if (context.canceled)
        {
            _networkAnimator.ResetTrigger("attack");
        }
    }

    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        GetComponent<PlayerInput>().enabled = base.IsOwner;
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        _userInput = context.ReadValue<Vector2>();
        _animator.SetFloat("speed", _userInput.magnitude);
        FlipCharacter();
    }

    private void FlipCharacter()
    {
        if (_userInput.x < 0 && !_flipped)
        {
            _flipped = true;
        }
        else if (_userInput.x > 0 && _flipped)
        {
            _flipped = false;
        }
        m_main.localScale = new Vector3(_flipped ? -1 : 1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner) return;
        _rigidbody2D.MovePosition(_rigidbody2D.position + _userInput * (Time.fixedDeltaTime * m_speed));
    }
}
