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
        mainCamera = Camera.main;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        if (other.CompareTag("ShopItem"))
        {
            Debug.Log("works");
            GameManager gameManager = FindObjectOfType<GameManager>();

            SpriteRenderer itemRenderer = other.GetComponent<SpriteRenderer>();

            if (gameManager != null && itemRenderer != null)
            {
                int itemIndex = System.Array.IndexOf(gameManager.itemObjects, other.gameObject);

                if (itemIndex >= 0 && itemIndex < gameManager.priceTexts.Length)
                {
                    string priceText = gameManager.priceTexts[itemIndex].text;

                    if (int.TryParse(priceText.Replace("$", ""), out int price))
                    {
                        GameManager.currentItem = itemRenderer.sprite.name;
                        GameManager.currentPrice = price;

                        Debug.Log("Interacted with item: " + itemRenderer.sprite.name);
                        Debug.Log("Price: $" + GameManager.currentPrice);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to parse price for item: " + other.name);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid item index: " + itemIndex);
                }
            }
            else
            {
                Debug.LogWarning("ShopController or SpriteRenderer not found.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            canTalk = false;
        }
    }
}

