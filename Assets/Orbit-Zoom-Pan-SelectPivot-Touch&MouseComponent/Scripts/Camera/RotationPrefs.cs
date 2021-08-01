using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotationPrefs
{
    [Space(10)]
    [SerializeField] float pitch;
    [SerializeField] float yaw;

    [Space(10)]
    [SerializeField] bool pitchClamp;
    [SerializeField] float pitchMin;
    [SerializeField] float pitchMax;

    [Space(10)]
    [SerializeField] bool yawClamp;
    [SerializeField] float yawMin;
    [SerializeField] float yawMax;

    public float Pitch { get => pitch; set => pitch = value; }
    public float Yaw { get => yaw; set => yaw = value; }
    public bool PitchClamp { get => pitchClamp; set => pitchClamp = value; }
    public float PitchMin { get => pitchMin; set => pitchMin = value; }
    public float PitchMax { get => pitchMax; set => pitchMax = value; }
    public bool YawClamp { get => yawClamp; set => yawClamp = value; }
    public float YawMin { get => yawMin; set => yawMin = value; }
    public float YawMax { get => yawMax; set => yawMax = value; }
}
