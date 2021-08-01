using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component will constrain the current transform.localScale to the specified range.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanConstrainScale")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Constrain Scale")]
	public class LeanConstrainScale : MonoBehaviour
	{
		/// <summary>Should each axis be checked separately? If not, the relative x/y/z values will be maintained.</summary>
		//[Tooltip("Should each axis be checked separately? If not, the relative x/y/z values will be maintained.")]
		//public bool Independent;

		/// <summary>Should there be a minimum transform.localScale?</summary>
		[Tooltip("Should there be a minimum transform.localScale?")]
		public bool Minimum;

		/// <summary>The minimum transform.localScale value.</summary>
		[Tooltip("The minimum transform.localScale value.")]
		public Vector3 MinimumScale = Vector3.one;

		/// <summary>Should there be a maximum transform.localScale?</summary>
		[Tooltip("Should there be a maximum transform.localScale?")]
		public bool Maximum;

		/// <summary>The maximum transform.localScale value.</summary>
		[Tooltip("The maximum transform.localScale value.")]
		public Vector3 MaximumScale = Vector3.one;	
		
		
		/// <summary>Should there be a minimum transform.rotation?</summary>
		[Tooltip("Should there be a minimum transform.rotation?")]
		public bool isMinimumRotation;

		/// <summary>The minimum transform.localScale value.</summary>
		[Tooltip("The minimum transform.localScale value.")]
		public Vector3 MinimumRotation = Vector3.zero;

		/// <summary>Should there be a maximum transform.rotation?</summary>
		[Tooltip("Should there be a maximum transform.rotation?")]
		public bool isMaximumRotation;

		/// <summary>The maximum transform.rotatoon value.</summary>
		[Tooltip("The maximum transform.rotation value.")]
		public Vector3 MaximumRotation = Vector3.zero;



        private void Update()
        {

		}

		protected virtual void LateUpdate()
		{

			Vector3 rot = transform.localRotation.eulerAngles;


			var scale = transform.localScale;
			//if (Independent == true)
			{
				if (Minimum == true)
				{
					scale.x = Mathf.Max(scale.x, MinimumScale.x);
					scale.y = Mathf.Max(scale.y, MinimumScale.y);
					scale.z = Mathf.Max(scale.z, MinimumScale.z);
				}

				if (Maximum == true)
				{
					scale.x = Mathf.Min(scale.x, MaximumScale.x);
					scale.y = Mathf.Min(scale.y, MaximumScale.y);
					scale.z = Mathf.Min(scale.z, MaximumScale.z);
				}

				if (isMinimumRotation == true)
				{
					rot.x = Mathf.Max(rot.x, MinimumRotation.x);
					rot.y = Mathf.Max(rot.y, MinimumRotation.y);
					rot.z = Mathf.Max(rot.z, MinimumRotation.z);
				}

				if (isMaximumRotation == true)
				{
					rot.x = Mathf.Min(rot.x, MaximumRotation.x);
					rot.y = Mathf.Min(rot.y, MaximumRotation.y);
					rot.z = Mathf.Min(rot.z, MaximumRotation.z);
				}

			}
			/*
			else
			{
				if (Minimum == true)
				{
					var best  = 1.0f;
					var found = false;

					if (scale.x < MinimumScale.x)
					{
						var current = scale.x / MinimumScale.x;
						found = true;
					}

					if (found == true)
					{
						scale *= best;
					}
				}
			}
			*/

			transform.localScale = scale;
			transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

		}
	}
}