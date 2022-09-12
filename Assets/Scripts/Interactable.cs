using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("The string that will be displayed while the player looks at the object.")]
    [SerializeField] protected string _hoverText = "";
    [Tooltip("If true, the player will have to hold the interaction for a duration (defined by _holdTimeInSeconds); otherwise, the player will have to click.")]
    [SerializeField] private bool _shouldHold;
    [Tooltip("If _shouldHold is true, this is the duration that it will take to complete the interaction.")]
    [SerializeField] private float _holdTimeInSeconds;
    protected float _currentProgress;

    [Header("Audio")]
    [Tooltip("The audio source of the interactable.")]
    [SerializeField] protected AudioSource _audioSource;
    [Tooltip("The sound that plays when the interactable is broken and not interacted with.")]
    [SerializeField] protected AudioClip _brokenSound;
    [Tooltip("The sound that plays when the interactable is used (or when a hold is complete).")]
    [SerializeField] protected AudioClip[] _useSound;
    [Tooltip("The sound that starts playing when the progress of a hold interaction beings. It will stop if the interaction is cancelled.")]
    [SerializeField] protected AudioClip[] _progressSound;
    [Tooltip("The sound volume to play this interactable's sounds at.")]
    [SerializeField][Range(0f, 1f)] protected float _volumeScale = 1f;
    [Tooltip("Whether to randomize the pitch of all of this interactable's sounds.")]
    [SerializeField] protected bool _randomizePitches = false;
    [Tooltip("The minimum pitch possible. Used if randomize pitches is true.")]
    [SerializeField][Range(-3, 3f)] protected float _minPitch = 0f;
    [Tooltip("The maximum pitch possible. Used if randomize pitches is true.")]
    [SerializeField][Range(-3, 3f)] protected float _maxPitch = 3f;

    ///<summary>Handles what the interactable does when it is used.</summary>
    ///<remarks>If _shouldHold is true, this is called once the hold is completed; otherwise, it is called when interacted with.</remarks>
    public abstract void Use(TopDownController user);

    ///<summary>Handles what should be displayed next to the player's cursor when the player is looking at the interactable.</summary>
    ///<remarks>This is not called while the player is interacting with the object.</remarks>
    public virtual void ShowTooltips()
    {
        if (IsInteractable())
        {
            string hold = "";
            if (_shouldHold)
            {
                hold = "hold ";
            }

            //UIManager.Instance.RevealInteractionText($"{hold}[{_player.GetPlayerControls().Player.Interact.bindings[0].ToDisplayString()}] {_hoverText}");
        }
    }

    ///<returns>True if the interactable can be used; otherwise false. This is specific to each interactable.</returns>
    protected abstract bool IsInteractable();

    ///<summary>Handles a hold interaction's progress. This also triggers the interaction progress to display.</summary>
    public void AddProgress(TopDownController user)
    {
        if (IsInteractable())
        {
            if (_currentProgress == 0)
            {
                //AudioManager.PlaySFXRandom(_audioSource, _progressSound, _randomizePitches, _minPitch, _maxPitch, _volumeScale);
            }

            _currentProgress += Time.deltaTime;
            float _currentProgressClamped = Mathf.InverseLerp(0f, _holdTimeInSeconds, _currentProgress);

            //Events.OnHoldProgress?.Invoke(_currentProgressClamped);

            if (_currentProgress >= _holdTimeInSeconds)
            {
                Use(user);
                if (_useSound.Length > 0)
                {
                    //AudioManager.Instance.PlayEffect(_useSound[UnityEngine.Random.Range(0, _useSound.Length)]);
                }

                //UIManager.Instance.HideInteractionText();

                _currentProgressClamped = 0;
                //Events.OnHoldProgress?.Invoke(_currentProgressClamped);
            }
        }
    }

    ///<summary>Resets a hold interaction to default values.</summary>
    public virtual void Reset()
    {
        if (_currentProgress > 0)
        {
            _currentProgress = 0;
            //Events.OnHoldProgress?.Invoke(_currentProgress);

            if (( _progressSound.Length > 0 ) && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }

    ///<returns>True if this is a hold interaction; otherwise, false.</returns>
    public bool ShouldHold()
    {
        return _shouldHold;
    }
}
