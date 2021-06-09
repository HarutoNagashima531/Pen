using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 接触判定用のクラス
/// </summary>
public class Trigger : MonoBehaviour
{

    /// <summary>
    /// CrackLength.csに渡すための準備
    /// </summary>
    private GameObject recCrackObj;
    private RecCrack RCScript;
    
    

    void Start()
    {
        ///emptyオブジェクトのRecCrackObjを取得
        recCrackObj = GameObject.Find("RecCrackObj");
        RCScript = recCrackObj.GetComponent<RecCrack>();
        
    }


    void OnTriggerEnter(Collider other)
    {
        ///RecCrackのConStatusにContactを代入
        RCScript.ConStatus = true;
    }

    void OnTriggerExit(Collider other)
    {
        ///RecCrackのConStatusにContactを代入
        RCScript.ConStatus = false;
        RCScript.n = 0;
    }
}