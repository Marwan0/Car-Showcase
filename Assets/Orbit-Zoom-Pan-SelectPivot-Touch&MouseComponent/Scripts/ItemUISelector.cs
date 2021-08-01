using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUISelector : MonoBehaviour
{
    [ContextMenu("Select")]
    public void select()
    {
        var leanSelectable = this.GetComponent<LeanSelectable>();
        leanSelectable.Select(null);
        LeanSelect.Instances.First.Value.OnObjectSelection.Invoke(leanSelectable.transform);
    }
}