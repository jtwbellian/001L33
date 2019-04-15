using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLast : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private SkinnedMeshRenderer[] skinRenderers;

    public void SetOpaque(Material material)
    {
        //material.SetOverrideTag("RenderType", "");
        //material.SetOverrideTag("Queue", "Geometry");
      // material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
       // material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHAPREMULTIPLY_OFF");
        material.renderQueue = 3000;
    }


    // Start is called before the first frame update
    void Start()
    {
        renderLast();

        Debug.Log("Setting " + (renderers.Length + skinRenderers.Length) + "materials");
    }

    [ContextMenu("redraw")]
    public void renderLast()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        int i = 0;
        int n = renderers.Length + skinRenderers.Length;

        foreach (MeshRenderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                SetOpaque(mat);
                Debug.Log("updating renderer " + i + " / " + n);
                i++;
            }
        }

        foreach (SkinnedMeshRenderer sr in skinRenderers)
        {
            foreach (Material mat in sr.materials)
            {
                SetOpaque(mat);
                Debug.Log("updating renderer " + i + " / " + n);
                i++;
            }
        }
    }

}