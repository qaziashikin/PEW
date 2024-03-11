using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public bool isPistol = false;

    public ParticleSystem muzzleFlash;
    public ScoreSystem score;

    private Transform shootingPoint;
    private float nextTimeToFire = 0f;
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public GameObject bulletPrefab; 
    public float shootForce;

    void Start()
    {
        // Find the shooting point child object
        shootingPoint = transform.Find("ShootingPoint");
    }

    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;

        // Set the Z component of the rotation to 0
        currentRotation.z = 0;

        // Apply the modified rotation back to the transform
        transform.eulerAngles = currentRotation;

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        if (isPistol){
            audioSource1.Play();
        } else {
            audioSource2.Play();
        }
        
        // Check if the shooting point is found
        if (shootingPoint != null)
        {

            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(shootingPoint.forward * shootForce, ForceMode.Impulse);
            RaycastHit hit;

            // Cast ray from shooting point's position and forward direction
            if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, range))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null && hit.collider.CompareTag("Target") || hit.collider.CompareTag("Robot"))
                {
                    // target.Flash();
                    target.MakeInvisibleAndBack();
                    // Call AddScore on the player script, passing damage as points
                    score.AddScore((int)damage);
                }
            }
        }
    }
}


