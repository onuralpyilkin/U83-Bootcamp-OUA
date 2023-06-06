using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public struct VFX
{
    public GameObject gameObject;
    public VisualEffect vfx;

    public VFX(VisualEffect vfx)
    {
        this.gameObject = vfx.gameObject;
        this.vfx = vfx;
    }

    public void SetFloat(string name, float value)
    {
        vfx.SetFloat(name, value);
    }

    public void SetVector4(string name, Vector4 value)
    {
        vfx.SetVector4(name, value);
    }

    public void Play()
    {
        vfx.Play();
    }

    public void Stop()
    {
        vfx.Stop();
    }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}