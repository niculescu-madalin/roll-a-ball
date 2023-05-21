
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public float jumpForce = 2.0f;
    public Transform respawnPoint;
    public MenuController  menuController;

    private bool isGrounded;
    private Rigidbody rb;
    private int count;
    private AudioSource pickUpPop;
    private float movementX;
    private float movementY;
    private Vector3 jump;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        // winTextObject.SetActive(false);
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        pickUpPop = GetComponent<AudioSource>();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&  isGrounded) {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if(transform.position.y < -10) {
            EndGame();
        }

    }

    void OnCollisionStay() 
    {
        isGrounded = true;
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 15)
        {
            // Set the text value of your 'winText'
            // winTextObject.SetActive(true);
            menuController.WinGame();
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            pickUpPop.Play();

        }
    }

    public void Respawn() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        transform.position = respawnPoint.position;

    }

    void EndGame() 
    {
        menuController.LoseGame();
        gameObject.SetActive(false);
    }

}
