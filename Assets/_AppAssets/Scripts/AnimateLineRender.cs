using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class AnimateLineRender : MonoBehaviour
{
    LineRenderer LR;
    [TextArea]
    public string InfoText;
    public TextMeshPro txtComponent;
    Vector3[] LineRenderPostions;

    public UnityEvent AnimationComplete;

    // Start is called before the first frame update
    void Start()
    {
        //initialzing the component
        LR = GetComponent<LineRenderer>();
        txtComponent.transform.localScale = Vector3.zero;
        LineRenderPostions = new Vector3[LR.positionCount];
        LR.GetPositions(LineRenderPostions);
        LR.SetPositions(new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero });

        //the magic happen from here
        _AnimateLineRender(1f);

    }

    public void _AnimateLineRender(float duration)
    {
        txtComponent.text = InfoText;
        Vector3 startPos = new Vector3(1f, 0.6f, 0);
        Vector3 EndPos = new Vector3(2.1f, 0.8f, 0);


        Vector3 customV = Vector3.zero;
        DOTween.To(() => new Vector3(0, 0, 0), x => customV = x, LineRenderPostions[1], duration).OnUpdate(() =>
          {
              LR.SetPosition(1, customV);
              LR.SetPosition(2, customV);
          });
        Tween tt = null;
        tt = DOTween.To(() => new Vector3(0, 0, 0), x => customV = x, LineRenderPostions[2], duration).SetDelay(1f).From(LineRenderPostions[1]).OnUpdate(() =>
        {

            LR.SetPosition(2, customV);
            if (tt.IsComplete())
            {
                txtComponent.transform.DOLocalMove(EndPos, 1).From(startPos);
                txtComponent.transform.DOScale(Vector3.one, 1).From(Vector3.zero).OnComplete(() => AnimationComplete.Invoke());
            }
        });

    }

}
