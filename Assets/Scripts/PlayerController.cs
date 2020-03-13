using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	#region binding IDs
	private const string horizontal = "Horizontal";
	private const string vertical = "Vertical";
	#endregion binding IDs
	
	[Tooltip("The camera following this target")]
	public Transform theCamera;
	public GameObject playerModel;

	[Tooltip("How fast the player rotates towards a new direction")]
	[SerializeField] private float turnSpeed = 15f;
	[SerializeField] private float walkSpeed = 6f;

	[Tooltip("PlayerModel transparency if too close to the camera")]
	[SerializeField] private float distanceToTransparent = 1.5f;
	[SerializeField, Range(0.01f, 1f)] private float transparency = 0.25f;

	private CharacterController characterController;
	private Component[] meshRenderers;
	private Vector3 moveDirection;

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}

	private void Update()
	{
		PlayerMovement();
		ChangeColorAlpha();
	}

	void PlayerMovement()
	{
		moveDirection = transform.forward * Input.GetAxis(vertical) + transform.right * Input.GetAxis(horizontal);
		moveDirection = moveDirection.normalized * walkSpeed;
		characterController.Move(moveDirection * Time.deltaTime);

		if (Input.GetAxis(horizontal) != 0 || Input.GetAxis(vertical) != 0)
		{
			transform.rotation = Quaternion.Euler(0f, theCamera.rotation.eulerAngles.y, 0f);
			Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
			playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, turnSpeed * Time.deltaTime);
		}
	}

	private void ChangeColorAlpha()
	{
		
		if (Vector3.Distance(theCamera.transform.position, transform.position) < distanceToTransparent)
		{
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				Color newColor = meshRenderer.material.color;
				newColor.a = transparency;
				meshRenderer.material.color = newColor;
			}
		}
		else
		{
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				Color newColor = meshRenderer.material.color;
				newColor.a = 1f;
				meshRenderer.material.color = newColor;
			}
		}
	}
}