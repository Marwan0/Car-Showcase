using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ADComponent : MonoBehaviour
{
    public Vector3 disassembleDirection;
    [Space(10)]
    public float disassembleDistanceLimit;
    [Space(10)]
    private Vector3 initialPosition;
    [Space(10)]
    public List<ADComponent> precedentComponents;

    public UnityEvent OnAssemblyCompletion;
    public UnityEvent OnDisassemblyCompletion;

    public PrecedentUnityEvent waitingList;
    public bool isAssembleState;

    [HideInInspector]
    public float animationSpeed;
    [HideInInspector]
    public Ease animationEase;

    private void Start()
    {
        if (waitingList == null)
            waitingList = new PrecedentUnityEvent();

        initialPosition = transform.localPosition;
        FindObjectOfType<ADComponent>();
    }

    public void disassemble(float animationSpeed, Ease animationEase)
    {
        this.animationEase = animationEase;
        this.animationSpeed = animationSpeed;
        if (precedentComponents.Count > 0)
        {
            foreach (var precedentComponent in precedentComponents)
            {
                precedentComponent.waitingList.AddListener(attemptToMove);
            }
        }
        else
        {
            var movementVector = initialPosition + (disassembleDirection * disassembleDistanceLimit);
            transform.DOLocalMove(movementVector, animationSpeed).SetEase(animationEase).SetSpeedBased().OnComplete(() =>
            {
                waitingList.Invoke(this);
                this.isAssembleState = false;
                OnAssemblyCompletion.Invoke();
            });
        }

    }

    public void attemptToMove(ADComponent precedentComponent)
    {
        precedentComponents.Remove(precedentComponent);

        if (precedentComponents.Count == 0)
        {
            var movementVector = initialPosition + (disassembleDirection * disassembleDistanceLimit);
            transform.DOLocalMove(movementVector, animationSpeed).SetEase(animationEase).SetSpeedBased().OnComplete(() =>
            {
                waitingList.Invoke(this);
                this.isAssembleState = false;
                OnAssemblyCompletion.Invoke();
            });
        }

    }

    public void assemble(float animationSpeed, Ease animationEase)
    {

        transform.DOLocalMove(initialPosition, animationSpeed).SetEase(animationEase).SetSpeedBased().OnComplete(() =>
        {
            isAssembleState = true;
            OnDisassemblyCompletion.Invoke();
        });
    }
}

[System.Serializable]
public class PrecedentUnityEvent : UnityEvent<ADComponent> { }