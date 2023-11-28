using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

    public virtual float RadiusCast()
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

    /*public virtual void ChangeScaleVisualEffect(List<GameObject> effectList)
    {
        foreach (GameObject effect in effectList)
        {
            VisualEffect[] visualEffect = effect.GetComponentsInChildren<VisualEffect>();
            foreach (VisualEffect ve in visualEffect)
            {
                ve.pause = true;
            }
        }
    }

    public virtual void ChangeScaleMaterialAndParticle(GameObject effectModel)
    {
        ParticleSystem[] particleSystems = effectModel.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            main.simulationSpeed = scaleGame;
        }

        Renderer[] renderers = effectModel.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            foreach (Material material in materials)
            {
                if (material.name == "BlackHole_Material (Instance)")
                {
                    material.SetFloat("_RotationAmount", scaleGame);
                }
            }
        }
    }*/
}
