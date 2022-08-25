using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should only store a reference to the player's data. This is an instance, but not a DDOL instance
/// </summary>
public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter instance;

    public float speed = 5;
    public float runSpeed = 2f;
    public float kingsBootsSpeed = 4f;
    public float jumpSpeed = 2;
    public float gravity = 0.1f;

    public bool useKingsBoots = false;
    public bool useKingsLance = false;
    public bool useKingsShield = false;
    public bool useKingsBow = false;

    public GameObject kingsBowPrefab;

    float yMomentum;

    Animator animator;

    CharacterController controller;

    bool kingsLanceRSlash;

    //Add King's Equipment actions (L slash, R slash, guard, bow, sprint)

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RunMovement();

        RunKingsEquipmentActions();
    }

    void RunKingsEquipmentActions()
    {
        RunKingsLance();
        RunKingsLance();
        RunKingsBow();
    }

    void RunKingsLance()
    {
        if (useKingsLance)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("Lance Slash Right", kingsLanceRSlash);

                animator.SetBool("Attacking", true);
                
                kingsLanceRSlash = !kingsLanceRSlash;
            } else
            {
                animator.SetBool("Attacking", false);
            }
        }
    }

    void RunKingsBow()
    {
        if (useKingsBow)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Transform newArrow = Instantiate(kingsBowPrefab, transform.position, Quaternion.identity).transform;
                newArrow.transform.LookAt(newArrow.transform.position + WorldCameraScript.instance.GetDirectionFacing());
            }
        }
    }

    void RunMovement()
    {
        float crouchSpeedMultiplier = 1;

        #region Crouch/Shield
        if (Input.GetButton("Crouch"))
        {
            crouchSpeedMultiplier = 0.5f;
            if (useKingsShield)
            {
                crouchSpeedMultiplier = 0.2f;
            }
        }
        #endregion

        #region Get Movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        movement = WorldCameraScript.instance.RotateVectorToCamera(movement);

        movement = movement.normalized;

        movement *= speed;

        if (Input.GetButton("Sprint"))
        {
            movement *= runSpeed;
            if (useKingsBoots)
            {
                movement *= kingsBootsSpeed;
            }
        }

        movement *= crouchSpeedMultiplier;
        
        #endregion

        #region Calculate Vertical Momentum
        if (controller.isGrounded)
        {
            if (yMomentum < 0)
            {
                yMomentum = 0;
            }
        }

        yMomentum -= gravity * Time.deltaTime;

        #region Jumping
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * 1.75f) &&
            Input.GetButtonDown("Jump"))
        {
            float finalJumpForce = jumpSpeed;
            if (Input.GetButton("Sprint"))
            {
                finalJumpForce += 5;
                if (useKingsBoots)
                {
                    finalJumpForce += 10;
                }
            }
            Jump(finalJumpForce);
        }
        #endregion

        movement.y = yMomentum;

        #endregion

        movement *= Time.deltaTime;

        controller.Move(movement);
    }

    public void Jump(float force)
    {
        yMomentum += force;
    }
}
