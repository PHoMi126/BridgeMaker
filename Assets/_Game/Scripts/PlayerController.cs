using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _moveSpeed;

    public CharacterController character;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Moving();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Stopping();
        }
    }

    public void Moving()
    {
        if (_joystick != null && character != null)
        {
            horizontal = _joystick.Horizontal;
            vertical = _joystick.Vertical;
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                character.ChangeAnimation(CharacterController.AnimType.Running);
                direction = new Vector3(horizontal, 0f, vertical);
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
                transform.position += _moveSpeed * Time.deltaTime * direction;
            }
        }
    }

    public void Stopping()
    {
        character.ChangeAnimation(CharacterController.AnimType.Idle);
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            character.winLooseScript.Win();
        }
    }
}
