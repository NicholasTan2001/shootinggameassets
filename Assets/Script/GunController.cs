using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint; 
    public float bulletSpeed = 10f; 
    public Camera mainCamera; 
    public GameObject objectToCheck; 
    public ParticleSystem fireEffect; 
    public AudioSource shootSound; 

    private int bulletRemaining = 10;
    private int bulletsShot = 0; 
    private bool canShoot = true; 
    private int bulletQuantity = 10;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && IsObject1Unhidden() && bulletsShot < bulletRemaining && canShoot)
        {
            ShootBullet();
            bulletsShot++; 
            bulletQuantity=bulletRemaining-bulletsShot;
            canShoot = false; 
            Invoke("EnableShooting", 1f);
        }

        if (Input.GetKeyDown(KeyCode.C) && IsPlayerNearBullet())
        {
            CollectBullets(10);
        }
    }

    bool IsObject1Unhidden()
    {
        return objectToCheck != null && objectToCheck.activeSelf;
    }

    void ShootBullet()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null && mainCamera != null)
        {
            if (fireEffect != null)
            {
                fireEffect.Play();
                if (shootSound != null)
                {
                    shootSound.Play();
                }
            }

            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

            Ray ray = mainCamera.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = (hit.point - bulletSpawnPoint.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));

                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
        }
        else
        {
            Debug.LogError("Bullet prefab, spawn point, or main camera is not assigned in the GunController!");
        }
    }

    bool IsPlayerNearBullet()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("BulletBox"))
            {
                return true; 
            }
        }
        return false; 
    }

    void CollectBullets(int amount)
    {
        bulletRemaining += amount;
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletBox");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    void EnableShooting()
    {
        canShoot = true; 
    }

    public int GetBulletRemaining()
    {
        bulletQuantity=bulletRemaining-bulletsShot;

        return bulletQuantity;
    }
}
