using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
//using Unity.Mathematics;
////:///
public class Dungeon : MonoBehaviour
{
    /*
    *設定する値
    */
    public int max = 5;        //縦横のサイズ(奇数)
    public GameObject wall;    //壁用オブジェクト
    public GameObject wall2;    //壁用オブジェクト
    public GameObject wall_regid;    //壁の上に乗れないようにするための透明オブジェクト
    public GameObject floor;    //床用オブジェクト
    public GameObject maruta_up;
    public GameObject maruta_down;

    public GameObject start;   //スタート地点に配置するオブジェクト
    public GameObject goal;    //ゴール地点に配置するオブジェクト
    public GameObject breakwall; //壊れる壁
    public GameObject quiz; //壊れる壁
    public GameObject canvas;
    public GameObject goal_area;
    public GameObject goal_flag;
    

    /*
    *内部パラメータ
    */
    private Materials[,] Maze;      //マップの状態 
    public static Vector2Int startPos;    //スタートの座標
    public Vector2Int goalPos;     //ゴールの座標
    private List<Vector2Int> ReStartPos;
    //private int hight = 3;
    void Start()
    {
        ReStartPos = new List<Vector2Int>();
        Maze = new Materials[max, max];
        ini_Maze();

        //スタート地点の取得
        startPos = GetStartPosition();

        // 穴掘り開始
        goalPos = Dig(startPos.x, startPos.y);

        CreateOuterWall();

        //マップの状態に応じて壁と通路を生成する
        BuildDungeon();

        //スタート地点とゴール地点にオブジェクトを配置する
        GameObject startObj = Instantiate(start, new Vector3(startPos.x, -1, startPos.y), Quaternion.identity) as GameObject;
        GameObject goalObj = Instantiate(goal, new Vector3(goalPos.x, -1, goalPos.y), Quaternion.identity) as GameObject;
        GameObject goalareaObj = Instantiate(goal_area, new Vector3(goalPos.x, 0, goalPos.y), Quaternion.identity) as GameObject;
        GameObject goalflagObj = Instantiate(goal_flag, new Vector3(goalPos.x, -1, goalPos.y), Quaternion.identity) as GameObject;
        startObj.transform.parent = transform;
        goalObj.transform.parent = transform;
        goalareaObj.transform.parent = transform;
        goalflagObj.transform.parent = transform;
  
        DontDestroyOnLoad(transform);
        DontDestroyOnLoad(canvas);

        //SceneManager.LoadScene ("Game");
        Invoke("GotoGame",3);
    }
    private void GotoGame(){
         SceneManager.LoadScene ("Game");
    }

    private void ini_Maze()
    {
        // 全てを壁で埋める
        // 穴掘り開始候補(x,yともに偶数)座標を保持しておく
        for (int y = 0; y < max; y++)
        {
            for (int x = 0; x < max; x++)
            {
                if (x == 0 || y == 0 || x == max - 1 || y == max - 1)
                {
                    this.Maze[x, y] = Materials.Path;  // 外壁は判定の為通路にしておく(最後に戻す)
                }
                else
                {
                    this.Maze[x, y] = Materials.Wall;
                }
            }
        }
    }

    private void CreateOuterWall()
    {
        for (int y = 0; y < max; y++)
        {
            for (int x = 0; x < max; x++)
            {
                if (x == 0 || y == 0 || x == max - 1 || y == max - 1)
                {
                    this.Maze[x, y] = Materials.Wall;
                }
            }
        }
    }

    /*
    *スタート地点の取得
    */
    private Vector2Int GetStartPosition()
    {
        //ランダムでx,yを設定
        int randx = Random.Range(0, max);
        int randy = Random.Range(0, max);

        //x、yが両方共偶数になるまで繰り返す
        while (randx % 2 == 0 || randy % 2 == 0)
        {
            randx = Mathf.RoundToInt(Random.Range(0, max));
            randy = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new Vector2Int(randx, randy);
    }

    private Vector2Int Dig(int x, int y)
    {
        // 指定座標から掘れなくなるまで堀り続ける
        while (true)
        {
            // 掘り進めることができる方向のリストを作成
            List<Direction> directions = new List<Direction>();
            if (this.Maze[x, y - 1] == Materials.Wall && this.Maze[x, y - 2] == Materials.Wall)
                directions.Add(Direction.Up);
            if (this.Maze[x + 1, y] == Materials.Wall && this.Maze[x + 2, y] == Materials.Wall)
                directions.Add(Direction.Right);
            if (this.Maze[x, y + 1] == Materials.Wall && this.Maze[x, y + 2] == Materials.Wall)
                directions.Add(Direction.Down);
            if (this.Maze[x - 1, y] == Materials.Wall && this.Maze[x - 2, y] == Materials.Wall)
                directions.Add(Direction.Left);

            // 掘り進められない場合、ループを抜ける
            if (directions.Count == 0) break;

            // 指定座標を通路とし穴掘り候補座標から削除
            SetPath(x, y);
            // 掘り進められる場合はランダムに方向を決めて掘り進める
            int dirIndex = Random.Range(0, directions.Count);
            // 決まった方向に先2マス分を通路とする
            switch (directions[dirIndex])
            {
                case Direction.Up:
                    SetPath(x, --y);
                    SetPath(x, --y);
                    break;
                case Direction.Right:
                    SetPath(++x, y);
                    SetPath(++x, y);
                    break;
                case Direction.Down:
                    SetPath(x, ++y);
                    SetPath(x, ++y);
                    break;
                case Direction.Left:
                    SetPath(--x, y);
                    SetPath(--x, y);
                    break;
            }
        }

        // どこにも掘り進められない場合、穴掘り開始候補座標から掘りなおし
        // 候補座標が存在しないとき、穴掘り完了

        Vector2Int Pos = GetReStartPos();
        if (Pos != new Vector2Int(-1, -1))
        {
            Dig(Pos.x, Pos.y);
        }
        return  new Vector2Int(x, y);
    }
    //パラメータに応じてオブジェクトを生成する
    private void BuildDungeon()
    {
        //縦横1マスずつ大きくループを回し、壁とする
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                //範囲外、または壁の場合に壁オブジェクトを生成する
                if (this.Maze[i, j] == Materials.Wall)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        GameObject wallObj = Instantiate(wall, new Vector3(i, k, j), Quaternion.identity) as GameObject;
                        wallObj.transform.parent = transform;
                        GameObject wall2Obj = Instantiate(wall2, new Vector3(i, k, j), Quaternion.identity) as GameObject;
                        wall2Obj.transform.parent = transform;
                    }
                    GameObject wall_regidObj = Instantiate(wall_regid, new Vector3(i, 2, j), Quaternion.identity) as GameObject;
                    wall_regidObj.transform.parent = transform;
                }
                if(CheckPlaceGameObj(i,j)){
                    float objp=Random.Range( 0.0f, 1.0f ) ;
                    if(objp < 0.02){
                        GameObject quizObj = Instantiate(quiz, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        quizObj.transform.parent = transform;
                    }
                    else if(objp < 0.07){
                            GameObject breakwallObj = Instantiate(breakwall, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                            breakwallObj.transform.parent = transform;
                    }
                    else if(objp < 0.17){
                        GameObject maruta_downObj = Instantiate(maruta_down, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                        maruta_downObj.transform.parent = transform;
                    }
                    else if(objp < 0.27){
                        GameObject maruta_upObj = Instantiate(maruta_up, new Vector3(i, 1, j), Quaternion.identity) as GameObject;
                        maruta_upObj.transform.parent = transform;
                        GameObject wall_regidObj = Instantiate(wall_regid, new Vector3(i, 4, j), Quaternion.identity) as GameObject;
                    wall_regidObj.transform.parent = transform;
                    }
                    Maze[i, j]=Materials.GameObj;
                }
                //全ての場所に床オブジェクトを生成
                GameObject floorObj = Instantiate(floor, new Vector3(i, -1, j), Quaternion.identity) as GameObject;
                floorObj.transform.parent = transform;
            }
        }

    }

    private bool CheckPlaceGameObj(int x, int y){
        if((this.Maze[x, y] == Materials.Path) &&
        (!(x == startPos.x && y == startPos.y)) && (!(x == goalPos.x && y == goalPos.y)) && 
        (!(x == startPos.x + 1 && y == startPos.y))  && (!(x == startPos.x - 1 && y == startPos.y)) && (!(x == startPos.x && y == startPos.y + 1))  && (!(x == startPos.x && y == startPos.y - 1)) && 
        (!(x == goalPos.x + 1 && y == goalPos.y))  && (!(x == goalPos.x - 1 && y == goalPos.y)) && (!(x == goalPos.x && y == goalPos.y + 1))  && (!(x == goalPos.x && y == goalPos.y - 1)) && 
        (this.Maze[x-1, y] != Materials.GameObj) && (this.Maze[x, y-1] != Materials.GameObj) && (this.Maze[x+1, y] != Materials.GameObj) && (this.Maze[x, y+1] != Materials.GameObj)){
            return true;
        }
        return false;
    }

    // 座標を通路とする(穴掘り開始座標候補の場合は保持)
    private void SetPath(int x, int y)
    {
        this.Maze[x, y] = Materials.Path;
        if (x % 2 == 1 && y % 2 == 1)
        {
            // 穴掘り候補座標
            ReStartPos.Add(new Vector2Int(x, y));
        }
    }

    private Vector2Int GetReStartPos()
    {
        if (ReStartPos.Count == 0) return (new Vector2Int(-1, -1));

        // ランダムに開始座標を取得する
        int index = Random.Range(0, ReStartPos.Count);
        Vector2Int Pos = ReStartPos[index];
        ReStartPos.RemoveAt(index);

        return Pos;
    }

    private enum Materials
    {
        Path = 0,
        Wall = 1,
        GameObj = 3
    }

    // 方向
    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    // Update is called once per frame
    void Update()
    {

    }
}