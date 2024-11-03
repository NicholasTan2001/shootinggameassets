using UnityEngine;

public class Deer : MonoBehaviour
{
    public float moveRange = 5f; 
    public float moveSpeed = 2f; 
    public int maxLives = 3; 

    private Vector3 initialPosition; 
    private Vector3 targetPosition;
    private float moveDelay = 5f; 
    private float moveTimer = 0f; 
    private int currentLives;
    private Animator animator;

    public AudioSource bloodSound;
    public AudioSource deerSound;

    public ParticleSystem bloodParticles;

    private bool rotating = false; 
    private float rotationSpeed = 50f; 
    private float targetRotation = 0f; 

    public GameObject object1; 

    void Start()
    {
        initialPosition = transform.position;
        MoveRandom();
        animator = GetComponent<Animator>();
        currentLives = maxLives;

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= moveDelay)
            {
                moveTimer = 0f;
                MoveRandom();
            }
            else
            {
                animator.SetInteger("deerState", 0);
            }
        }
        else
        {
            animator.SetInteger("deerState", 1);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (rotating)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), step);

            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation)) < 0.1f)
            {
                rotating = false;
            }
        }
    }

    void MoveRandom()
    {
        float randomX = Random.Range(initialPosition.x - moveRange, initialPosition.x + moveRange);
        float randomZ = Random.Range(initialPosition.z - moveRange, initialPosition.z + moveRange);
        randomX = Mathf.Clamp(randomX, initialPosition.x - moveRange, initialPosition.x + moveRange);
        randomZ = Mathf.Clamp(randomZ, initialPosition.z - moveRange, initialPosition.z + moveRange);
        targetPosition = new Vector3(randomX, initialPosition.y, randomZ);
        transform.LookAt(targetPosition);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(moveRange * 2, 0.1f, moveRange * 2));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            bloodSound.Play();
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            currentLives -= 2;
            if (currentLives <= 0)
            {
                deerSound.Play();
                targetRotation = 90f;
                rotating = true;
                DestroyAfterDelay();
            }
        }
        else if (other.CompareTag("Punch"))
        {
            bloodSound.Play();
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            currentLives--;
            if (currentLives <= 0)
            {
                deerSound.Play();
                targetRotation = 90f;
                rotating = true;
                DestroyAfterDelay();
            }
        }
    }

    void DestroyAfterDelay()
    {
        Invoke("DestroyDeer", 2.3f);
    }

    void DestroyDeer()
    {
        Destroy(gameObject);

        FindObjectOfType<ScoreManager>().IncreaseScore(20);
        FindObjectOfType<TotalKillsManager>().IncreaseKills(1);

        if (object1 != null)
        {
            Instantiate(object1, transform.position, Quaternion.identity);
        }
    }
}
