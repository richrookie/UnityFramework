using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class InputController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private float _jumpPower = 750.0f;


    private void Start()
    {
        Managers.InputMgr.KeyAction -= OnKeyboard;
        Managers.InputMgr.KeyAction += OnKeyboard;

        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        this.GetComponent<ConstantForce>().force = new Vector3(0, -25, 0);
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Vector3.forward), .2f);
            this.transform.position += Vector3.forward * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Vector3.back), .2f);
            this.transform.position += Vector3.back * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Vector3.left), .2f);
            this.transform.position += Vector3.left * _speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Vector3.right), .2f);
            this.transform.position += Vector3.right * _speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * _jumpPower);
        }
    }
}
