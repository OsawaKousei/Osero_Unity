using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMnager : MonoBehaviour
{
    //プレハブ格納
    public GameObject discWhitePrefab;
    public GameObject SuquareInnerPrefab;
    public GameObject SuquareOuterPrefab;
    public GameObject discblackPrefab;

    //ポインタの位置格納用
    public Vector2 nowPointer;

    //盤面情報格納用
    //1はBlack、2はWhite
    public int[,] boad = new int[8, 8];

    //ターン格納用
    //1はBlack、2はWhite
    public int turn = 1;

    // Start is called before the first frame update
    void Start()
    {
        setDiscStartPosition();
        setBoadStartPositon();
        setPointerStsrtPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //ポインタの位置を更新
        nowPointer = pointer.transform.position;

        //キー入力を取得
        if (Input.GetKeyDown(KeyCode.UpArrow) && nowPointer.y < 70)
        {
            movePointer(0, 10);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && 0 < nowPointer.y)
        {
            movePointer(0, -10);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && nowPointer.x < 70)
        {
            movePointer(10, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && 0 < nowPointer.x)
        {
            movePointer(-10, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return) && discSetable())
        {
            setNewDisc();
        }
    }

    void setBoadStartPositon()
    {
        //8x8の盤面を作る二重ループ
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //SquareInnerPrefabを生成
                Instantiate(SuquareInnerPrefab, new Vector2(i * 10, j * 10), Quaternion.identity);
            }
        }

        //8x8の盤面を作る二重ループ
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // //SquareOuterrPrefabを生成
                Instantiate(SuquareOuterPrefab, new Vector2(i * 10, j * 10), Quaternion.identity);
            }
        }

        //初期盤面をboadに格納
        boad[3, 3] = 1;
        boad[4, 4] = 1;
        boad[3, 4] = 2;
        boad[4, 3] = 2;

    }
    void setDiscStartPosition()
    {
        //discを取得
        GameObject discBlack = GameObject.Find("discBlack");
        GameObject discWhite = GameObject.Find("discWhite");

        //位置初期化
        discBlack.transform.position = new Vector2(0, -10);
        discWhite.transform.position = new Vector2(0, -10);

        //開始位置にセット
        Instantiate(discblackPrefab, new Vector2(30, 30), Quaternion.identity);
        Instantiate(discblackPrefab, new Vector2(40, 40), Quaternion.identity);
        Instantiate(discWhitePrefab, new Vector2(30, 40), Quaternion.identity);
        Instantiate(discWhitePrefab, new Vector2(40, 30), Quaternion.identity);
    }

    void setPointerStsrtPosition()
    {
        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //位置初期化
        pointer.transform.position = new Vector2 (0, 0);
    }

    void movePointer(float x,float y)
    {
        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //合成元のベクトルを取得
        Vector2 addVector = new Vector2 (x,y);

        //ポインターの位置を変更
        pointer.transform.position = nowPointer+ addVector;

        
    }

    void setNewDisc()
    {
        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //discを追加するベクトルを指定
        Vector2 setVector = pointer.transform.position;

        //ターンを判定
        if(turn == 1)
        {
            //discを生成
            Instantiate(discblackPrefab, setVector, Quaternion.identity);

            //ターンを変更
            turn = 2;

            //盤面を更新
            boad[(int)nowPointer.x/10,(int)nowPointer.y/10] = 1;
        }
        else
        {
            //discを変更
            Instantiate(discWhitePrefab, setVector, Quaternion.identity);

            //ターンを変更
            turn = 1;

            //盤面を更新
            boad[(int)nowPointer.x / 10, (int)nowPointer.y / 10] = 2;
        }
    }

    bool discSetable()
    {
        bool discSetable = true;

        if (boad[(int)nowPointer.x/10,(int)nowPointer.y/10] == 0) { } else { discSetable = false; }

        return discSetable;
    }
}
