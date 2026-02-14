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
    [SerializeField] GameEvent Event_OnSensetivityValueChnaged;



    void Start()
    {
        blackScreenFade.gameObject.SetActive(true);
        SetPauseMenuStatus(false);

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

    public void SetPauseMenuStatus(bool show)
    {
        pauseMenuPanel.SetActive(show);
    }

    public void SetControllstatus(bool isMaster)
    {
        masterPlayerControlles.SetActive(isMaster);
        clientPlayerControlles.SetActive(!isMaster);
    }

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

    public void ConfigureControlls(bool isMaster)
    {

        cursorImage.SetActive(isMaster);
        #if (UNITY_IOS || UNITY_ANDROID)
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

        #endif

        
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


}
