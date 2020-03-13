using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region binding IDs
    private const string mouseXInput = "Mouse X";
    private const string mouseYInput = "Mouse Y";
    private const string zoomInput = "Mouse ScrollWheel";
	private const string horizontalInput = "Horizontal";
	private const string verticalInput = "Vertical";
    private const string resetView = "Reset View";
	#endregion binding IDs

	[Header("Camera Preferences")]
    [Tooltip("Target to follow")] public GameObject cameraTarget;

    [SerializeField] private float horizontalSensitivity = 1.25f;
    [SerializeField] private float verticalSensitivity = 0.75f;
    [Tooltip("Invert looking up/down.")]
    [SerializeField] private bool invertY = false;

    [SerializeField, Range(1, 10)] private float maxZoomOut = 6f;
    [SerializeField] private float zoomSensitivity = 3f;

    [Tooltip("Cap viewing angle on the Y axis.")]
    [SerializeField, Range(0f, 90f)] private float maxLookDownAngle = 70f;
    [SerializeField, Range(0f, 90f)] private float maxLookUpAngle = 45f;
        
    [Header("Smoothing (lower = smoother camera).")]
    [SerializeField, Range(2f, 40f)] private float smoothness = 14f;
    [SerializeField] private bool useSmoothing = true;

    private float cameraOffset;
    private float mouseX;
    private float mouseY;
    private float zoom;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraOffset = Vector3.Distance(transform.position, cameraTarget.transform.position);
    }

    private void Update()
    {
        CamerInput();
	}

    private void LateUpdate()
    {
        CameraUpdate();
        ObstacleCheck();
    }

    private void CamerInput()
    {
		zoom += Input.GetAxis(zoomInput) * zoomSensitivity;
		zoom = Mathf.Clamp(zoom, -maxZoomOut, cameraOffset);

		mouseX = Input.GetAxis(mouseXInput) * horizontalSensitivity;
		mouseY += (!invertY) ? Input.GetAxis(mouseYInput) * verticalSensitivity : Input.GetAxis(mouseYInput) * -verticalSensitivity;
		mouseY = Mathf.Clamp(mouseY, -maxLookDownAngle, maxLookUpAngle);

		if (Input.GetButton(resetView) && Input.GetAxis(horizontalInput) == 0f && Input.GetAxis(verticalInput) == 0f)
		{
            LookInPlayerDirection();
		}
	}

    private void CameraUpdate()
    {
        transform.eulerAngles = new Vector3(-mouseY, transform.eulerAngles.y + mouseX, 0);

        transform.position = (!useSmoothing) ? cameraTarget.transform.position - (transform.forward * (cameraOffset - zoom)) :
            Vector3.Slerp(transform.position, cameraTarget.transform.position - (transform.forward * (cameraOffset - zoom)), smoothness * Time.fixedDeltaTime);
	}

    void ObstacleCheck()
    {
        float currentCameraOffset = Vector3.Distance(cameraTarget.transform.position, transform.position);
        float sphereRadius = 0.12f;

        if (Physics.SphereCast(cameraTarget.transform.position, sphereRadius,transform.position - cameraTarget.transform.position, out RaycastHit hit, currentCameraOffset))
        {
            transform.position = hit.point;
        }
	}

    void LookInPlayerDirection()
    {
        float cameraResetSpeed = 8f;
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTarget.GetComponent<PlayerController>().playerModel.transform.rotation, cameraResetSpeed * Time.deltaTime);
    }
}