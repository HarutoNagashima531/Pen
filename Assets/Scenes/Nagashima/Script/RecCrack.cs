using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RecCrack : MonoBehaviour
{
    /// <summary>
    /// 変化点を保持するリスト型変数
    /// </summary>
    private List<CrackPoint> cPoints = null;

    [SerializeField]
    private GameObject linePrefab;



    /// <summary>
    /// スタートボタンが押されているか
    /// </summary>
    private bool ChalkingStatus = false;


    private CrackInfoObject crackInfoObject;

    //接触判定
    public bool ConStatus = false;
    //接触した瞬間だけを記録したいからこれを使う
    public int n = 0;
    //jsonに変換する前の文字列
    public string jsonstr = "null";
    // 線の全体の長さ、このスクリプトでの計算とDaraManager.csに渡すための変数
    public double lineLength = 0d;

    // Start is called before the first frame update
    void Start()
    {
        crackInfoObject = new CrackInfoObject();
        crackInfoObject.crackInfos = new List<CrackInfo>();
        Debug.Log("ひび割れ幅を選択してください");
        jsonstr = "nul";
    }

    // Update is called once per frame
    void Update()
    {

        if (ConStatus)
        {
            n++;
            if (n == 1)
            {

                ///ContactPointの座標を代入
                GameObject contactPoint = GameObject.FindWithTag("ContactPoint");

                Vector3 pos = contactPoint.transform.position;

                ///変化点を保持するリストに追加
                cPoints.Add(new CrackPoint()
                {
                    x = pos.x,
                    y = pos.y,
                    z = pos.z,
                });

            }
        }
    }

    public void startChalking()
    {
        ChalkingStatus = true;
        cPoints = new List<CrackPoint>();
        Debug.Log("記録を開始。壁にペン先を接触させてください。");

    }

    public void finishChalking()
    {
        if (cPoints == null || cPoints.Count == 1)
        {
            Debug.Log("スタートボタンを押してください。また、1つ以上の点を選択する必要があります。");
        }
        else
        {
            // 設定されたひび幅の値(単位：mm)
            //ここにつまみで設定したヒビの幅を入れる
            double selectWidth = 0.1;

            lineLength = 0d;

            for (int i = 0; i < cPoints.Count - 1; i++)
            {
                lineLength += Vector3.Distance(new Vector3(cPoints[i].x, cPoints[i].y, cPoints[i].z), new Vector3(cPoints[i + 1].x, cPoints[i + 1].y, cPoints[i + 1].z)) * 1000d;//* 1000dで単位をmmにしている
            }

            CrackInfo crackInfo = new CrackInfo()
            {
                // 単位：mm
                CrackWidth = selectWidth,
                CrackLength = lineLength,
                //crackPoint = cPoints,
            };

            // 最終的に保存する配列にチョーキング情報を追加
            crackInfoObject.crackInfos.Add(crackInfo);

            string json = JsonUtility.ToJson(crackInfo);
            //Debug.Log(json);

            // 変化点同士をつないで線を描く処理
            // ここでの線の幅はピクセル
            // mmではないので注意
            Draw(cPoints, selectWidth);

            ChalkingStatus = false;
            cPoints = null;

            Debug.Log("チョーキング完了");
        }
    }




    /// <summary>
    /// オブジェクトの配列をJson形式で保存する処理
    /// </summary>
    public void SaveCrackInformations()
    {
        if (crackInfoObject.crackInfos.Count == 0)
        {
            Debug.Log("チョーキングデータが存在しません");
        }
        else
        {
            StreamWriter writer;
            string adress = Application.dataPath + "/crackdata.json";

            // Json形式にシリアライズ（変換）
            jsonstr = JsonUtility.ToJson(crackInfoObject, true);
            //Debug.Log(jsonstr);

            if (!File.Exists(adress))
            {
                //Debug.Log("ないから作る");
                writer = File.CreateText(adress);
                writer.Flush();
                writer.Dispose();
            }

            // Jsonデータを保存する処理
            writer = new StreamWriter(adress, false);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();

            Debug.Log("出力完了");
        }
    }

    /// <summary>
    /// // 変化点同士をつないで線を描く処理
    /// </summary>
    /// <param name="points"></param>
    private void Draw(List<CrackPoint> cPoints, double lineWidth)
    {
        GameObject line = Instantiate(linePrefab);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

        // 線の太さを設定
        // ピクセルなのでちょうどよくなるように調整する
        lineRenderer.startWidth = (float)lineWidth * 0.1f;
        lineRenderer.endWidth = (float)lineWidth * 0.1f;

        // 選択された変化点を設定
        lineRenderer.positionCount = cPoints.Count;
        // リスト型を配列にする
        Vector3[] positions = new Vector3[cPoints.Count];
        for (int i = 0; i < cPoints.Count; i++)
        {
            // 壁と同じ奥行き(z方向)にすると線が埋もれて見えないので少し浮かせる
            positions[i] = new Vector3(cPoints[i].x, cPoints[i].y, cPoints[i].z - 0.02f);
        }

        lineRenderer.SetPositions(positions);
    }




}


/// <summary>
/// 変化点を保存するクラス
/// </summary>
[System.Serializable]
public class CrackPoint
{
    public float x;
    public float y;
    public float z;
}


/// <summary>
/// ヒビ1つ分のヒビの情報
/// </summary>
[System.Serializable]
public class CrackInfo
{
    // floatだとToJsonの際に誤差が生じる
    // 単位：m
    public double CrackWidth = 1;
    public double CrackLength;
    //public List<CrackPoint> crackPoint;
}


/// <summary>
/// 全てのヒビのデータをまとめるためのクラス
/// </summary>
[System.Serializable]
public class CrackInfoObject
{
    public List<CrackInfo> crackInfos;
}
