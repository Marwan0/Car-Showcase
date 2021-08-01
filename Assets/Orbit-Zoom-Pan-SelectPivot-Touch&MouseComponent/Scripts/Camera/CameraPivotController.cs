using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using DG.Tweening;

public class CameraPivotController : MonoBehaviour
{
    LeanSelect leanSelect;
    Transform selectedObject;

    LeanPitchYaw leanPitchYaw;
    LeanPinchCamera leanPinchCamera;


    public Ease CameraMovementAnimationEase;
    public float CameraMovementSpeed = 1;
    public float CameraresetPanMovementSpeed = 1;
    public float CameraAutoRotationSpeed = 100;
    public float CameraZoomResetSpeed = 100;

    public void Awake()
    {
        leanSelect = FindObjectOfType<LeanSelect>();
        leanPitchYaw = GetComponent<LeanPitchYaw>();
        leanPinchCamera = Camera.main.GetComponent<LeanPinchCamera>();

        leanSelect.OnObjectSelection += focusOnThis;
        leanSelect.OnAllDeselection += resetFocus;
    }
    //private void LateUpdate()
    //{
    //    if (selectedObject)
    //    {
    //        this.transform.position = selectedObject.position;
    //    }
    //}

    public void focusOnThis(Transform selectedTransform)
    {
        RotationPrefs tempPrefs = null;
        if (selectedObject != null)
        {
            var selectedObjectPrefs = selectedObject.GetComponent<SelectableItemPrefs>();
            if (selectedObjectPrefs)
            {
                tempPrefs = selectedObjectPrefs.OriginRotationPrefs;
            }
        }


        selectedObject = selectedTransform;
        transform.DOMove(selectedObject.position, CameraMovementSpeed, false).SetSpeedBased().SetEase(CameraMovementAnimationEase).OnComplete(OnReachSelected);

        var selectedItemPrefs = selectedObject.GetComponent<SelectableItemPrefs>();
        if (selectedItemPrefs)
        {
            if (selectedItemPrefs.isOriginPrefsInitialized)
            {
                if (tempPrefs != null)
                    selectedItemPrefs.initializeOriginPose(tempPrefs);
            }
            else
            {
                selectedItemPrefs.initializeOriginPose(leanPitchYaw);
            }
            applyPose(selectedItemPrefs.RotationPrefs);
        }
    }

    public void applyPose(RotationPrefs rotationPrefs)
    {
        leanPitchYaw.PitchClamp = false;
        leanPitchYaw.PitchMin = rotationPrefs.PitchMin;
        leanPitchYaw.PitchMax = rotationPrefs.PitchMax;
        leanPitchYaw.YawClamp = false;
        leanPitchYaw.YawMin = rotationPrefs.YawMin;
        leanPitchYaw.YawMax = rotationPrefs.YawMax;

        DOTween.To(assignPitch, leanPitchYaw.Pitch, rotationPrefs.Pitch, CameraAutoRotationSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase).OnComplete(() => { OnPosePitchReach(rotationPrefs.PitchClamp); });
        DOTween.To(assignYaw, leanPitchYaw.Yaw, rotationPrefs.Yaw, CameraAutoRotationSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase).OnComplete(() => { OnPoseYawReach(rotationPrefs.YawClamp); });
    }

    public void OnPosePitchReach(bool pitchClamp)
    {
        leanPitchYaw.PitchClamp = pitchClamp;
    }

    public void OnPoseYawReach(bool yawClamp)
    {
        leanPitchYaw.YawClamp = yawClamp;
    }

    public void OnReachSelected()
    {

    }

    public void resetFocus(Transform nothing)
    {
        SelectableItemPrefs selectedItemPrefs = null;

        if (selectedObject != null)
            selectedItemPrefs = selectedObject.GetComponent<SelectableItemPrefs>();

        if (selectedItemPrefs)
        {
            applyPose(selectedItemPrefs.OriginRotationPrefs);
            selectedItemPrefs.reset();
        }

        selectedObject = null;
        transform.DOMove(Vector3.zero, CameraMovementSpeed, false).SetSpeedBased().SetEase(CameraMovementAnimationEase).OnComplete(OnReachOrigin);

    }

    public void resetPivotPosition()
    {
        if (selectedObject)
        {
            var selectedItemPrefs = selectedObject.GetComponent<SelectableItemPrefs>();
            if (selectedItemPrefs && !selectedItemPrefs.isOriginPrefsInitialized)
            {
                selectedItemPrefs.initializeOriginPose(leanPitchYaw);
            }

            leanSelect.DeselectAll();
        }
        else
        {
            transform.DOMove(Vector3.zero, CameraMovementSpeed, false).SetSpeedBased().SetEase(CameraMovementAnimationEase).OnComplete(OnReachOrigin);
        }
        resetCameraLocalPos();
    }

    public void OnReachOrigin()
    {

    }

    public void resetCameraLocalPos()
    {
        Camera.main.transform.DOLocalMove(Vector3.zero + Vector3.forward * -50, CameraresetPanMovementSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase);
    }

    public void resetPivotRotation()
    {
        DOTween.To(assignPitch, leanPitchYaw.Pitch, 2, CameraAutoRotationSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase);
        DOTween.To(assignYaw, leanPitchYaw.Yaw, 1, CameraAutoRotationSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase);
    }

    public void assignPitch(float originalPitch)
    {
        leanPitchYaw.Pitch = originalPitch;
    }

    public void assignYaw(float originalYaw)
    {
        leanPitchYaw.Yaw = originalYaw;
    }

    public void reset()
    {
        resetPivotPosition();
        resetPivotRotation();
        resetCameraZoom();
    }

    public void resetCameraZoom()
    {
        DOTween.To(assignCameraZoom, leanPinchCamera.Zoom, 6, CameraZoomResetSpeed).SetSpeedBased().SetEase(CameraMovementAnimationEase);
    }

    private void assignCameraZoom(float zoomValue)
    {
        leanPinchCamera.Zoom = zoomValue;
    }
}
