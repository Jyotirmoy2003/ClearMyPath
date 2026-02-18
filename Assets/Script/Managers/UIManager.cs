using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Foldout("Ammo")][SerializeField] RectTransform amunationPanel;
    [Foldout("Ammo")][SerializeField] TMP_Text numberOfBombLeft;

    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject masterPlayerControlles;
    [SerializeField] GameObject clientPlayerControlles;

    [SerializeField] CanvasGroup blackScreenFade;
    [SerializeField] float fadeDuration = 0.3f;

    [Foldout("Input")] [SerializeField] GameObject joystickInputforClientAndroid;
    [Foldout("Input")] [SerializeField] GameObject buttonInputForMasterAndroid;
    [Foldout("Input")] [SerializeField] Slider sensetivitySlider;
    [Foldout("Input")] [SerializeField] GameObject cursorImage;
    [SerializeField] GameObject devtalkPanel;
    [SerializeField] GameEvent Event_OnSensetivityValueChnaged;
    
    [Header("Bomb UI")]
    [SerializeField] private RectTransform bombPanel;
    [SerializeField] private TMP_Text bombText;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float visibleDuration = 1.5f;

    private Tween currentTween;
    private Vector2 hiddenPosition;
    private Vector2 shownPosition;



    void Start()
    {
        blackScreenFade.gameObject.SetActive(true);
        SetPauseMenuStatus(false);
        emojiButtonContainer.SetActive(false);

        hiddenPosition = new Vector2(0, -(Screen.height+150));
        shownPosition = new Vector2(0, 150); // Slightly above center

        bombPanel.anchoredPosition = hiddenPosition;

    }

    public void UpdateAmmunationInfo(int amount)
    {
        numberOfBombLeft.text = amount.ToString();
    }

    public void NoAmmoSignel()
    {
        ShakeUI(amunationPanel);
    }

    public void ShakeUI(RectTransform target, float strength = 20f, float duration = 0.3f)
    {
        target.DOKill(); // stop any previous tweens

        target.DOPunchAnchorPos(
            new Vector2(strength, 0f),  // shake only X axis
            duration,
            vibrato: 10,
            elasticity: 1f
        );
    }

    internal void ShowBombAddedUI(int bombAmount)
    {
        // Stop any ongoing animation
        currentTween?.Kill();

        bombText.text = $"+{bombAmount}";

        bombPanel.gameObject.SetActive(true);
        bombPanel.anchoredPosition = hiddenPosition;

        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(bombPanel.DOAnchorPos(shownPosition, animationDuration)
            .SetEase(Ease.OutBack));

        seq.AppendInterval(visibleDuration);

        seq.Append(bombPanel.DOAnchorPos(hiddenPosition, animationDuration)
            .SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            bombPanel.gameObject.SetActive(false);
        });

        currentTween = seq;
    }

    public void SetPauseMenuStatus(bool show)
    {
        pauseMenuPanel.SetActive(show);
    }

    #region Settings and Controls
    public void SetControllstatus(bool isMaster)
    {
        masterPlayerControlles.SetActive(isMaster);
        clientPlayerControlles.SetActive(!isMaster);
    }

   

    public void ConfigureControlls(bool isMaster)
    {

        cursorImage.SetActive(isMaster);
        joystickInputforClientAndroid.SetActive(false);
        buttonInputForMasterAndroid.SetActive(false);
       if(Application.isMobilePlatform)
        {
            
            if(isMaster)
            {
                buttonInputForMasterAndroid.SetActive(true);
                joystickInputforClientAndroid.SetActive(false);
                
            }
            else
            {
                joystickInputforClientAndroid.SetActive(true);
                buttonInputForMasterAndroid.SetActive(false);
            }

        }
        

        
    }

    public void OnTouchSenstivityValueChange(float value)
    {
        Event_OnSensetivityValueChnaged.Raise(this,value);
    }

    public void SetSliderData(float maxValue,float currentValue)
    {
        sensetivitySlider.maxValue = maxValue;
        sensetivitySlider.value = currentValue;
    }

    #endregion
    public void ActivateDevTalk()
    {
        devtalkPanel.SetActive(true);
    }

    #region EMOJI
    [SerializeField] GameObject emojiButtonContainer;
    public void OnMessgButtonPressed()
    {
        emojiButtonContainer.SetActive(!emojiButtonContainer.activeSelf);
    }

    

    #endregion
    #region BlackScreen
     public void BlackScreenFadeIn()
    {
        blackScreenFade.gameObject.SetActive(true);
        blackScreenFade.DOFade(1,fadeDuration).OnComplete(() =>
            {
                blackScreenFade.blocksRaycasts = true;
            });
    }

    public void BlackScreenFadeOut()
    {
        blackScreenFade.DOFade(0f, fadeDuration)
            .OnComplete(() =>
            {
                blackScreenFade.gameObject.SetActive(false);;
            });
    }
    #endregion

}
