using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    int correctCount = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            AnswerManager answerManager;
            //若是在场景2才能找到answerManager的引用
            answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();

            //GameObject.Find("PanelUI").transform.FindChild("TextLevel").GetComponent<Text>().text = "第" + level + "关";

            correctCount = answerManager.correctCount;
            PassLevel();
        }
    }

    void PassLevel()
    {
        if (correctCount == 5)
        {
            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("PassLevel").gameObject.SetActive(true);

            correctCount = -1;//防止意外
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene2");
    }

    //    public static GameManager instance = null;
    //    public AudioClip bgm1;
    //    public AudioClip bgm2;
    //    public static AudioSource audioSource;
    //    public static Slider audioSlider;
    //    public static int level = 0;  //关卡
    //    int correctCount = 0;

    //    void Awake () {
    //        if (instance == null)
    //            instance = this;
    //        else if (instance != null)
    //        {
    //            Destroy(gameObject);
    //        }

    //        DontDestroyOnLoad(gameObject);
    //	}

    //    void Start()
    //    {
    //        if (SceneManager.GetActiveScene().name == "Scene1")
    //        {
    //            audioSource = transform.GetComponent<AudioSource>();
    //            audioSlider = GameObject.Find("Canvas").transform.FindChild("setPanel").FindChild("Audio").FindChild("AudioSlider").GetComponent<Slider>();
    //            //播放音乐
    //            audioSource.clip = bgm1;
    //            audioSource.Play();
    //        }
    //    }

    //	void Update () {

    //        if (SceneManager.GetActiveScene().name == "Scene2")
    //        {
    //            AnswerManager answerManager;
    //            QuestionManager questionManager;
    //            //若是在场景2才能找到answerManager和questionManager的引用
    //            answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();
    //            questionManager = GameObject.Find("questionManager").GetComponent<QuestionManager>();

    //            GameObject.Find("PanelUI").transform.FindChild("TextLevel").GetComponent<Text>().text = "第" + level + "关";

    //            //若正确答案大于零
    //            if (answerManager.correctAnswer > 0)
    //            {
    //                //若可以下一题并且出生点子物体数为0(即散场完毕)
    //                if ((answerManager.canNext) && (questionManager.leftCreatePoint.childCount <= 0))
    //                {
    //                    answerManager.houseFinishFirstMoving = false;
    //                    answerManager.houseMoveSecondTime = true;
    //                }
    //            }
    //            else//否则答案等于0时，等待房屋掀起完毕后才可以下降
    //            {
    //                if (answerManager.canNext)
    //                {
    //                    while (answerManager.houseFinishFirstMoving)
    //                    {
    //                        answerManager.houseFinishFirstMoving = false;
    //                        answerManager.houseMoveSecondTime = true;
    //                    }
    //                }
    //            }
    //            //当房屋完成第二次移动(回落)时，开始按钮恢复，选项恢复，窗恢复
    //            if (answerManager.houseFinishSecondMoving)
    //            {
    //                correctCount = answerManager.correctCount;
    //                whenHouseFinishSecondMoving();
    //                answerManager.canNext = false;
    //                answerManager.houseFinishSecondMoving = false;
    //            }
    //        }

    //        //audioSource.volume = audioSlider.value;
    //        //PlayerPrefs.SetFloat("slider", audioSlider.value);

    //    }

    //    //当房屋完成第二次移动(回落)时，开始按钮恢复，选项恢复，窗恢复
    //    void whenHouseFinishSecondMoving()
    //    {
    //        if (correctCount == 5)
    //        {
    //            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("PassLevel").gameObject.SetActive(true);
    //            correctCount = -1;//防止意外
    //        }
    //        else
    //        {
    //            GameObject.Find("Canvas").transform.FindChild("BeginButton").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("A").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("B").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("C").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("D").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("A").FindChild("correctImage").gameObject.SetActive(false);
    //            GameObject.Find("Canvas").transform.FindChild("B").FindChild("correctImage").gameObject.SetActive(false);
    //            GameObject.Find("Canvas").transform.FindChild("C").FindChild("correctImage").gameObject.SetActive(false);
    //            GameObject.Find("Canvas").transform.FindChild("D").FindChild("correctImage").gameObject.SetActive(false);
    //            GameObject.Find("Canvas").transform.FindChild("house").FindChild("window").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("house").FindChild("WenHao").gameObject.SetActive(false);
    //            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("TestID").GetComponent<Text>().text = "";
    //            GameObject.Find("Canvas").transform.FindChild("house").gameObject.SetActive(true);
    //            GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("Demystify").gameObject.SetActive(false);
    //        }
    //    }
    #region 场景1的按钮点击事件
    //    //场景1的进入游戏按钮
    //    public void OnBeginButtonClick1()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("beginBtn").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").gameObject.SetActive(true);
    //    }
    //    //激活设置面板
    //    public void OnEnableSettingPanel()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("settingBtn").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("setPanel").gameObject.SetActive(true);
    //    }
    //    //关闭设置面板
    //    public void OnDisableSettingPanel()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("setPanel").FindChild("exitButton").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("setPanel").gameObject.SetActive(false);
    //    }
    //    //激活帮助面板
    //    public void OnEnableHelpPanel()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("helpBtn").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("helpPanel").gameObject.SetActive(true);
    //    }
    //    //关闭帮助面板
    //    public void OnDisableHelpPanel()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("helpPanel").FindChild("exitButton").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("helpPanel").gameObject.SetActive(false);
    //    }
    //    //关闭关卡面板
    //    public void OnDisableLevelPanel()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("exitButton").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").gameObject.SetActive(false);
    //    }
    //    public void OnLevel1()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn1").GetComponent<AudioSource>().Play();

    //        level = 1;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();


    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel2()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn2").GetComponent<AudioSource>().Play();

    //        level = 2;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel3()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn3").GetComponent<AudioSource>().Play();

    //        level = 3;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel4()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn4").GetComponent<AudioSource>().Play();

    //        level = 4;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel5()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn5").GetComponent<AudioSource>().Play();

    //        level = 5;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel6()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn6").GetComponent<AudioSource>().Play();

    //        level = 6;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel7()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn7").GetComponent<AudioSource>().Play();

    //        level = 7;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel8()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn8").GetComponent<AudioSource>().Play();

    //        level = 8;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    //    public void OnLevel9()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("levelPanel").FindChild("levelBtn9").GetComponent<AudioSource>().Play();

    //        level = 9;
    //        //播放音乐
    //        audioSource.clip = bgm2;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene2");
    //    }
    #endregion

    //    //场景2的开始按钮
    //    public void OnBeginButtonClick2()
    //    {
    //        //除了生成题目以外，这里做其他逻辑
    //        GameObject.Find("Canvas").transform.FindChild("BeginButton").GetComponent<AudioSource>().Play();
    //        GameObject.Find("Canvas").transform.FindChild("BeginButton").gameObject.SetActive(false);
    //        //生成题目在其他脚本的方法里实现
    //    }
    //    //场景2的返回按钮
    //    public void OnBackButtonClick()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("backButton").GetComponent<AudioSource>().Play();
    //        //播放音乐
    //        audioSource.clip = bgm1;
    //        audioSource.Play();
    //        SceneManager.LoadScene("Scene1");
    //    }
    //    //场景2揭秘按钮
    //    public void OnDemystifyClick()
    //    {
    //        GameObject.Find("Canvas").transform.FindChild("PanelUI").FindChild("Demystify").GetComponent<AudioSource>().Play();
    //        GameObject house = GameObject.Find("Canvas").transform.FindChild("house").gameObject;
    //        if (house.activeInHierarchy)
    //            house.SetActive(false);
    //        else
    //            house.SetActive(true);

    //    }
}
