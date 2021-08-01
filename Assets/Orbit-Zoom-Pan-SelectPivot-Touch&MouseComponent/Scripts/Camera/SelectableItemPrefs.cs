using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectableItemPrefs : MonoBehaviour
{

    [SerializeField] RotationPrefs rotationPrefs;
    RotationPrefs originRotationPrefs;
    
    public bool isOriginPrefsInitialized;
    public RotationPrefs OriginRotationPrefs { get => originRotationPrefs; set => originRotationPrefs = value; }
    public RotationPrefs RotationPrefs { get => rotationPrefs; set => rotationPrefs = value; }

    public void initializeOriginPose(float pitch, float yaw, bool isPitchClamped = false, float pitchMin = 0, float pitchMax = 0, bool isYawClamped = false, float yawMin = 0, float yawMax = 0)
    {
        isOriginPrefsInitialized = true;

        OriginRotationPrefs.Pitch = pitch;
        OriginRotationPrefs.Yaw = yaw;

        if (isPitchClamped)
        {
            OriginRotationPrefs.PitchClamp = isPitchClamped;
            OriginRotationPrefs.PitchMin = pitchMin;
            OriginRotationPrefs.PitchMax = pitchMax;
        }

        if (isYawClamped)
        {
            OriginRotationPrefs.YawClamp = isYawClamped;
            OriginRotationPrefs.YawMin = yawMin;
            OriginRotationPrefs.YawMax = yawMax;
        }
    }

    public void initializeOriginPose(RotationPrefs rotPrefs)
    {
        isOriginPrefsInitialized = true;
        originRotationPrefs = new RotationPrefs();
        OriginRotationPrefs.Pitch = rotPrefs.Pitch;
        OriginRotationPrefs.Yaw = rotPrefs.Yaw;

        OriginRotationPrefs.PitchClamp = rotPrefs.PitchClamp;
        OriginRotationPrefs.PitchMin = rotPrefs.PitchMin;
        OriginRotationPrefs.PitchMax = rotPrefs.PitchMax;

        OriginRotationPrefs.YawClamp = rotPrefs.YawClamp;
        OriginRotationPrefs.YawMin = rotPrefs.YawMin;
        OriginRotationPrefs.YawMax = rotPrefs.YawMax;
    }    
    public void initializeOriginPose(LeanPitchYaw leanPitchYaw)
    {
        isOriginPrefsInitialized = true;
        originRotationPrefs = new RotationPrefs();
        OriginRotationPrefs.Pitch = leanPitchYaw.Pitch;
        OriginRotationPrefs.Yaw = leanPitchYaw.Yaw;

        OriginRotationPrefs.PitchClamp = leanPitchYaw.PitchClamp;
        OriginRotationPrefs.PitchMin = leanPitchYaw.PitchMin;
        OriginRotationPrefs.PitchMax = leanPitchYaw.PitchMax;

        OriginRotationPrefs.YawClamp = leanPitchYaw.YawClamp;
        OriginRotationPrefs.YawMin = leanPitchYaw.YawMin;
        OriginRotationPrefs.YawMax = leanPitchYaw.YawMax;
    }


    public void reset()
    {
        isOriginPrefsInitialized = false;
    }
}

