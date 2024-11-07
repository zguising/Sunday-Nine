using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanBackground : MonoBehaviour
{

    private Color tan = new Color(0.824f, 0.706f, 0.549f);

    // Start is called before the first frame update
    void Start()
    {
        Renderer cubeRenderer = GetComponent<Renderer>();

        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = tan;
            cubeRenderer.material.renderQueue = 1000;
        }
        else
        {
            Debug.Log("Color error");
        }
    }
}
