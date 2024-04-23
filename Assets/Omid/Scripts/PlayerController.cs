using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float cameraSmoothnes = 0.3f;

    private Camera mainCamera;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool flipped;
    private GameManager gameManager;
    private Vector3 velocity = Vector3.zero;
    public static bool canTalk;
    public static bool canMove;

    void Start()
    {
        canMove = true;
        mainCamera = Camera.main;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        GameManager.sleep = false;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        if (movement.x < 0 && movement.y == 0 && flipped == false)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            flipped = true;
        }
        else if (movement.x > 0 && movement.y == 0 && flipped == true)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            flipped = false;
        }
        animator.SetFloat("Horizontal", Mathf.Abs(movement.x));
        animator.SetFloat("Vertical", movement.y);


        if (mainCamera != null)
        {
            if (!gameManager.isStore)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
                mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, cameraSmoothnes);
            }
        }

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            canTalk = true;
        }
        if (other.CompareTag("Bed"))
        {
            GameManager.sleep = true;
        }
        if (other.CompareTag("ShopItem"))
        {
            GameManager.itId = other.GetComponent<ItemProperties>().itemId;
            GameManager.itPrice = other.GetComponent<ItemProperties>().itemPrice;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            canTalk = false;
        }
        if (other.CompareTag("Bed"))
        {
            GameManager.sleep = false;
        }
        if (other.CompareTag("ShopItem"))
        {
            Debug.Log("works");
            GameManager.itId = 0;
            GameManager.itPrice = 0;
        }
    }
}

