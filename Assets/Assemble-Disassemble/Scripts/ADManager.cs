using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ADManager : MonoBehaviour
{
    public List<ADComponent> anatomyComponents = new List<ADComponent>();
    public float animationSpeed;
    public Ease animationEase;
    public UnityEvent OnFullyDisassembled;
    public UnityEvent OnFullyAssembled;

    [ContextMenu("Fetch Components")]
    public void grabAnimationComponents()
    {
        ADComponent[] anatomyComps = GetComponentsInChildren<ADComponent>();

        anatomyComponents.Clear();

        foreach (var comp in anatomyComps)
        {
            anatomyComponents.Add(comp);
        }

    }

    public void Init()
    {

        foreach (var anatomyComponent in anatomyComponents)
        {
            anatomyComponent.OnAssemblyCompletion.AddListener(declareAssemblingCompletion);
            anatomyComponent.OnDisassemblyCompletion.AddListener(declareDisassemblingCompletion);
        }
    }

    [ContextMenu("disassemble")]
    public void disassemble()
    {
        foreach (var anatomyComponent in anatomyComponents)
        {
            //var movementVector = ( component.deAssembleDirection * component.deAssembleDistanceLimit);
            //component.transform.DOLocalMove (movementVector, animationSpeed, false).SetEase(animationEase).SetSpeedBased();
            anatomyComponent.disassemble(animationSpeed, animationEase);
        }

    }

    [ContextMenu("assemble")]
    public void assemble()
    {
        foreach (var anatomyComponent in anatomyComponents)
        {
            anatomyComponent.assemble(animationSpeed, animationEase);
        }
    }

    public void declareDisassemblingCompletion()
    {
        foreach (var anatomyComponent in anatomyComponents)
        {
            if (anatomyComponent.isAssembleState) return;
        }
        this.OnFullyDisassembled.Invoke();
    }

    public void declareAssemblingCompletion()
    {
        foreach (var anatomyComponent in anatomyComponents)
        {
            if (!anatomyComponent.isAssembleState) return;
        }
        this.OnFullyAssembled.Invoke();
    }
}
