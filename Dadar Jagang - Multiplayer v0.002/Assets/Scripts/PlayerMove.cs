using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    public float moveSpeed, gravityForce, jumpForce;
    public CharacterController characterController;
    private Vector3 moveInput;
    public Transform cameraTransform;
    public float mouseSensivity;
    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask ground;
    public Transform firePoint;

    // Update is called once per frame
    void Update()
    {
        // Cek apakah ini milik client lokal
        if (!IsOwner) return;

        float yVelocity = moveInput.y;
        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horizontalMove + verticalMove;
        moveInput.Normalize();
        moveInput *= moveSpeed;

        moveInput.y = yVelocity;
        moveInput.y = Physics.gravity.y * gravityForce * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveInput.y += Physics.gravity.y * gravityForce * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.2f, ground).Length > 0;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpForce;
        }

        characterController.Move(moveInput * Time.deltaTime);

        // Rotasi berdasarkan input mouse
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        // Menembak
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if (IsOwner)
        {
            Vector4 rot = new Vector4(firePoint.rotation.x, firePoint.rotation.y, firePoint.rotation.z, firePoint.rotation.w);
            GameSceneManager.instance.RequestSpawnBulletServerRpc(firePoint.position, rot,NetworkManager.Singleton.LocalClientId);
            // Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
}
