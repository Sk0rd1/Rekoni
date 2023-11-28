using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public static class DamageInfo
{
    public static Color GetColor(TypeDamage typeDamage)
    {
        if (typeDamage == TypeDamage.Fire)
            return new Color(0.7f, 0f, 0f, 0.9f);
        else if (typeDamage == TypeDamage.Thunder)
            return new Color(0.5f, 1.0f, 1.0f, 0.9f);
        else if (typeDamage == TypeDamage.Light)
            return new Color(1f, 0.92f, 0.016f, 0.9f);
        else if (typeDamage == TypeDamage.Cold)
            return new Color(1f, 1f, 1f, 0.9f);
        else if (typeDamage == TypeDamage.Poison)
            return new Color(0.698f, 0.87f, 0.153f, 0.9f);
        else if (typeDamage == TypeDamage.Necrotic)
            return new Color(0.118f, 0.510f, 0.298f, 0.9f);
        else
            return new Color(0f, 0f, 0f, 0.9f);
    }

}

public enum TypeDamage
{
    Force, // чорний
    Fire, // червоний
    Thunder, // блакитний
    Light, // жовтий
    Cold, // білий
    Poison, // зелено-жовтий
    Necrotic // темно-зелений
}
