using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject mainMenuContent;
    public GameObject aboutContent;

    [Header("End Screen")]
    public Image endScreenBG;
    public TMP_Text title;
    public Button btn_restart;
    public Button btn_menu;

    private TMP_Text txt_restart;
    private TMP_Text txt_menu;

    private Image img_restart;
    private Image img_menu;

    [Header("Pause Screen")]
    public GameObject pauseScreenParent;

    [Header("Scene Loader")]
    public SceneLoader sceneLoader;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) DemoInitialize();
    }

    public void DemoInitialize()
    {
        txt_restart = btn_restart.transform.GetChild(0).GetComponent<TMP_Text>();
        txt_menu = btn_menu.transform.GetChild(0).GetComponent<TMP_Text>();

        img_restart = btn_restart.GetComponent<Image>();
        img_menu = btn_menu.GetComponent<Image>();
    }

    #region Main Menu Callbacks

    public void ToggleMainMenuAndAbout(bool b)
    {
        mainMenuContent.SetActive(!b);
        aboutContent.SetActive(b);
    }

    public void QuitGame() => Application.Quit();

    #endregion

    public void LoadGameScenes(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        //sceneLoader.LoadSceneWithDelay(index);
    }

    public void Button_Resume() => GameManager.Main.GamePause(false);

    public void Button_Restart()
    {
        //StartCoroutine(RestartBGFadeInOut());
        endScreenBG.color = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, 0f);
        title.color = new(title.color.r, title.color.g, title.color.b, 0f);
        btn_restart.interactable = false;
        btn_menu.interactable = false;
        txt_restart.color = new(txt_restart.color.r, txt_restart.color.g, txt_restart.color.b, 0f);
        txt_menu.color = new(txt_menu.color.r, txt_menu.color.g, txt_menu.color.b, 0f);
        img_restart.color = new(img_restart.color.r, img_restart.color.g, img_restart.color.b, 0f);
        img_menu.color = new(img_menu.color.r, img_menu.color.g, img_menu.color.b, 0f);
        GameManager.Main.RestartLevel();
    }

    public IEnumerator FadeInEndScreen(float dur)
    {
        yield return new WaitForSeconds(2f);

        float d = 0f;

        // title
        Color curTitle = new(title.color.r, title.color.g, title.color.b, 0f);
        Color tarTitle = new(title.color.r, title.color.g, title.color.b, 1f);

        // endScreenBG
        Color curBG = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, 0f);
        Color tarBG = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, 0.89f);

        // btn_restart components
        Color curRes = new(img_restart.color.r, img_restart.color.g, img_restart.color.b, 0f);
        Color tarRes = new(img_restart.color.r, img_restart.color.g, img_restart.color.b, 1f);
        Color curResText = new(txt_restart.color.r, txt_restart.color.g, txt_restart.color.b, 0f);
        Color tarResText = new(txt_restart.color.r, txt_restart.color.g, txt_restart.color.b, 1f);

        // btn_menu components
        Color curMenu = new(img_menu.color.r, img_menu.color.g, img_menu.color.b, 0f);
        Color tarMenu = new(img_menu.color.r, img_menu.color.g, img_menu.color.b, 1f);
        Color curMenuText = new(txt_menu.color.r, txt_menu.color.g, txt_menu.color.b, 0f);
        Color tarMenuText = new(txt_menu.color.r, txt_menu.color.g, txt_menu.color.b, 1f);

        while (d < dur)
        {
            d += Time.deltaTime;
            float t = Mathf.Clamp01(d / dur);
            float t2 = Mathf.SmoothStep(0f, 1f, t);

            title.color = Color.Lerp(curTitle, tarTitle, t2);
            endScreenBG.color = Color.Lerp(curBG, tarBG, t2);
            img_restart.color = Color.Lerp(curRes, tarRes, t2);
            txt_restart.color = Color.Lerp(curResText, tarResText, t2);
            img_menu.color = Color.Lerp(curMenu, tarMenu, t2);
            txt_menu.color = Color.Lerp(curMenuText, tarMenuText, t2);

            yield return null;
        }

        title.color = tarTitle;
        endScreenBG.color = tarBG;
        img_restart.color = tarRes;
        txt_restart.color = tarResText;
        img_menu.color = tarMenu;
        txt_menu.color = tarMenuText;
        btn_restart.interactable = true;
        btn_menu.interactable = true;
    }

    //private IEnumerator RestartBGFadeInOut()
    //{
    //    float d = 0f;
    //    bool b = true;
    //    Color curBG = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, endScreenBG.color.a);
    //    Color blackBG = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, 1f);
    //    Color transBG = new(endScreenBG.color.r, endScreenBG.color.g, endScreenBG.color.b, 0f);

    //    while (d < 2f)
    //    {
    //        d += Time.deltaTime;
    //        float t = Mathf.Clamp01(d / 2f);
    //        float t2 = Mathf.SmoothStep(0f, 1f, t);

    //        endScreenBG.color = d < 1f ? Color.Lerp(curBG, blackBG, t2) : Color.Lerp(blackBG, transBG, t2);

    //        if (b && d > 1f)
    //        {
    //            GameManager.Main.RestartLevel();
    //            b = false;
    //        }

    //        yield return null;
    //    }

    //    endScreenBG.color = transBG;
    //}
}
