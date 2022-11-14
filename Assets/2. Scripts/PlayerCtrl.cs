using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static bool isActivated = true;
    
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float swimFastSpeed;
    [SerializeField] private float upSwimSpeed;
    private float applySpeed;

    [SerializeField] private float jumpForce;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    private Vector3 lastPos;

    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    [SerializeField] private CapsuleCollider capsuleCollider;

    [SerializeField] private float LookSensitivity;

    [SerializeField] private float CameraRotationLimit;
    private float CurrentCameraRotationX = 0f;

    [SerializeField] private Camera TheCamera;
    private Rigidbody myRigid;
    
    private GunCtrl theGunCtrl;
    private CrossHair theCrosshair;
    private StatusCtrl theStatusCtrl;

    private bool pauseCameraRotation = false;
    

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunCtrl = FindObjectOfType<GunCtrl>();
        theCrosshair = FindObjectOfType<CrossHair>();
        theStatusCtrl = FindObjectOfType<StatusCtrl>();

        applySpeed = walkSpeed;
        originPosY = TheCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();
            TryJump();
            if (!GameManager.isWater)
            {
                TryRun();
            }
            TryCrouch();
            Move();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void WaterCheck()
    {
        if (GameManager.isWater)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                applySpeed = swimFastSpeed;
            else
                applySpeed = swimSpeed;
        }
    }

    private void FixedUpdate()
    {
        MoveCheck();        
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusCtrl.GetCurrentSP() > 0 && !GameManager.isWater)
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
            UpSwim();
    }

    private void UpSwim()
    {
        myRigid.velocity = transform.up * upSwimSpeed;
    }


    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }

    private void Jump()
    {
        if (isCrouch)
            Crouch();

        theStatusCtrl.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = TheCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.05f);
            TheCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 20)
                break;
            yield return null;
        }
        TheCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusCtrl.GetCurrentSP() > 0)
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusCtrl.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunCtrl.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusCtrl.DecreaseStamina(1);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirx = Input.GetAxis("Horizontal");
        float _moveDirz = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirx;
        Vector3 _moveVertical = transform.forward * _moveDirz;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && !isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    private void CameraRotation()
    {
        if (!pauseCameraRotation)
        {
            float _xRotation = Input.GetAxis("Mouse Y");
            float _CameraRotationX = _xRotation * LookSensitivity;
            CurrentCameraRotationX -= _CameraRotationX;
            CurrentCameraRotationX = Mathf.Clamp(CurrentCameraRotationX, -CameraRotationLimit, CameraRotationLimit);

            TheCamera.transform.localEulerAngles = new Vector3(CurrentCameraRotationX, 0f, 0f);
        }        
    }

    public IEnumerator TreeLookCoroutine(Vector3 _target)
    {
        pauseCameraRotation = true;

        Quaternion direction = Quaternion.LookRotation(_target - TheCamera.transform.position);
        Vector3 eulerValue = direction.eulerAngles;
        float destinationX = eulerValue.x;

        while (Mathf.Abs(destinationX - CurrentCameraRotationX) >= 0.5f)
        {
            eulerValue = Quaternion.Lerp(TheCamera.transform.localRotation, direction, 0.3f).eulerAngles;
            TheCamera.transform.localRotation = Quaternion.Euler(eulerValue.x, 0f, 0f);
            CurrentCameraRotationX = TheCamera.transform.localEulerAngles.x;
            yield return null;
        }

        pauseCameraRotation = false;
    }

    private void CharacterRotation()
    {
        float _yRotion = Input.GetAxis("Mouse X");
        Vector3 _CharacterRotationY = new Vector3(0f, _yRotion, 0f) * LookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_CharacterRotationY));
    }
}
