using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float jumpVelocity;
	[SerializeField] private LayerMask platformLayerMask;
	private Rigidbody2D _playerRb;
	private CircleCollider2D _circleCollider2D;
	private float _horizontalInput;
	private bool _jumpButton;
	private bool _jumpButtonReleased;

	private void Awake()
	{
		_playerRb = GetComponent<Rigidbody2D>();
		_circleCollider2D = GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		_horizontalInput = Input.GetAxisRaw("Horizontal");
		_jumpButton = Input.GetKeyDown(KeyCode.Space);
		_jumpButtonReleased = Input.GetKeyUp(KeyCode.Space);
		
		if (_jumpButton && IsGrounded())
			_playerRb.velocity = new Vector2(_playerRb.velocity.x, jumpVelocity);
		if (_jumpButtonReleased && _playerRb.velocity.y > 0f)
			_playerRb.velocity = new Vector2(_playerRb.velocity.x, 0);
	}

	private void FixedUpdate()
	{
		_playerRb.velocity = new Vector2(_horizontalInput * (Time.fixedDeltaTime * movementSpeed), _playerRb.velocity.y);
	}

	private bool IsGrounded()
	{
		float extraHeightTest = 0.2f;
		var bounds = _circleCollider2D.bounds;
		RaycastHit2D raycastHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, extraHeightTest, platformLayerMask);
		return raycastHit.collider != null;
	}
}
