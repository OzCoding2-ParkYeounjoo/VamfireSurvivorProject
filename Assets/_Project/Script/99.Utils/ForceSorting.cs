using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ForceSorting : MonoBehaviour
{
    private void Awake()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;

        GraphicsSettings.transparencySortAxis = new Vector3(0, 1, 0);
    }
}
