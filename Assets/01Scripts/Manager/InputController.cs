using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InputController : MonoBehaviour
{
    [SerializeField]
    private Define.eInputMode _inputMode = Define.eInputMode.Default;

    [SerializeField]
    private float _moveSpeed = 10.0f;
    [SerializeField]
    public float _rollSpeed = 12.0f;
    [SerializeField]
    public float _pitchSpeed = 10.0f;
    [SerializeField]
    private bool _isJump = false;
    [SerializeField]
    private float _jumpPower = 5.0f;

    private Rigidbody _rb = null;
    private ConstantForce _constForce = null;


    private void Start()
    {
        Managers.InputMgr.KeyAction -= OnKeyboard;
        Managers.InputMgr.KeyAction += OnKeyboard;

        _rb = this.GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _constForce = this.GetComponent<ConstantForce>();
        _constForce.force = new Vector3(0, -25, 0);
    }

    void OnKeyboard()
    {
        switch (_inputMode)
        {
            case Define.eInputMode.Default:
                DefaultMode();
                break;

            case Define.eInputMode.Flying:
                FlyingMode();
                break;
        }
    }

    private void DefaultMode()
    {
        _constForce.enabled = true;

        if (Input.GetKeyDown(KeyCode.Space) && !_isJump)
        {
            _isJump = true;

            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }

        Vector3 rotVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            rotVec = Vector3.forward;
            this.transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rotVec = Vector3.back;
            this.transform.position += Vector3.back * _moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotVec = Vector3.right;
            this.transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotVec = Vector3.left;
            this.transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
        }

        _rb.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(rotVec), .2f);
    }

    private void FlyingMode()
    {
        _constForce.enabled = false;

        float rollInput = Input.GetAxis("Horizontal");      // z축
        float pitchInput = Input.GetAxis("Vertical");       // x축

        float rollAngle = -rollInput * _rollSpeed;
        float pitchAngle = pitchInput * _pitchSpeed;

        Quaternion targetRotation = this.transform.rotation * Quaternion.Euler(pitchAngle, 0.0f, rollAngle);

        _rb.velocity = this.transform.forward * _moveSpeed;
        _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * 5.0f);
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
            {
                _isJump = false;
                break;
            }
        }
    }
}
