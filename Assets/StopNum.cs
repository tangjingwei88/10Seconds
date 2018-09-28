using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopNum : MonoBehaviour {

    public Text countLable;
    public Text btnText;
    public Text recordText;
    public Text marginText;
    public Text FailText;
    public Text SuccessText;
    public Text NewRecordText;

    public float timeNum = 0.011f;
    public bool running = false;
    public bool isClick = true;

    void Awake()
    {
        if (PlayerPrefs.GetFloat("Record") == 0)
        {
            PlayerPrefs.SetFloat("Record", 0.011f);
        }
        recordText.gameObject.SetActive(true);
        marginText.gameObject.SetActive(true);
        SuccessText.gameObject.SetActive(false);
        FailText.gameObject.SetActive(false);
        NewRecordText.gameObject.SetActive(false);
        running = false;
        isClick = true;
        timeNum = PlayerPrefs.GetFloat("Record");
        recordText.text = Get2float(timeNum).ToString() ;
        marginText.text = CompareBS(timeNum);
    }

    void Update()
    {
        if (running)
        {
            timeNum += Time.deltaTime;
            countLable.text = AddLastZero(Get2float(timeNum).ToString());
            //Debug.LogError("@@" + Get2float(timeNum).ToString() + "#ln:" + Get2float(timeNum).ToString().Length);
            
        }
    }

    public void OnClick()
    {
        if (!isClick)
        {
            running = false;
            isClick = true;
            btnText.text = "Start";
            //10秒成功
            if (Get2float(timeNum) == 10.00)
            {
                ShowWin();
            }
            else if (CompareRecord(timeNum))
            {
                //新纪录
                PlayerPrefs.SetFloat("Record",timeNum);
                recordText.text = AddLastZero(Get2float(timeNum).ToString());
                //误差
                marginText.text = CompareBS(timeNum);
                NewRecordText.gameObject.SetActive(true);
            }
            else
            {
                //失败
                FailText.gameObject.SetActive(true);
            }
        }
        else {
            timeNum = 0.00f;
            running = true;
            isClick = false;
            btnText.text = "Stop";
            SuccessText.gameObject.SetActive(false);
            FailText.gameObject.SetActive(false);
            NewRecordText.gameObject.SetActive(false);
        }
    }


    public bool CompareRecord(float nowScore)
    {
        float record = PlayerPrefs.GetFloat("Record");
        if(Mathf.Abs(1000-nowScore * 100) > Mathf.Abs(1000 - record * 100))
        {
            return false;
        }
        else{
            return true;
        }
    }

    public string CompareBS(float nowScore)
    {
        nowScore = Get2float(nowScore);

        if (nowScore > 10.00f)
        {
            marginText.color = new Color32(255, 0, 0, 255);

            //string str = recordText.ToString();
            //int count = str.LastIndexOf(".");
            //return "+" + str.Substring(count ,count + 2);

            nowScore = nowScore * 100 - 1000;
            Debug.LogError("@@#" + nowScore.ToString());
            return "+" + Mathf.Abs(nowScore/100.0f);
        }
        else if (nowScore < 10.00f)
        {
            marginText.color = new Color32(0, 255, 0, 255);
            return "-" + Mathf.Abs(nowScore - 10.00f);
        }
        else
            return "" + 0.00;
    }

    //获取小数点后两位（不四舍五入）
    public float Get2float(float nowScore)
    {
        string str = (nowScore * 100).ToString();
        int count = str.LastIndexOf(".");
        float score = float.Parse(str.Substring(0, count));
        return score / 100.0f;
    }

    public string AddLastZero(string str)
    {
        if (float.Parse(str) < 10)
        {
            if (str.Length == 1)
                return str + ".00";
            else if (str.Length == 3)
                return str + "0";
            else
                return str;
        }
        else {
            if (str.Length == 2)
                return str + ".00";
            else if (str.Length == 4)
                return str + "0";
            else
                return str;
        }
    }

    public void ClearRecord()
    {
        PlayerPrefs.SetFloat("Record", 0.011f);
        timeNum = PlayerPrefs.GetFloat("Record");
        recordText.text = Get2float(timeNum).ToString();
        marginText.text = CompareBS(timeNum);
    }

    public void ShowWin()
    {
        recordText.gameObject.SetActive(false);
        marginText.gameObject.SetActive(false);
        SuccessText.gameObject.SetActive(true);
    }
}
