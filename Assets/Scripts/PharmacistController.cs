using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmacistController : MonoBehaviour
{
    //Public properties
    public int MaxHealth;
    public float Speed;
    public float InvicibilityTime;
    public bool IsUsingMouse;

    Rigidbody2D rigidbody2D;
    Animator animator;

    int currentHealth;
    bool isInvincible;
    float invincibilityTimer;
    Vector2 startingPosition;
    Vector2 targetPosition;

    private GameObject plantObject;
    private bool isGrabbing = false;
    private float grabbingTimer;

    private bool isDead = false;
    private float DeathTimer;

    private bool plantEffect1 = false;
    private float speedBoostTimer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isInvincible = false;
        currentHealth = MaxHealth;
        startingPosition.x = 0.5f;
        startingPosition.y = -0.5f;
        targetPosition = startingPosition;

        Application.targetFrameRate = 60;
        //QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (IsUsingMouse) //Movement
            {
                //Grabbing Plant
                if (!isGrabbing)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (plantObject != null)
                        {
                            isGrabbing = true;
                            animator.SetBool("isGrabbing", isGrabbing);
                            targetPosition = rigidbody2D.position;
                            grabbingTimer = plantObject.GetComponent<PlantsEffects>().GrabTime;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        float angle = Mathf.Atan2((targetPosition.y - rigidbody2D.position.y), (targetPosition.x - rigidbody2D.position.x)) * Mathf.Rad2Deg;
                        transform.eulerAngles = new Vector3(0, 0, angle - 90);
                    }

                    if (plantEffect1)
                    {
                        rigidbody2D.MovePosition(Vector2.MoveTowards(rigidbody2D.position, targetPosition, Time.deltaTime * (Speed * 1.5f)));
                        speedBoostTimer -= Time.deltaTime;
                        if (speedBoostTimer <= 0) plantEffect1 = false;
                    }
                    else
                    {
                        rigidbody2D.MovePosition(Vector2.MoveTowards(rigidbody2D.position, targetPosition, Time.deltaTime * Speed));
                    }
                }
                else
                {
                    grabbingTimer -= Time.deltaTime;
                    if (grabbingTimer <= 0)
                    {
                        isGrabbing = false;
                        animator.SetBool("isGrabbing", isGrabbing);

                        switch (plantObject.GetComponent<PlantsEffects>().Type)
                        {
                            case 1://Temporary SpeedBoost
                                plantEffect1 = true;
                                speedBoostTimer = plantObject.GetComponent<PlantsEffects>().EffectTime;
                                break;
                            case 2: //life Bonus
                                ChangeHealth(1);
                                break;
                        }

                        GameObject.Destroy(plantObject);
                    }
                }
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector2 position = rigidbody2D.position;
                //Vector2 position = transform.position;

                position.x = position.x + Time.deltaTime * Speed * horizontal;
                position.y = position.y + Time.deltaTime * Speed * vertical;

                rigidbody2D.MovePosition(position);
                //transform.position = position;
            }

            //Running - Stall Animation.
            if (targetPosition == rigidbody2D.position)
            {
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isRunning", true);
            }

            //Invincibility Timer.
            if (isInvincible)
            {
                invincibilityTimer -= Time.deltaTime;

                if (invincibilityTimer <= 0)
                {
                    isInvincible = false;
                }
            }
        }
        else //Dead
        {
            DeathTimer -= Time.deltaTime;

            if (DeathTimer <= 0)
            {
                isDead = false;
                animator.SetBool("isDead", false);
                rigidbody2D.position = startingPosition;
                currentHealth = MaxHealth;
                UIHealthBar.instance.SetValue(currentHealth);

                targetPosition = startingPosition;

                var manager = GetComponentInParent<GameManager>();
                manager.Reset();
            }
        }
    }

    void ChangeHealth(int amount)
    {
        if (amount > 0 || !isInvincible)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);
            Debug.Log(currentHealth + "/" + MaxHealth);

            if (currentHealth <= 0)
            {
                isDead = true;
                DeathTimer = 3.0f;
                animator.SetBool("isDead", true);
                targetPosition = startingPosition;
            }
            else if (amount < 0)
            {
                isInvincible = true;
                invincibilityTimer = InvicibilityTime;
            }
        }

        UIHealthBar.instance.SetValue(currentHealth);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Loader")
        {
            var grid = other.transform.parent.gameObject;
            var board = grid.transform.parent.gameObject;

            var manager = GetComponentInParent<GameManager>();
            manager.Loader(board);
        }
        else if (other.CompareTag("TextLoader"))
        {
            var manager = GetComponentInParent<GameManager>();
            manager.UpdateText();
        }
        else if (other.CompareTag("Plant"))
        {
            plantObject = other.gameObject;
        }
        else
        {
            ChangeHealth(-1);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            if (other.gameObject == plantObject)
                plantObject = null;
        }
    }
}
