using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPoolController : MonoBehaviour
{

    //public List<VisualEffect> vfxList = new List<VisualEffect>();
    public Queue<VFX> vfxQueue = new Queue<VFX>();

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
}
