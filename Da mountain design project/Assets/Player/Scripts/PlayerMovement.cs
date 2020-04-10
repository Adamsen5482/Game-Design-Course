using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    // Movement Properties
    [Header("Player Movement Properties")]
    public float speed = 6.0f;
    public float fallspeed = 100f;
    public float gravity = 20.0f;
    public float jumpSpeed = 8.0f;
    // Camera Properties
    [Header("Camera Properties")]
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;
    bool isSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            // When grounded recalculating the move direction based on axes.
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButtonDown("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

        SlopeDirection();
    }


    private void SlopeDirection()
    {
        // Raycast with infinite distance to check the slope directly under the player no matter where they are
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, Mathf.Infinity);

        // Saving the normal
        Vector3 n = hit.normal;

        // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
        Vector3 groundParallel = Vector3.Cross(transform.up, n);

        // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
        Vector3 slopeParallel = Vector3.Cross(groundParallel, n);
        Debug.DrawRay(hit.point, slopeParallel * 10, Color.green);

        // Just the current angle we're standing on
        float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, transform.up));
        Debug.Log(currentSlope);
        
        // If the slope is on a slope too steep and the player is Grounded the player is pushed down the slope.
        if (currentSlope >= 45f && controller.isGrounded)
        {
            isSliding = true;
            transform.position += (slopeParallel.normalized / 2) * fallspeed;
        }

        // If the player is standing on a slope that isn't too steep, is grounded, as is not sliding anymore we start a function to count time
        else if (currentSlope < 45 && controller.isGrounded && isSliding)
        {
            //TimePassed();

            // If enough time has passed the sliding stops. There's no need for these last two if statements, the thing works already, but it's nicer to have the player slide for a little bit more once they get back on the ground
           // if (currentSlope < 45 && MaintainingGround() && isSliding && timePassed > 1f)
           // {
           //     isSliding = false;
           // }
        }
    }
}
