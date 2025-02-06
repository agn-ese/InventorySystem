using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 4000f;                          // Amount of force added when the player jumps.           
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    private float m_StandardSpeed = 1.7f;                                       // Amount of standard speed of the character
    private bool m_AirControl = true;                                           // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Animator animator;                                 // Animator for animating the character
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    private bool isSlowed = false;
    private float slowedSpeed = 0.8f;
    private float normalSpeed = 1.7f;
    private InventoryItem Item;
    private bool ActionDone = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }


    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * m_StandardSpeed;
        Move(horizontalMovement , false);
        animator.SetFloat("Movement", Mathf.Abs(horizontalMovement));
        animator.SetBool("IsGrounded", m_Grounded);
        if (Input.GetKeyDown(KeyCode.Space))
            Move(horizontalMovement , true);

        if(Item!= null )
        {
            switch (Item.itemId)  // Manages the behavior of each item 
            {
                case 0:
                    if(Input.GetKeyDown(KeyCode.F))
                        ThrowRock(Item.prefab, Item.angle, Item.throwForce);
                    break;
                case 1:
                    if(Input.GetKeyDown(KeyCode.F))
                        CreateBox(Item.prefab);
                    break;
                case 2:
                    if(Input.GetKey(KeyCode.F))
                        ReduceSpeedForSeconds(Item.slowDuration);
                    break;
            }
            if (ActionDone)
            {
                Item = null;
                ActionDone = false;
            }
        }
    }


    public void Move(float move, bool jump)
    {

        // Only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            animator.SetTrigger("Jump");
        }


       
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");

    }

    public void Jump()
    {
        Move(2f, true);
    }

    private void ThrowRock(GameObject prefab, float angle, float throwForce)
    {
        Vector3 spawnPosition = transform.position;
        float offset = 1f; // Distance to spawn the rock in front of the player
        spawnPosition.x += m_FacingRight ? offset : -offset;
        GameObject rock = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Adjusts the throw direction based on player's facing direction
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 throwVelocity = new Vector2(
                (m_FacingRight ? 1 : -1) * throwForce * Mathf.Cos(angleInRadians),
                throwForce * Mathf.Sin(angleInRadians)
            );
            rb.velocity = throwVelocity;
        }

        ActionDone = true;
        StartCoroutine(DestroyAfterSeconds(rock)); // Destroy the instantiated rock
    }

    private IEnumerator DestroyAfterSeconds(GameObject prefab)
    {
        yield return new WaitForSeconds(2); 
        Destroy(prefab);
    }

    public void ReduceSpeedForSeconds(float numberOfSeconds)
    {
        if (!isSlowed) // Prevent overlapping slowdowns
        {
            StartCoroutine(SlowDown(numberOfSeconds));
            ActionDone = true;
        }
    }

    private IEnumerator SlowDown(float duration)
    {
        isSlowed = true;
        m_StandardSpeed = slowedSpeed; // Reduce the speed
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        m_StandardSpeed = normalSpeed; // Restore the speed
        isSlowed = false;
    }

    public void setItem(InventoryItem item)
    {
        if(Item == null) // If no item is set, then set new item
            Item = item;
        else  // If Item is already set, means that the item has not been used yet, so reassign item and put the last item in the inventory
        {
            Inventory.instance.AddItemToInventory(Item);
            Item = item;
        }
    }

    private void CreateBox(GameObject prefab)
    {
        // Create a box at the position of the mouse
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + transform.forward * 2f;
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        ActionDone = true;
    }

}