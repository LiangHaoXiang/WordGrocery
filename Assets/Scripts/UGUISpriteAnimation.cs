using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class UGUISpriteAnimation : MonoBehaviour
{
    private Image ImageSource;
    private int mCurFrame = 0;  //当前帧
    private AnswerManager answerManager;
    System.Random r = new System.Random();

    public List<Sprite> SpriteFrames;

    void Awake()
    {
        answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();
        ImageSource = transform.GetComponent<Image>();
    }
    void Start()
    {
        int x = r.Next(0, 4);
        RandomPlayer(x);
    }

    public void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        //该部分为设置成原始图片大小，如果只需要显示Image设定好的图片大小，注释掉该行即可。
        //ImageSource.SetNativeSize();
    }
    //随机玩家图片
    void RandomPlayer(int x)
    {
        switch (x)
        {
            case 0:
                ImageSource.sprite = Resources.Load<Sprite>("Arts/WG_alligator");
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_alligator"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_alligator_success"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_alligator_fail"));
                break;
            case 1:
                ImageSource.sprite = Resources.Load<Sprite>("Arts/WG_hedgehog");
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_hedgehog"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_hedgehog_success"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_hedgehog_fail"));
                break;
            case 2:
                ImageSource.sprite = Resources.Load<Sprite>("Arts/WG_pig");
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_pig"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_pig_success"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_pig_fail"));
                break;
            case 3:
                ImageSource.sprite = Resources.Load<Sprite>("Arts/WG_raccoon");
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_raccoon"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_raccoon_success"));
                SpriteFrames.Add(Resources.Load<Sprite>("Arts/WG_raccoon_fail"));
                break;
        }
    }
}
