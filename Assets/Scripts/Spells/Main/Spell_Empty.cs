using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Empty : SpellUniversal
{
    public override bool IsMomemtaryCast()
    {
        return true;
    }

    public override bool IsSpellReady()
    {
        return false;
    }

    public override float TimeReload()
    {
        return 0f;
    }
}
