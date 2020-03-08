using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input Properties")]
    public float controlAreaSize;

    public float rotateSpeed;
    public float singleTouchAccelTime;
    public float accelFactor;

    public float dualTouchRotateSpeed;
    public float dualToSingleSlowdown;

    public float maxDoubleTapDuration;
    public float doubleTapVariance;

    [Header("Shooting Properties")]
    public ObjectPooler bulletPool;
    public float shootingCooldown;
    public Vector2 bulletOffset;

    private float lastShootTime = 0;


    [Header("Physics")]
    public float momentOfInertia;
    public float rotSpeedCap;

    private float rotInput;
    private float rotation;
    private float singleTouchBeginTime;

    private float tapTime;
    private float tapCount = 0;
    private Touch lastTouch;

    private float oldDeltaY = 0;
    private int oldTouchCount = 0;

    private bool letGoOfDualSteering = false;

    private bool isShooting;

    #region Touch Input Events

    public delegate void OnDoubleTapDelegate();
    public event OnDoubleTapDelegate OnDoubleTap;

    public delegate void OnBeginMovementDelegate();
    public event OnBeginMovementDelegate OnBeginMovement;

    public delegate void OnEndMovementDelegate();
    public event OnEndMovementDelegate OnEndMovement;

    public delegate void OnBeginDualSteeringDelegate();
    public event OnBeginDualSteeringDelegate OnBeginDualSteering;

    public delegate void OnEndDualSteeringDelegate();
    public event OnEndDualSteeringDelegate OnEndDualSteering;

    #endregion

    #region Generic Events
    public delegate void OnShootDelegate();
    public event OnShootDelegate OnShoot;
    #endregion


    private void Awake()
    {
        OnBeginDualSteering += EnableShooting;
        OnEndDualSteering += DisableShooting;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectGestures();
        GetInput();

        Move();

        //isShooting = Input.GetKey(KeyCode.Space);

        Shoot();
    }

    void EnableShooting()
    {
        isShooting = true;
    }

    void DisableShooting()
    {
        isShooting = false;
    }

    private void DetectGestures()
    {
        HandleDoubleTap();
    }

    private void HandleDoubleTap()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x > GameManager.Instance.screenWidth * controlAreaSize / 2
                && touch.position.x < GameManager.Instance.screenWidth - GameManager.Instance.screenWidth * controlAreaSize / 2
                && touch.phase == TouchPhase.Began)
                tapCount++;

            if (tapCount == 1)
            {
                tapTime = Time.time + maxDoubleTapDuration;
            }
            else if (tapCount == 2 && Time.time < tapTime)
            {
                if ((lastTouch.position - touch.position).magnitude < doubleTapVariance * Screen.dpi)
                    OnDoubleTap?.Invoke();

                tapCount = 0;
            }

            if (Time.time > tapTime)
                tapCount = 0;

            lastTouch = touch;
        }
    }

    private void GetInput()
    {
        int touchCount = Input.touchCount;

        if (oldTouchCount == 2 && touchCount <= 1)
        {
            OnEndDualSteering?.Invoke();
            letGoOfDualSteering = true;
        }

        if (touchCount == 1)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (oldTouchCount == 0) singleTouchBeginTime = Time.time;

            float preRotInput;

            if (touchPos.x < GameManager.Instance.screenWidth * controlAreaSize / 2)
                preRotInput = rotateSpeed;
            else if (touchPos.x > GameManager.Instance.screenWidth - GameManager.Instance.screenWidth * controlAreaSize / 2)
                preRotInput = -rotateSpeed;
            else
            {
                touchCount = 0;
                preRotInput = 0;
            }

            preRotInput *= (Time.time - singleTouchBeginTime) * singleTouchAccelTime + 1;

            if (letGoOfDualSteering)
                rotInput = Mathf.Lerp(rotInput, preRotInput, dualToSingleSlowdown * Time.deltaTime);
            else
                rotInput = preRotInput;

            if (rotInput != 0 && oldTouchCount == 0) OnBeginMovement?.Invoke();
        }
        else if (touchCount == 2)
        {
            if (oldTouchCount != 2)
            {
                if (oldTouchCount == 0) OnBeginMovement?.Invoke();
                OnBeginDualSteering?.Invoke();
            }

            Vector2[] touches = new Vector2[2];
            for (int i = 0; i < 2; i++)
                touches[i] = Input.GetTouch(i).position;

            if (touches[0].x > touches[1].x)
            {
                Vector2 tmp = touches[0];
                touches[0] = touches[1];
                touches[1] = tmp;
            }

            if (touches[0].x < GameManager.Instance.screenWidth * controlAreaSize / 2 
                && touches[1].x > GameManager.Instance.screenWidth - GameManager.Instance.screenWidth * controlAreaSize / 2)
            {
                float deltaY = (touches[0].y - touches[1].y) / GameManager.Instance.screenHeight;

                float acceleration = deltaY - oldDeltaY;

                rotInput = -deltaY * deltaY * dualTouchRotateSpeed * Mathf.Sign(deltaY) - acceleration * accelFactor;

                oldDeltaY = deltaY;
            }
            else
            {
                if (touches[0].x < GameManager.Instance.screenWidth * controlAreaSize / 2) rotInput = -rotateSpeed;
                else if (touches[1].x > GameManager.Instance.screenWidth - GameManager.Instance.screenWidth * controlAreaSize / 2) rotInput = rotateSpeed;
                else rotInput = 0;
            }

        }
        else
        {
            rotInput = 0;
            letGoOfDualSteering = false;
            OnEndMovement?.Invoke();
        }

        oldTouchCount = touchCount;
    }

    private void Move()
    {
        if (rotInput == 0)
            rotation = Mathf.Lerp(rotation, 0, momentOfInertia * Time.deltaTime);
        else 
            rotation = Mathf.Lerp(rotation, rotInput, 5 * momentOfInertia * Time.deltaTime);

        rotation = Mathf.Clamp(rotation, -rotSpeedCap, rotSpeedCap);

        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }

    private void Shoot()
    {
        if (isShooting)
        {
            if (Time.time > lastShootTime + shootingCooldown)
            {
                lastShootTime = Time.time;

                OnShoot?.Invoke();

                GameObject bullet = bulletPool.ActivateObject();
                if (bullet == null) return;

                BulletMovement bulletMovement = bullet.GetComponent<BulletMovement>();

                bullet.transform.position = transform.position + new Vector3(bulletOffset.x * Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), bulletOffset.y * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
                bulletMovement.angle = transform.rotation.eulerAngles.z;
                bulletMovement.Move();
            }

        }
    }
}
