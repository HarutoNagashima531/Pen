using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public GameObject data_object = null; // Textオブジェクト

    /// <summary>
    /// CrackLength.csからjosnstrをもらうための準備
    /// </summary>
    private GameObject recCrackObj;
    private RecCrack RCScript;
    public string DisplayText = "null";

    // Start is called before the first frame update
    void Start()
    {
        ///emptyオブジェクトのRecCrackObjを取得
        recCrackObj = GameObject.Find("RecCrackObj");
        RCScript = recCrackObj.GetComponent<RecCrack>();
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトからTextコンポーネントを取得
        Text score_text = data_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        DisplayText = RCScript.lineLength.ToString();
        score_text.text = DisplayText;
    }
}
