using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameMnager : MonoBehaviour
{
    //プレハブ格納
    public GameObject discWhitePrefab;
    public GameObject SuquareInnerPrefab;
    public GameObject SuquareOuterPrefab;
    public GameObject discBlackPrefab;

    public Text text;

    //プレハブ追跡用
    GameObject[,] obj = new GameObject[8, 8];

    //ポインタの位置格納用
    public Vector2 nowPointer;

    //盤面情報格納用
    //1はBlack、-1はWhite
    public int[,] boad = new int[9, 9];

    //勝敗判定用
    public int blackCount = 0;
    public int whiteCount = 0;

    //ターン格納用
    //1はBlack、-1はWhite
    public int turn = 0;

    //画面更新用
    public int layer = 5;

    // Start is called before the first frame update
    void Start()
    {
        setBoadStartPositon();
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            changeDisc();
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
        boad[3, 4] = -1;
        boad[4, 3] = -1;

        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //pointerの位置初期化
        pointer.transform.position = new Vector2(0, 0);

        //discを取得
        GameObject discBlack = GameObject.Find("discBlack");
        GameObject discWhite = GameObject.Find("discWhite");

        //discの位置の初期化
        discBlack.transform.position = new Vector2(0, 80);
        discWhite.transform.position = new Vector2(0, 80);

        //discを描画
        drawBoad();

        //ターンを初期化
        turn = 1;

    }

    void movePointer(float x, float y)
    {
        //pointerを取得
        GameObject pointer = GameObject.Find("pointer");

        //合成元のベクトルを取得
        Vector2 addVector = new Vector2(x, y);

        //ポインターの位置を変更
        pointer.transform.position = nowPointer + addVector;


    }
   
    void changeDisc() //コマを置けるか判定し、置けるなら挟まれた敵コマをひっくり返す
    {
        int able = 0;

        //８方向全てに対して
        for (int i = 0;i < 8; i++)
        {
            //盤のそとに出ない限り
            for (int j = 0;j <= giveMax(i);j++) 
            {
                //隣に敵コマがあり、現在のpointerの位置に石がなく、そこを起点にして、その方向に味方のコマJがあれば
                if (boad[(int)nowPointer.x / 10 + j * (int)giveDirection(i).x, (int)nowPointer.y / 10 + j * (int)giveDirection(i).y] == turn 
                    && boad[(int)nowPointer.x/10,(int)nowPointer.y/10] == 0 
                    && boad[(int)nowPointer.x/10 + (int)giveDirection(i).x,(int)nowPointer.y/10 + (int)giveDirection(i).y] == -1 * turn)
                {

                    //JとNPよってに挟まれる敵コマをひっくり返す
                    for (int k= 1; k < j;k++)
                    {
                        boad[(int)nowPointer.x/10 + k*(int)giveDirection(i).x,(int)nowPointer.y/10 + k*(int)giveDirection(i).y] = turn;

                        able = 1;
                    }
                }
            }
        }

        if(able == 1) {
            //現在のpointerの位置に味方のコマNPを置き
            boad[(int)nowPointer.x / 10, (int)nowPointer.y / 10] = turn;

            turn *= -1;
        }

        drawBoad();
    }
    void drawBoad() //boadに記録された盤面を描画する
    {
        blackCount = 0;
        whiteCount = 0;

        //discを取得
        GameObject discBlack = GameObject.Find("discBlack");
        GameObject discWhite = GameObject.Find("discWhite");

        //描画の初期化
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Destroy(obj[i, j]);
            }
        }

        //boadに保存された盤面を描画
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (boad[i,j] == 1) 
                {
                    obj[i,j] = Instantiate(discBlack, new Vector2(i * 10, j * 10), Quaternion.identity);
                    blackCount++;
                }else if(boad[i, j] == -1)
                {
                    obj[i,j] = Instantiate(discWhite, new Vector2(i * 10, j * 10), Quaternion.identity);
                    whiteCount++;
                }
            }
        }
        text.text = "blackScore" + blackCount + "\r\n" + "whiteScore" + whiteCount;
    }

    Vector2 giveDirection(int i)
    {
        Vector2 direction = new Vector2(0,0);
        switch (i)
        {
            case 0:
                direction = new Vector2(1,0); break;
            case 1:
                direction = new Vector2(1,1); break;
            case 2:
                direction = new Vector2(0,1); break;
            case 3:
                direction = new Vector2(-1,1); break;
            case 4:
                direction = new Vector2(-1,0); break;
            case 5:
                direction = new Vector2(-1,-1);break;
            case 6:
                direction = new Vector2(0,-1);break;
            case 7:
                direction = new Vector2(1,-1); break;
        }
        return direction;
    }

    int giveMax(int i)
    {
        int max = 0;

        //最大でも８以下のjを１から１づつ増やしていき
        for (int j = 1;j < 9; j++)
        {
            //あるjに対して、現在位置から与えられた方向iへjだけ離れた位置をtestとし、
            Vector2 test = nowPointer / 10 + j * giveDirection(i);

            //testが盤の内部にない場合、
            if(!((int)test.x >= 0 && (int)test.x <= 7 && (int)test.y >= 0 && (int)test.y <= 7))
            {
                max = j - 1;
                break;
            }
        }

        //maxを返す
        return max;
    }
}