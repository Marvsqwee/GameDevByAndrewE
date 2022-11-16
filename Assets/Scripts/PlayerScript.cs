using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerType {Player1 , Player2}
public class PlayerScript : MonoBehaviour
{
    [Header("Player Variables")]
    public float moveSpeed;
    public float jumpForce;
    public float pickUpRange;

    private KeyCode left;
    private KeyCode right;
    private KeyCode jump;
    private KeyCode pickup;
    private Vector2 rayDirection = Vector2.right;

    private Rigidbody2D rb;

    [Header("PlayerSCript Variable")]
    public PlayerType PlayerType;
    public Transform groundCheckPoint;
    public Transform boxholderTransform;

    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;


    private GameObject box;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetupPlayer();
    }




    void Update()
    {
        if(GameManager.instance.GameState == GameState.Ingame)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
            PlayerRotation();
            BoxChecker();
            if (Input.GetKey(left))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            else if (Input.GetKey(right))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (Input.GetKey(jump) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            rb.freezeRotation = true;
        }
    

    }

    public void SetupPlayer()
    {
        switch (PlayerType) {

            case PlayerType.Player1:
                left = KeyCode.LeftArrow;
                right = KeyCode.RightArrow;
                jump = KeyCode.UpArrow;
                pickup = KeyCode.P;
                break;

            case PlayerType.Player2:
                left = KeyCode.A;
                right = KeyCode.D;
                jump = KeyCode.W;
                pickup = KeyCode.R;
                break;



        }

    }

    public void PlayerRotation()
    {
        if(rb.velocity.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            rayDirection = Vector2.right;
        }
        else if (rb.velocity.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            rayDirection = Vector2.left;
        }
    }
    public void BoxChecker()
    {
        RaycastHit2D boxObject = Physics2D.Raycast(boxholderTransform.position, rayDirection, pickUpRange);
        if (box && Input.GetKey(pickup))
        {
            box.GetComponent<BoxCollider2D>().enabled = false;
            box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            box.transform.position = boxholderTransform.transform.position;
        }
        else if(box && !Input.GetKey(pickup))
        {
            box.GetComponent<BoxCollider2D>().enabled = true;
            box.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            box = null;
        }

        if (boxObject.collider)
        {
        
            if(boxObject.collider.tag == "Box")
            {
                if(Input.GetKey(pickup) && !box)
                {
                    Debug.Log("pressed");
                    PickUpBox(boxObject.collider.gameObject);
                }
             
            }
        }
    }

    public void PickUpBox(GameObject x)
    {
        
        box = x;
        
    }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = boxholderTransform.TransformDirection(rayDirection) * pickUpRange;
        Gizmos.DrawRay(boxholderTransform.position, direction);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
           
            GameManager.instance.LosePlayer();
            this.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "Win")
        {
            if (GameManager.instance.DoorOpen)
            {
                GameManager.instance.PlayerIn();
                this.gameObject.SetActive(false);
            }
            
        }

        if(collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            GameManager.instance.DoorOpen = true;
        }

    }
}
