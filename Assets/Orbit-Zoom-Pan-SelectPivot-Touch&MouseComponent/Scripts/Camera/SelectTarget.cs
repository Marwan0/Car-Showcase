using UnityEngine;
using System.Collections;

public class SelectTarget : MonoBehaviour {
	
	public Transform target;

	[Header("--- Roatation ---")] 
	[Space(10F)]

	public bool allowRotation;

	[Space(10F)]

	public float yMinRotationLimit;
	public float yMaxRotationLimit;
	
	public float xMinRotationLimit;
	public float xMaxRotationLimit;

	[Header("--- Panning ---")] 
	[Space(10F)]

	public bool allowPanning;

	[Space(10F)]

	public float moveMinY;
	public float moveMaxY;
	public float moveMinX;
	public float moveMaxX;
	public float moveMinZ;
	public float moveMaxZ;

	[Header("--- Zoom ---")] 
	[Space(10F)]
	
	public float zMinLimit;
	public float zMaxLimit;

	[Header("--- Offset ---")] 
	[Space(10F)]

	public float zOffset;
	public float offsetRotataionY;
	public float offsetRotataionX;

	[Space(10F)]
	[Header("--- Camera ---")] 
	[Space(10F)]

	public GameObject Camera;

	CameraMovement mCamera;

	void Start()
	{

		target = GameObject.Find (this.name).transform;

		Camera = GameObject.Find ("FixedCamera");

		mCamera = Camera.GetComponent<CameraMovement> ();
	}
	
	public void selectTarget()
	{

		if (allowPanning == true) {
			
			Camera.GetComponent<CameraMovement> ().allowPanning = true;

		} else {
			
			Camera.GetComponent<CameraMovement> ().allowPanning = false;
		}

		if (allowRotation == true) {
			
			Camera.GetComponent<CameraMovement> ().allowRotation = true;

		} else {
			
			Camera.GetComponent<CameraMovement> ().allowRotation = false;
		}

		/*objmed.selectCurrentTarget(target,yMinRotationLimit,yMaxRotationLimit,
		                           xMinRotationLimit,xMaxRotationLimit, 
		                           moveMinY,  moveMaxY,
		                           moveMinX,  moveMaxX,
		                           moveMinZ,  moveMaxZ,
		                           zMinLimit,zMaxLimit,zOffset,
		                           offsetRotataionY,offsetRotataionX);*/

		mCamera.rotationBoundMinX = xMinRotationLimit;
		mCamera.rotationBoundMaxX = xMaxRotationLimit;
		mCamera.rotationBoundMinY = yMinRotationLimit;
		mCamera.rotationBoundMaxY = yMaxRotationLimit;
		mCamera.zOffset = zOffset;
		mCamera.moveBoundMaxX = moveMaxX;
		mCamera.moveBoundMinX = moveMinX;
		mCamera.moveBoundMaxY = moveMaxY;
		mCamera.moveBoundMinY = moveMinY;
		mCamera.moveBoundMaxZ = moveMaxZ;
		mCamera.moveBoundMinZ = moveMinZ;


		mCamera.zMinLimit = zMinLimit;
		mCamera.zMinLimitTemp = zMinLimit;
		mCamera.zMaxLimit = zMaxLimit;
		mCamera.zMaxLimitTemp = zMaxLimit;
		mCamera.offsetRotataionX = offsetRotataionX;
		mCamera.offsetRotataionY = offsetRotataionY;
		mCamera.gotoTarget(target);


	}	
}
