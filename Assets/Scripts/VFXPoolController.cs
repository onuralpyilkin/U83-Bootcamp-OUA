using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPoolController : MonoBehaviour
{

    //public List<VisualEffect> vfxList = new List<VisualEffect>();
    private Queue<VFX> vfxQueue = new Queue<VFX>();
    [HideInInspector]
    public bool SelfRelease = false;
    [HideInInspector]
    public float ReleaseDelay = 3f;

    void Awake()
    {
        List<VisualEffect> vfxList = new List<VisualEffect>(GetComponentsInChildren<VisualEffect>());
        foreach (VisualEffect vfx in vfxList)
        {
            vfxQueue.Enqueue(new VFX(vfx));
            vfx.transform.SetParent(null);
        }
    }

    public VFX Get()
    {
        VFX vfx = vfxQueue.Dequeue();
        vfx.SetActive(true);
        if (SelfRelease)
        {
            StartCoroutine(ReleaseCoroutine(vfx, ReleaseDelay));
        }
        return vfx;
    }

    public void Release(VFX vfx)
    {
        vfxQueue.Enqueue(vfx);
        vfx.SetActive(false);
    }

    public int GetCount()
    {
        return vfxQueue.Count;
    }

    IEnumerator ReleaseCoroutine(VFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        Release(vfx);
    }
}
