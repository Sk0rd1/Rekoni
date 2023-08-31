using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUniversal : MonoBehaviour
{
    public virtual bool IsMomemtaryCast()
    {
        return false;
    }

    public virtual bool IsSpellReady()
    {
        return false;
    }

    public virtual float TimeReload()
    {
        return 0f;
    }

    public virtual void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    public virtual void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        
    }

    public virtual void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }
}
