using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    
    private Animator _animator;
    private SpriteRenderer[] _targetSpriteRenderers;
    private Color[] _originalColors;
    private Coroutine _flashCoroutine;
    private Material[] _originalSharedMaterials;
    
    [Header("Health Settings")]
    [SerializeField] private int maximumHitPoints = 10;
    [SerializeField] private int currentHitPoints;
    [SerializeField] private string hitTriggerName = "Hit";
    
    [Header("Flash Settings")]
    [SerializeField] private Material flashMaterial;  
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField, Min(0f)] private float singleFlashDurationSeconds = 0.08f;
    [SerializeField, Min(1)] private int numberOfFlashes = 1;
    
    
    public System.Action OnDeath;
    private void Awake()
    {
        currentHitPoints = maximumHitPoints;
        _animator = GetComponent<Animator>();
        _targetSpriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        _originalColors = new Color[_targetSpriteRenderers.Length];
        for (var i = 0; i < _targetSpriteRenderers.Length; i++)
        {
            _originalColors[i] = _targetSpriteRenderers[i].color;
        }
        _originalSharedMaterials = new Material[_targetSpriteRenderers.Length];
        for (var i = 0; i < _targetSpriteRenderers.Length; i++)
        {
            var sr = _targetSpriteRenderers[i];
            _originalSharedMaterials[i] = sr != null ? sr.sharedMaterial : null;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHitPoints -= Mathf.Max(0, damageAmount);

        if (currentHitPoints <= 0)
        {
            currentHitPoints = 0;
            HandleDeath();
        }
        else
        {
            if (_animator == null || string.IsNullOrEmpty(hitTriggerName)) return;

            _animator.ResetTrigger(hitTriggerName);
            _animator.SetTrigger(hitTriggerName);

            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                RestoreOriginalColors();
            }
            _flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }
    
    private IEnumerator FlashRoutine()
    {
        for (var cycle = 0; cycle < numberOfFlashes; cycle++)
        {
            SetAllSpriteColors(flashColor);
            SetAllFlashMaterials(flashMaterial);
            yield return new WaitForSeconds(singleFlashDurationSeconds);
            
            RestoreOriginalColors();
            RestoreOriginalMaterials();
            yield return new WaitForSeconds(singleFlashDurationSeconds * 0.8f); // small off gap (tweak)
        }

        _flashCoroutine = null;
    }

    private void SetAllFlashMaterials(Material material)
    {
        foreach (var sr in _targetSpriteRenderers)
        {
            if (sr is null || material is null) continue;
            sr.material = material;
        }
    }
    
    private void RestoreOriginalMaterials()
    {
        for (var j = 0; j < _targetSpriteRenderers.Length; j++)
        {
            var sr = _targetSpriteRenderers[j];
            if (sr is null) continue;
            sr.sharedMaterial = _originalSharedMaterials[j];
        }
    }


    private void SetAllSpriteColors(Color color)
    {
        foreach (var targetSpriteRenderer in _targetSpriteRenderers)
        {
            if (targetSpriteRenderer is not null)
                targetSpriteRenderer.color = color;
        }
    }

    private void RestoreOriginalColors()
    {
        for (var i = 0; i < _targetSpriteRenderers.Length; i++)
        {
            if (_targetSpriteRenderers[i] is not null)
                _targetSpriteRenderers[i].color = _originalColors[i];
        }
    }
    private void HandleDeath()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}