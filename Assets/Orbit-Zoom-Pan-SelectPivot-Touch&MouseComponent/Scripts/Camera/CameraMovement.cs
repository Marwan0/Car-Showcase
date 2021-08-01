// Marmoset Skyshop
// Copyright 2013 Marmoset LLC
// http://marmoset.co

using UnityEngine;
using System.Collections;
using Lean;
using Lean.Touch;

public class CameraMovement : MonoBehaviour
{

	[Header("--- Speed ---")] 
	[Space(10F)]

	public float thetaSpeed = 100.0f;

	public float rotationSpeed = 100.0f;

	public float moveSpeed = 3.0f;

	public float zoomSpeed = 30.0f;

	[Space(10F)]
	[Header("--- Panning ---")] 
	[Space(10F)]

	public bool useMoveBounds = false;

	[Space(10F)]

	public float moveBoundMinX = 5f;
	public float moveBoundMaxX = 5f;

	[Space(10F)]

	public float moveBoundMinY = 5f;
	public float moveBoundMaxY = 5f;

	[Space(10F)]

	public float moveBoundMinZ = 5f;
	public float moveBoundMaxZ = 5f;

	[Space(10F)]

	public float moveSmoothing = 5f;

	public float moveSmoothingFocus = 5f;

	[Space(10F)]
	[Header("--- Rotation ---")] 
	[Space(10F)]

	public float rotationBoundMinX = -180f;
	public float rotationBoundMaxX = 180f;

	[Space(10F)]

	public float rotationBoundMinY = -180f;
	public float rotationBoundMaxY = 180f;

	[Space(10F)]

	public float rotateSmoothing = 3;

	public float rotateSmoothingFocus = 4f;

	[Space(10F)]
	[Header("--- Zoom ---")] 
	[Space(10F)]

	public float zMinLimitTemp;
	public float zMaxLimitTemp;

	[Space(10F)]

	public float zMinLimit = 5;
	public float zMaxLimit = 30;

	[Space(10F)]

	public float zoomSmooth=0.7f;

	[Space(10F)]
	[Header("--- Offset ---")] 
	[Space(10F)]

	public float zOffset = 5;

	[Space(10F)]

	public float offsetRotataionY = 25;

	[Space(10F)]

	public float offsetRotataionX = 25;

	[Space(10F)]

	public float smoothTime = 3.0f;

	[Space(10F)]

	public AnimationCurve pitchCurve;

	[Space(10F)]

	public float t = 0;

	[Space(10F)]

	public Rect paramInputBounds = new Rect(0, 0, 1, 1);

	[Space(10F)]

	public bool usePivotPoint;

	[Space(10F)]

	public Vector3 pivotPoint = new Vector3(0, 2, 0);

	[Space(10F)]

	public Transform pivotTransform;

	[Space(10F)]

	public bool start = false;

	[Space(10F)]

	public float startPostion = 5;

	[Space(10F)]

	public float EndstartPostion = 5;

	[Space(10F)]

	public  bool allowRotation = true;

	[Space(10F)]

	public bool allowPanning = true;

	Vector3 targetLookAtOrigin;
	Vector3 targetPosi;
	Quaternion targetRota;
	bool focus;
	bool startFocus;



	float dx;
	float dy;
	bool rotInput = false;
	bool skyInput = false;
	bool panInput = false;
	bool zoomInput = false;


	private Vector3 velocity = Vector3.zero;

	private Vector2 euler;

	private Quaternion targetRot;
	private Vector3 targetLookAt;
	private float targetDist;
	private Vector3 distanceVec = new Vector3(0, 0, 0);

	private Transform target;
	private Rect inputBounds;


	#if UNITY_IPHONE || UNITY_ANDROID
	private bool firstTouch = true;
	#endif
	public void Awake()
	{
		//Subscribe to LeanTouch Events
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//LeanTouch.OnSoloDrag += OnSingleFingerDrag;
			//LeanTouch.OnMultiDrag += OnMultiFingersDrag;
			//LeanTouch.OnPinch += OnPinch;
			//LeanTouch.OnFingerUp += OnFingerUp;
		}

		zMinLimitTemp = zMinLimit;
		zMaxLimitTemp = zMaxLimit;
		distanceVec = new Vector3(0,0,startPostion);
		start = true;
	}
	public void Start()
	{
		// pitchCurve = AnimationCurve.EaseInOut(0.0f, offsetRotataionX, 1.0f, 90.0f);

		target = pivotTransform;

		targetRot = transform.rotation;
		targetLookAt = target.position;
		#if UNITY_IPHONE || UNITY_ANDROID
		firstTouch = true;
		#endif
	}

	public void Update()
	{
		//NOTE: mouse coordinates have a bottom-left origin, camera top-left

		if (target)
		{
			dx = Input.GetAxis("Mouse X");
			dy = Input.GetAxis("Mouse Y");

			bool click1 = Input.GetMouseButton(0);//|| Input.touchCount == 1;
			bool click2 = Input.GetMouseButton(1);//|| Input.touchCount == 2;
			bool click3 = Input.GetMouseButton(2);//|| Input.touchCount == 3;
			//bool click4 = Input.touchCount >= 4;
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				rotInput = click1;
				skyInput = /*click4 || */click1 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				panInput = click3 || click1 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
				zoomInput = click2;
			}

			if (skyInput)
			{
				dx = dx * thetaSpeed * 0.02f;
			}
			else if (panInput && allowPanning)
			{

				dx = dx * moveSpeed * 0.005f * targetDist;
				dy = dy * moveSpeed * 0.005f * targetDist;
				targetLookAt -= transform.up * dy + transform.right * dx;
				if (useMoveBounds)
				{
					targetLookAt.x = Mathf.Clamp(targetLookAt.x, targetLookAtOrigin.x-moveBoundMinX,  targetLookAtOrigin.x+moveBoundMaxX);
					targetLookAt.y = Mathf.Clamp(targetLookAt.y, targetLookAtOrigin.y-moveBoundMinY,  targetLookAtOrigin.y+moveBoundMaxY);
					targetLookAt.z = Mathf.Clamp(targetLookAt.z, targetLookAtOrigin.z-moveBoundMinZ,  targetLookAtOrigin.z+moveBoundMaxZ);
				}
			}
			else if (zoomInput )
			{
				//dy = dy * zoomSpeed * 0.005f * targetDist;
				//targetDist += dy;
				//targetDist = Mathf.Max(0.1f, targetDist);
			}
			else if (rotInput && allowRotation)
			{
				dx = dx * thetaSpeed * 0.02f;
				dy = dy * rotationSpeed * 0.02f;
				if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
				{
					euler.x += dx;
					euler.y -= dy;
					euler.y = ClampAngle(euler.y, rotationBoundMinY, rotationBoundMaxY);
					//new update
					euler.x = ClampAngle(euler.x, rotationBoundMinX, rotationBoundMaxX);
					targetRot = Quaternion.Euler(euler.y + offsetRotataionX, euler.x + offsetRotataionY, 0);
				}
			}
			else if (focus)
			{
				targetRot = Quaternion.Slerp(targetRot, targetRota, Time.fixedTime * rotateSmoothingFocus);
				targetLookAt = Vector3.Lerp(targetLookAt, targetPosi, Time.fixedTime * moveSmoothingFocus);
				startFocus = true;
				if (targetLookAt == targetPosi)
				{
					focus = false;
				}

			}
		}
	}

	public void FixedUpdate()
	{

		if (targetDist < zMinLimit)
		{
			targetDist = zMinLimit;
		}

		if (targetDist > zMaxLimit)
		{
			targetDist = zMaxLimit;
		}

		targetDist -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 0.5f;
		//  distance = zoomSmooth * targetDist + (1 - zoomSmooth) * distance;
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotateSmoothing);

		target.position = Vector3.Lerp(target.position, targetLookAt, Time.fixedDeltaTime * moveSmoothing);
		//old code	distanceVec.z = distance;
		//new code 
		//zoom limit
		if (start)
		{
			transform.position = new Vector3(target.position.x,target.position.y,startPostion);
			StartCoroutine(flyStart ());
		}
		if (startFocus)
		{

			StartCoroutine(focusAndZoom());

		}
		else
			distanceVec.z =  Mathf.Lerp(distanceVec.z,Mathf.Clamp(targetDist,zMinLimit,zMaxLimit),Time.fixedDeltaTime*moveSmoothing);

		transform.position = target.position - transform.rotation *  distanceVec;

	}

	public void gotoTarget(Transform _target)
	{
		///   targetRot=Quaternion.identity;

		//   float targetRotX = pitchCurve.Evaluate(t);
		//  targetRota = Quaternion.Euler(targetRotX, offsetRotataionY, 0.0f);
		targetRota = Quaternion.Euler(offsetRotataionX, offsetRotataionY, 0.0f);


		Vector3 offset = new Vector3(0.0f, 0f,0f);
		//     Debug.Log(offset);
		targetPosi = _target.position - targetRot *  offset;
		targetLookAtOrigin = new Vector2(targetPosi.x, targetPosi.y);
		//StartCoroutine(fly(targetLookAt, targetRot));
		focus = true;

	}


	static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f) angle += 360f;
		if (angle > 360f) angle -= 360f;
		return Mathf.Clamp(angle, min, max);
	}

	IEnumerator flyStart()
	{

		zMinLimit = EndstartPostion;
		distanceVec.z = Mathf.Lerp(distanceVec.z, ClampAngle(targetDist, zMinLimit, zMaxLimit), Time.deltaTime * moveSmoothing);
		yield return new WaitForSeconds(1f);
		zMinLimit = zMinLimitTemp;
		start = false;
	}
	IEnumerator focusAndZoom()
	{

		zMinLimit = zOffset;
		zMaxLimit = zOffset;
		distanceVec.z = Mathf.Lerp(distanceVec.z, ClampAngle(targetDist, zMinLimit, zMaxLimit), Time.deltaTime * moveSmoothing);
		yield return new WaitForSeconds(0.2f);
		//   transform.position = target.position - transform.rotation * distanceVec;
		zMinLimit = zMinLimitTemp;
		zMaxLimit = zMaxLimitTemp;
		startFocus = false;
	}

	#region LeanTouch
	void OnSingleFingerDrag(Vector2 pixels)
	{
		if (LeanTouch.Fingers.Count > 1)
			return;

		panInput = false;
		rotInput = true;
		dx = Input.GetTouch(0).deltaPosition.x;
		dy = Input.GetTouch(0).deltaPosition.y;
	}

	void OnMultiFingersDrag(Vector2 pixels)
	{
		if(LeanTouch.Fingers.Count<3)
		{
			return;
		}
		rotInput = false;
		panInput = true;
		dx = Input.GetTouch(0).deltaPosition.x;
		dy = Input.GetTouch(0).deltaPosition.y;
	}

	void OnPinch(float scale)
	{
		rotInput = false;

		if(scale > 1)//Zoom in
			targetDist -= (scale - 1) * zoomSpeed * 0.5f;
		else//Zoom out
			targetDist += (1 - scale) * zoomSpeed * 0.5f;
	}

	void OnFingerUp(LeanFinger finger)
	{
		if(finger.Index == 0)
		{
			rotInput = false;
		}
		else
		{
			panInput = false;
		}
	}
	#endregion

	void OnDisable()
	{
		//Unsubscribe to LeanTouch Events
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//LeanTouch.OnSoloDrag -= OnSingleFingerDrag;
			//LeanTouch.OnMultiDrag -= OnMultiFingersDrag;
			//LeanTouch.OnPinch -= OnPinch;
			//LeanTouch.OnFingerUp -= OnFingerUp;
		}
	}
}
