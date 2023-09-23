using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_old : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    private float horizontal;
    public float jumpForce = 8f;
    public float crouchForce = 8f;
    public float moveForce = 8f;
    public float gravity = 9.81f * 2f;
    public GameObject crouch;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                direction = Vector3.up * jumpForce;
                crouch.SetActive(false);
            }
        }
        character.Move(direction * Time.deltaTime);

        if(Input.GetButtonDown("Down"))
        {
            direction = Vector3.down * crouchForce;
        }
        
        if(Input.GetButtonDown("Down") && character.isGrounded)
        {
            crouch.SetActive(true);
        }

        if(Input.GetButtonUp("Down") && character.isGrounded)
        {
            crouch.SetActive(false);
        }

        if(Input.GetButtonDown("Right"))
        {
            direction = Vector3.right * moveForce * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

}
