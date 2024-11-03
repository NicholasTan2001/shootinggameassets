using UnityEngine;

public class BossSpider : MonoBehaviour
{
    public float detectionRangeX = 10f; 
    public float detectionRangeZ = 10f; 
    public float stoppingRange = 5f; 
    public float movementSpeed = 3f; 
    public float delayBeforeState2 = 2f; 
    public float delayBeforeDestroy = 3f; 

    public GameObject objectToUnhide; 

    private Transform player; 
    private Animator animator;
    private float delayTimer = 0f; 
    private float destroyTimer = 0f; 
    private bool isObjectUnhidden = false; 
    private bool hasPlayedSound = false; 
    private bool hasPlayedDieSound = false; 
    private int life = 10;

    public ParticleSystem bloodParticles; 

    public AudioSource bloodSound; 
    public AudioSource dieSound;
    public AudioSource spiderSound;
    public AudioSource attackSound;


    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Vector3 center = transform.position + new Vector3(0f, 0.5f, 0f); 
        Vector3 size = new Vector3(detectionRangeX * 2, 1f, detectionRangeZ * 2);
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(stoppingRange * 2, 1f, stoppingRange * 2));
    }

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();


        SetBossState(0);
    }

    void Update()
    {
  
        if (life <= 0)
        {
            if (!hasPlayedDieSound)
            {
                dieSound.Play();
                hasPlayedDieSound = true;
            }

            SetBossState(3);
            
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= delayBeforeDestroy)
            {
                Destroy(gameObject);
                FindObjectOfType<ScoreManager>().IncreaseScore(100);
                FindObjectOfType<TotalKillsManager>().IncreaseKills(1);

            }
            return;
        }

        if (IsPlayerDetected())
        {

            if (!hasPlayedSound)
            {
                spiderSound.Play();
                hasPlayedSound = true;
            }

            MoveTowardsPlayer();
        }
        else
        {
            SetBossState(0);

            hasPlayedSound = false;
        }

        if (IsPlayerWithinStoppingRange())
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayBeforeState2)
            {
                SetBossState(2);
                attackSound.Play();
                delayTimer = 0f;
            }
            else
            {
                SetBossState(0);
            }
        }
        else
        {
            delayTimer = 0f;
        }

        if (animator.GetInteger("bossState") == 2 && !isObjectUnhidden)
        {
            objectToUnhide.SetActive(true);

            isObjectUnhidden = true;

            Invoke("HideObject", 0.007f);
        }
    }

    void HideObject()
    {
        objectToUnhide.SetActive(false);

        isObjectUnhidden = false;
    }

    bool IsPlayerDetected()
    {
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceZ = Mathf.Abs(player.position.z - transform.position.z);

        return distanceX <= detectionRangeX && distanceZ <= detectionRangeZ;
    }

    bool IsPlayerWithinStoppingRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        return distanceToPlayer <= stoppingRange;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingRange)
        {
            SetBossState(1);

            float movementAmount = movementSpeed * Time.deltaTime;

            transform.position += direction * movementAmount;

            Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookAtPosition);

            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
        }
    }

    void SetBossState(int state)
    {
        animator.SetInteger("bossState", state);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Punch"))
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            bloodSound.Play();
            life -= 1; 
        }
        else if (other.CompareTag("Bullet"))
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
             Destroy(other.gameObject);
            bloodSound.Play();
            life -= 2; 
        }
    }
}
