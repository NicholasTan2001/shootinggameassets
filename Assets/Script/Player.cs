using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float movementSpeed = 5f; 
    public GameObject objectToUnhide1; 
    public GameObject objectToUnhide2; 
    public GameObject objectToUnhide3; 
    public GameObject objectToUnhide4;

    public float detectionRange = 5f; 
    public float detectionRange1 = 5f;
    public float detectionRange2 = 4f;
    public float detectionRange3 = 3f;
    public TextMeshProUGUI livesText; 

    public AudioSource walkingSound;
    public AudioSource walkingRightSound;
    public AudioSource walkingLeftSound;
    public AudioSource walkingBackSound;
    public AudioSource hurtSound;
    public AudioSource punchSound;
    public AudioSource kickSound;
    public AudioSource meatSound;

    private Animator animator;
    private bool isAiming = false;
    private bool isTransitioning = false;
    private bool isWalking = false;
    private bool isWalkingRight = false;
    private bool isWalkingLeft = false;
    private bool isWalkingBack = false;

    private int lives = 3; 
    private bool isCooldown = false; 
    private float cooldownTimer = 0f; 
    private float cooldownDuration = 3f;

    private bool canControl = true;

    private bool isDamageCooldown = false; 
    private float damageCooldownTimer = 0f; 
    private float damageCooldownDuration = 1f; 

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("playerState", 0);
        UpdateLivesUI();
    }

    void Update()
    {
        if (!canControl)
            return;

        bool isMoving = false;

        if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                isCooldown = false;
                cooldownTimer = 0f;
            }
        }

        if (isDamageCooldown)
        {
            damageCooldownTimer += Time.deltaTime;
            if (damageCooldownTimer >= damageCooldownDuration)
            {
                isDamageCooldown = false;
                damageCooldownTimer = 0f;
            }
        }

        if (isAiming)
        {
            animator.SetInteger("playerState", 6);
            if (objectToUnhide1 != null && !isTransitioning)
            {
                isTransitioning = true;
                Invoke("UnhideObjects", 1f);
            }
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetInteger("playerState", 8);

                objectToUnhide3.SetActive(true);
                Invoke("HideObject3", 0.5f); 
            }

            if (IsSpiderNearby() && !isDamageCooldown)
            {
                TakeDamage(); 
                hurtSound.Play();
                isDamageCooldown = true; 
            }
            else if (IsBoss() && !isDamageCooldown)
            {
                TakeDamage(); 
                TakeDamage(); 
                hurtSound.Play();
                isDamageCooldown = true; 
            }

            if (lives == 0)
            {
                animator.SetInteger("playerState", 11);
            }

            else if (Input.GetMouseButtonDown(2))
            {
                isAiming = false;
                if (animator.GetInteger("playerState") != 0)
                {
                    animator.SetInteger("playerState", 7);
                }
                if (objectToUnhide1 != null && !isTransitioning)
                {
                    isTransitioning = true;
                    Invoke("HideObjects", 1f);
                }
                if (objectToUnhide2 != null && !isTransitioning)
                {
                    isTransitioning = true;
                    Invoke("HideObjects", 1f);
                }
                if (IsSpiderNearby() && !isDamageCooldown)
                {
                    TakeDamage(); 
                    hurtSound.Play();
                    isDamageCooldown = true; 
                }
                if (IsBoss() && !isDamageCooldown)
                {
                    TakeDamage(); 
                    TakeDamage(); 
                    hurtSound.Play();
                    isDamageCooldown = true; 
                }

                if (lives == 0)
                {
                    animator.SetInteger("playerState", 11); 
                }

            }
            return; 
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            isMoving = true;

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                MoveForwardRight();
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                MoveForwardLeft();
            }
            else
            {
                MoveForward();
            }

            if (!isWalking)
            {
                isWalking = true;
                walkingSound.Play();
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                walkingSound.Stop();
            }
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                MoveForwardRight();
            }
            else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                MoveBackRight();
            }
            else
            {
                MoveRight();
            }
            if (!isWalkingRight)
            {
                isWalkingRight = true;
                walkingRightSound.Play();
            }
        }
        else
        {
            if (isWalkingRight)
            {
                isWalkingRight = false;
                walkingRightSound.Stop();
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            isMoving = true;

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                MoveBackLeft();
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                MoveForwardLeft();
            }
            else
            {
                MoveLeft();
            }
            if (!isWalkingLeft)
            {
                isWalkingLeft = true;
                walkingLeftSound.Play();
            }
        }
        else
        {
            if (isWalkingLeft)
            {
                isWalkingLeft = false;
                walkingLeftSound.Stop();
            }
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isMoving = true;

            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            {
                MoveBackRight();
            }
            else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            {
                MoveBackLeft();
            }
            else
            {
                MoveBackward();
            }
            if (!isWalkingBack)
            {
                isWalkingBack = true;
                walkingBackSound.Play();
            }
        }
        else
        {
            if (isWalkingBack)
            {
                isWalkingBack = false;
                walkingBackSound.Stop();
            }
        }

        if (IsSpiderNearby() && !isDamageCooldown)
        {
            TakeDamage(); 
            hurtSound.Play();
            isDamageCooldown = true; 

            if (lives == 0)
            {
                animator.SetInteger("playerState", 11); 
            }
            else
            {
                animator.SetInteger("playerState", 9); 
            }

            isMoving = true;
        }
       
        else if (Input.GetMouseButtonDown(0) && !isCooldown)
        {
            if (IsSpiderNearby1())
            {
                animator.SetInteger("playerState", 8);
                objectToUnhide3.SetActive(true);
                kickSound.Play();
                Invoke("HideObject3", 0.5f); 
            }
            else
            {
                animator.SetInteger("playerState", 5);
                punchSound.Play();
                objectToUnhide4.SetActive(true);
                Invoke("HideObject4", 0.5f); 
            }
            isMoving = true;
            isCooldown = true; 
        } 
        else if (IsBoss() && !isDamageCooldown)
        {
            TakeDamage(); 
            TakeDamage(); 

            hurtSound.Play();
            isDamageCooldown = true;

            if (lives == 0)
            {
                animator.SetInteger("playerState", 11); 
            }
            else
            {
                animator.SetInteger("playerState", 9); 
            }

            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && IsPlayerNearBullet())
        {
            animator.SetInteger("playerState", 10);
            isMoving = true;
        }

        if (Input.GetMouseButtonDown(2))
        {
            isMoving = true;
            Aim();
        }

        if (!isMoving)
        {
            animator.SetInteger("playerState", 0);
            if (objectToUnhide1 != null && !isTransitioning)
            {
                isTransitioning = true;
                Invoke("HideObjects", 1f);
            }
            if (objectToUnhide2 != null && !isTransitioning)
            {
                isTransitioning = true;
                Invoke("HideObjects", 1f);
            }
        }

        if (animator.GetInteger("playerState") == 11)
        {
            canControl = false;
            Invoke("RestartGame", 5f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void HideObjects()
    {
        if (objectToUnhide1 != null)
        {
            objectToUnhide1.SetActive(false);
        }
        if (objectToUnhide2 != null)
        {
            objectToUnhide2.SetActive(false);
        }
        isTransitioning = false;
    }

    void UnhideObjects()
    {
        if (objectToUnhide1 != null)
        {
            objectToUnhide1.SetActive(true);
        }
        if (objectToUnhide2 != null)
        {
            objectToUnhide2.SetActive(true);
        }
        isTransitioning = false;
    }

    void HideObject3()
    {
        if (objectToUnhide3 != null)
        {
            objectToUnhide3.SetActive(false);
        }
    }

    void HideObject4()
    {
        if (objectToUnhide4 != null)
        {
            objectToUnhide4.SetActive(false);
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        animator.SetInteger("playerState", 1);
    }

    void MoveLeft()
    {
        transform.Translate(Vector3.left * movementSpeed * 0.5f * Time.deltaTime); // Decrease movement speed

        animator.SetInteger("playerState", 3);
    }

    void MoveBackward()
    {
        transform.Translate(Vector3.back * movementSpeed * 0.5f * Time.deltaTime); // Decrease movement speed

        animator.SetInteger("playerState", 4);
    }

    void MoveRight()
    {
        transform.Translate(Vector3.right * movementSpeed * 0.5f * Time.deltaTime); // Decrease movement speed

        animator.SetInteger("playerState", 2);
    }

    void MoveForwardRight()
    {
        transform.Translate((Vector3.forward + Vector3.right).normalized * movementSpeed * 0.5f * Time.deltaTime);

        animator.SetInteger("playerState", 2);
    }

    void MoveBackRight()
    {
        transform.Translate((Vector3.back + Vector3.right).normalized * movementSpeed * 0.5f * Time.deltaTime);

        animator.SetInteger("playerState", 4);
    }

    void MoveBackLeft()
    {
        transform.Translate((Vector3.back + Vector3.left).normalized * movementSpeed * 0.5f * Time.deltaTime);

        animator.SetInteger("playerState", 4);
    }

    void MoveForwardLeft()
    {
        transform.Translate((Vector3.forward + Vector3.left).normalized * movementSpeed * 0.5f * Time.deltaTime);

        animator.SetInteger("playerState", 3);
    }

    void Aim()
    {
        isAiming = !isAiming;

        animator.SetInteger("playerState", isAiming ? 6 : 0);
    }

    void TakeDamage()
    {
        lives--;

        if (lives < 0)
        {
        lives = 0;
        }
        
        UpdateLivesUI();
        if (lives <= 0)
        {            
            Invoke("RestartGame", 5f);
            Debug.Log("Game Over!");
        }
    }

    void UpdateLivesUI()
    {
        livesText.text = lives.ToString();
    }

    bool IsSpiderNearby()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Spider"))
            {
                return true;
            }
        }
        return false;
    }

    bool IsSpiderNearby1()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange1);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Spider"))
            {
                return true;
            }
        }
        return false;
    }

    bool IsPlayerNearBullet()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange3);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("BulletBox"))
            {
                return true;
            }
        }
        return false;
    }

    bool IsBoss()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Boss"))
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            meatSound.Play();
            lives++;
            UpdateLivesUI();
            Destroy(other.gameObject); 
        }
    }
}