using UnityEngine;
using System;

public class Spider : MonoBehaviour
{
    public float movementSpeed = 3f; 
    public float detectionRange = 10f; 
    public float detectionRange1 = 1f; 
    public float stoppingDistance = 2f;
    public AudioSource spiderSound; 
    public AudioSource spiderAttackSound; 
    public AudioSource dieSound; 
    public AudioSource bloodSound; 

    public GameObject objectToUnhide; 
    public float unhideDuration = 1f; 
    public ParticleSystem bloodParticles; 

    private Transform player; 
    private Animator animator;
    private bool soundPlayed = false; 
    private bool cooldownActive = false; 
    private float cooldownTimer = 0f; 
    private float unhideTimer = 0f; 
    private bool bloodParticlesPlayed = false; 
    private bool stopFollowing = false; 

    public event Action OnSpiderDestroyed;

    private TotalKillsManager totalKillsManager; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure there is a GameObject with the 'Player' tag in the scene.");
        }

        animator.SetInteger("spiderState", 0);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && !stopFollowing)
            {
                animator.SetInteger("spiderState", 0);
                
                if (spiderSound != null && !spiderSound.isPlaying && !soundPlayed)
                {
                    spiderSound.Play();
                    soundPlayed = true; 
                }

                transform.LookAt(player.position);
                if (distanceToPlayer > stoppingDistance)
                {
                    animator.SetInteger("spiderState", 1);

                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    if (!cooldownActive)
                    {
                        cooldownActive = true;
                        cooldownTimer = 3f;
                        animator.SetInteger("spiderState", 2);
                        spiderAttackSound.Play();

                        if (objectToUnhide != null)
                        {
                            objectToUnhide.SetActive(true);
                            unhideTimer = unhideDuration;
                        }
                    }
                }

                transform.Rotate(Vector3.up, 180f);
            }
            else
            {
                animator.SetInteger("spiderState", 0);
                soundPlayed = false; 
            }

            if (cooldownActive)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    cooldownActive = false;
                    animator.SetInteger("spiderState", 0);
                }
            }

            if (objectToUnhide != null && objectToUnhide.activeSelf)
            {
                unhideTimer -= Time.deltaTime;
                if (unhideTimer <= 0f)
                {
                    objectToUnhide.SetActive(false);
                }
            }

            if (IsSpiderNearBullet())
            { 
                dieSound.Play();
                bloodSound.Play();
                
                if (!bloodParticlesPlayed)
                {
                    FindObjectOfType<ScoreManager>().IncreaseScore(40);
                    FindObjectOfType<TotalKillsManager>().IncreaseKills(1);


                    Instantiate(bloodParticles, transform.position, Quaternion.identity);
                    bloodParticlesPlayed = true;
                }
                animator.SetInteger("spiderState", 3);

                stopFollowing = true; 
                Invoke("DestroySpider", 3f); 
                DestroyBullet(); 
            }

           
            if (IsSpiderNearKick())
            {
                dieSound.Play();
                bloodSound.Play();
                
                if (!bloodParticlesPlayed)
                {
                    FindObjectOfType<ScoreManager>().IncreaseScore(40);
                    FindObjectOfType<TotalKillsManager>().IncreaseKills(1);


                    Instantiate(bloodParticles, transform.position, Quaternion.identity);
                    bloodParticlesPlayed = true;
                }
                animator.SetInteger("spiderState", 3);

                stopFollowing = true; 
                Invoke("DestroySpider", 3f); 
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnDrawGizmosSelected1()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange1);
    }

    bool IsSpiderNearBullet()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange1);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Bullet"))
            {
                return true;
            }
        }
        return false;
    }

    bool IsSpiderNearKick()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange1);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Kick"))
            {
                return true;
            }
        }
        return false;
    }

    void DestroyBullet()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet); 
        }
    }

    void DestroySpider()
    {
        OnSpiderDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
