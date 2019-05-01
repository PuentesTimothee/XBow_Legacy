using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearScript : MonoBehaviour
{
    private readonly int _hashAnimation = Shader.PropertyToID("_Animation");
    private readonly int _hashFadeOut = Shader.PropertyToID("_FadeOut");
    private readonly int _hashPoint = Shader.PropertyToID("_Point");

    private Renderer[] _renderers;
    private List<Material> _materials = new List<Material>();

    public float FullAnimationLength = 01f;
    public float MaxValue = 01f;

    public bool AppearOnStart = true;
    
    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in _renderers)
            _materials.AddRange(renderer.materials);
    }

    private void Start()
    {
        if (AppearOnStart) StartAppearing();
    }
    
    public void StartAppearing()
    {
        StartCoroutine(Appear());
    }

//    private void OnDisable()
//    {
//        StartCoroutine(Disapear());
//    }

    public IEnumerator Appear()
    {
        SetIntForAllMaterials(_hashFadeOut, 0);
        float time = 0;
        while (time < FullAnimationLength)
        {
            SetFloatForAllMaterials(_hashAnimation, (time / FullAnimationLength) * MaxValue);
            yield return null;
            time += Time.deltaTime;
        }
        SetFloatForAllMaterials(_hashAnimation, 1);
    }

    public IEnumerator Disapear()
    {
        SetIntForAllMaterials(_hashFadeOut, 1);
        float time = 0;
        while (time < FullAnimationLength)
        {
            SetFloatForAllMaterials(_hashAnimation, (time / FullAnimationLength) * MaxValue);
            yield return null;
            time += Time.deltaTime;
        }
        SetFloatForAllMaterials(_hashAnimation, 1);
    }

    private void SetFloatForAllMaterials(int hash, float val)
    {
        foreach (var material in _materials)
            material.SetFloat(hash, val);
    }

    private void SetIntForAllMaterials(int hash, int val)
    {
        foreach (var material in _materials)
            material.SetInt(hash, val);
    }

    public void SetPoint(Vector3 point)
    {
        foreach (var renderer in _renderers)
        {
            Vector3 val = renderer.transform.rotation * ((point - renderer.bounds.center));
            foreach (var mat in renderer.materials)
                mat.SetVector(_hashPoint, val);
        }
    }
}
