using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Spell_XXX : MonoBehaviour
{
    public virtual bool MomentaryCast()
    {
        return true;
    }

    public virtual void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    public virtual void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    public virtual void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        
    }

    public virtual bool IsSpellReady()
    {
        //return isSpellReady;
        return false;
    }
}
