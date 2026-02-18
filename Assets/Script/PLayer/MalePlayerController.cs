using UnityEngine;

public class MalePlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Cannon cannon;

    [Header("Rotation Speeds")]
    [SerializeField] private float horizontalSpeed = 80f;
    [SerializeField] private float verticalSensitivity = 3f;

    [Header("Vertical Clamp")]
    [SerializeField] private float minVerticalAngle = -40f;
    [SerializeField] private float maxVerticalAngle = 60f;

    private float verticalRotation;
    private float horizontalRotation;

    private bool rotateLeft;
    private bool rotateRight;
    private bool isMobile;

    private void Start()
    {
        if(Application.isMobilePlatform)
        {
            isMobile = true;
            ConfigurAndroidInput();
        }else
        {
            isMobile = false;
        }
    }



    private void Update()
    {
        HandleHorizontalRotation();
        HandleVerticalRotation();
        HandleFireInput();
    }

    #region HORIZONTAL

    private void HandleHorizontalRotation()
    {
        if (isMobile)
        {
            if (rotateLeft)
                horizontalRotation -= horizontalSpeed * Time.deltaTime;

            if (rotateRight)
                horizontalRotation += horizontalSpeed * Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
                horizontalRotation -= horizontalSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.D))
                horizontalRotation += horizontalSpeed * Time.deltaTime;
        }

        cannon.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    // Called by UI buttons
    public void SetRotateLeft(bool value)
    {
        rotateLeft = value;
    }

    public void SetRotateRight(bool value)
    {
        rotateRight = value;
    }

    public void ConfigurAndroidInput()
    {
        var leftButton = GameAssets.Instance.maleLeftSideButton;
        var rightButton = GameAssets.Instance.maleRightSideButton;
        var fireButton = GameAssets.Instance.maleShootButton;

        leftButton.onPressed.AddListener(() => SetRotateLeft(true));
        leftButton.onReleased.AddListener(() => SetRotateLeft(false));

        rightButton.onPressed.AddListener(() => SetRotateRight(true));
        rightButton.onReleased.AddListener(() => SetRotateRight(false));

        fireButton.onPressed.AddListener(FireButtonPressed);
    }

    #endregion

    #region VERTICAL

    private void HandleVerticalRotation()
    {
        float inputY = 0f;

        if (!isMobile)
        {
            if(Cursor.lockState != CursorLockMode.Locked) return;
            inputY = Input.GetAxis("Mouse Y") * verticalSensitivity * 100f * Time.deltaTime;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    inputY = touch.deltaPosition.y * verticalSensitivity * Time.deltaTime;
                }
            }
        }

        verticalRotation -= inputY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    #endregion

    #region FIRE

    private void HandleFireInput()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
                cannon.Fire();
        }
    }

    public void FireButtonPressed()
    {
        cannon.Fire();
    }

    #endregion

    public void ListnToOnSensetivityValueChanged(Component sender,object data)
    {
        float value = (float)data;

        verticalSensitivity = value;
        horizontalSpeed =value * 30f;
    }
}
