using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Camera mainCamera; 
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool flipped;
    private GameManager gameManager;

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
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 5f);
            }
        }

    }

    void FixedUpdate()
    {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
