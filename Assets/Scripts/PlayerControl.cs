using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    public float maxDistance = 10f;
    public float maxViewDistance = 100f;
    public float fieldOfViewAngle = 60f;
    private float interval = 2f;
    private float timer = 0f;
    public Camera mainCamera;
    public MotorImagery childActivator;
    public ScoreSystem score;
    public WeaponSwitching activeWeaponSwitching;
    public WeaponSwitching weaponSwitchingHand;
    public WeaponSwitching weaponSwitchingController;

    public AudioSource audio1;


    void Start()
    {
    }

    void Update()
    {
        DetermineActiveWeaponSwitching();

        // Example input to switch weapons on the active weapon holder
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (activeWeaponSwitching != null)
            {
                activeWeaponSwitching.SetSelectedWeapon(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (activeWeaponSwitching != null)
            {
                activeWeaponSwitching.SetSelectedWeapon(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (activeWeaponSwitching != null)
            {
                activeWeaponSwitching.SetSelectedWeapon(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Assuming the first child is at index 0
            // AOE
            childActivator.ActivateChild(0, 16);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Assuming the second child is at index 1
            childActivator.ActivateChild(1, 20);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeClosestTargetInvisibleAndBack();
        }

        // Accumulate time
        timer += Time.deltaTime;

        // Check if the timer has reached the interval
        if (timer >= interval)
        {
            // Reset the timer
            timer = 0f;
            FindClosestTargetAndFlash();
        }

        // find flashing targets and kill
        if (Input.GetKeyDown(KeyCode.F))
        {
            MakeFlashingTargetsInvisible();
        }
    }

    public void MakeFlashingTargetsInvisible()
    {

        // Find all game objects with the Target script attached
        Target[] targets = FindObjectsOfType<Target>();

        // Check if IsFlashing is true for each target
        foreach (Target target in targets)
        {
            if (target.IsFlashing)
            {
                //flashingTargets.Add(target.gameObject);
                target.GetComponent<Target>().MakeInvisibleAndBack();
                // audioSource.PlayOneShot(explosionSound);
                audio1.Play();
                int scoreToAdd = 0;
                if (target.CompareTag("Robot"))
                {
                    scoreToAdd = 120; // Score value for Robot
                }
                else if (target.CompareTag("Drone"))
                {
                    scoreToAdd = 240; // Score value for Drone
                }

                target.GetComponent<Target>().MakeInvisibleAndBack();
                score.AddScore(scoreToAdd);
            }

        }

    }

    void FindClosestTargetAndFlash()
    {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        GameObject[] targets = new GameObject[robots.Length + drones.Length];
        robots.CopyTo(targets, 0);
        drones.CopyTo(targets, robots.Length);
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        GameObject closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            Vector3 directionToTarget = target.transform.position - cameraPosition;
            float angle = Vector3.Angle(directionToTarget, cameraForward);

            // Check if the angle is within the range of -90 to 90 degrees
            if (angle >= -90f && angle <= 90f)
            {
                float sqrDistanceToTarget = directionToTarget.sqrMagnitude;

                // Update the closest target if necessary
                if (sqrDistanceToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = sqrDistanceToTarget;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            closestTarget.GetComponent<Target>().Flash();
        }

    }

    void MakeClosestTargetInvisibleAndBack()
    {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        GameObject[] targets = new GameObject[robots.Length + drones.Length];
        robots.CopyTo(targets, 0);
        drones.CopyTo(targets, robots.Length);
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        GameObject closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        float smallestAngle = 180f;


        foreach (GameObject target in targets)
        {
            Vector3 directionToTarget = target.transform.position - cameraPosition;
            float angle = Vector3.Angle(directionToTarget, cameraForward);
            float sqrDistanceToTarget = directionToTarget.sqrMagnitude;

            if (angle <= smallestAngle)
            {
                if (angle < smallestAngle || (angle == smallestAngle && sqrDistanceToTarget < closestDistanceSqr))
                {
                    smallestAngle = angle;
                    closestDistanceSqr = sqrDistanceToTarget;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            int scoreToAdd = 0; // Default score to add
            if (closestTarget.CompareTag("Robot"))
            {
                scoreToAdd = 120; // Score value for Robot
            }
            else if (closestTarget.CompareTag("Drone"))
            {
                scoreToAdd = 240; // Score value for Drone
            }

            closestTarget.GetComponent<Target>().MakeInvisibleAndBack();
            score.AddScore(scoreToAdd);
        }
    }

    void DetermineActiveWeaponSwitching()
    {
        // Check if the controller weapon holder is active, and assign it as the active weapon switching component
        if (weaponSwitchingController != null && weaponSwitchingController.gameObject.activeInHierarchy)
        {
            activeWeaponSwitching = weaponSwitchingController;
        }
        // Otherwise, check if the hand weapon holder is active
        else if (weaponSwitchingHand != null && weaponSwitchingHand.gameObject.activeInHierarchy)
        {
            activeWeaponSwitching = weaponSwitchingHand;
        }
        else
        {
            activeWeaponSwitching = null; // No active weapon holder found
        }
    }
}

