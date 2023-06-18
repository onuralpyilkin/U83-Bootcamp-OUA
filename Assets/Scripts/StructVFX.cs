using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public struct VFX
{
    public GameObject gameObject;
    public VisualEffect Vfx;

    public VFX(VisualEffect vfx)
    {
        this.gameObject = vfx.gameObject;
        this.Vfx = vfx;
    }

    public void SetFloat(string name, float value)
    {
        Vfx.SetFloat(name, value);
    }

    public void SetVector4(string name, Vector4 value)
    {
        Vfx.SetVector4(name, value);
    }

    public void Play()
    {
        Vfx.Play();
    }

    public void Stop()
    {
        Vfx.Stop();
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