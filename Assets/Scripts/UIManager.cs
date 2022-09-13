using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI _pointsUGUI;

    [Header("Interaction UGUI")]
    [SerializeField] private TextMeshProUGUI _interactTMP;
    [SerializeField] private Transform _reticleContainer;
    [SerializeField] private Image _interactionProgressImage;
    [SerializeField] private Vector3 _maxReticleScale = Vector3.one;
    [SerializeField] private float _reticleScaleResetSpeed = 1f;
    [SerializeField] private float _reticleScaleSpeed = 0.1f;
    private bool _interactionProgressStarted = false;

    public void SetInteractionText(string text)
    {
        _interactTMP.text = text;
    }

    public void UpdateInteractionProgressChange(float progress)
    {
        _interactionProgressImage.fillAmount = progress;

        if (!_interactionProgressStarted)
        {
            _interactionProgressStarted = true;
            StartCoroutine(ScaleReticle());
        }

        if (progress == 0)
        {
            ScaleObject(_reticleContainer, _reticleContainer.localScale, Vector3.one, _reticleScaleResetSpeed);
        }
    }

    private IEnumerator ScaleReticle()
    {
        while (_interactionProgressImage.fillAmount > 0f)
        {
            ScaleObject(_reticleContainer, _reticleContainer.transform.localScale, _maxReticleScale, _reticleScaleSpeed);
            yield return null;
        }

        ScaleObject(_reticleContainer, _reticleContainer.transform.localScale, Vector3.one, _reticleScaleResetSpeed);
        _interactionProgressStarted = false;
    }

    private void ScaleObject(Transform transformToScale, Vector3 start, Vector3 end, float speed)
    {
        transformToScale.localScale = Vector3.Lerp(start, end, speed);
    }
}
