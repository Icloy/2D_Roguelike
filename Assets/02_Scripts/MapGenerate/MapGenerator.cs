using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDevideRate; //공간이 나눠지는 최대 비율
    [SerializeField] private int maximumDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; //방을 구성하는 타일
    [SerializeField] Tile wallTile; //방과 외부를 구분지어줄 벽 타일
    [SerializeField] Tile outTile; //방 외부의 타일
    [SerializeField] GameObject enter;
    [SerializeField] GameObject exit;
    [SerializeField] private List<RectInt> orderedRooms = new List<RectInt>();
    private MonsterList monsterlist;

    void Start()
    {
        FillBackground();//신 로드 시 전부다 바깥타일로 덮음
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y));
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall(); //바깥과 방이 만나는 지점을 벽으로 칠해주는 함수
        monsterlist = GetComponent<MonsterList>();
        monsterlist.insertmonster();
        CreateObjectInRoom(orderedRooms);
    }

    void update()
    {

    }

    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; //내가 원하는 높이에 도달하면 더 나눠주지 않는다.
                                       //그 외의 경우에는

        int maxLength = tree.nodeRect.width;
        //가로와 세로중 더 긴것을 구한후, 가로가 길다면 위 좌, 우로 세로가 더 길다면 위, 아래로 나눠주게 될 것이다.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDevideRate));
        //나올 수 있는 최대 길이와 최소 길이중에서 랜덤으로 한 값을 선택
        if (tree.nodeRect.width >= tree.nodeRect.height) //가로가 더 길었던 경우에는 좌 우로 나누게 될 것이며, 이 경우에는 세로 길이는 변하지 않는다.
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //왼쪽 노드에 대한 정보다 
            //위치는 좌측 하단 기준이므로 변하지 않으며, 가로 길이는 위에서 구한 랜덤값을 넣어준다.
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
            //우측 노드에 대한 정보다 
            //위치는 좌측 하단에서 오른쪽으로 가로 길이만큼 이동한 위치이며, 가로 길이는 기존 가로길이에서 새로 구한 가로값을 뺀 나머지 부분이 된다. 
        }
        else
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
        }
        tree.leftNode.parNode = tree; //자식노드들의 부모노드를 나누기전 노드로 설정
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1); //왼쪽, 오른쪽 자식 노드들도 나눠준다.
        Divide(tree.rightNode, n + 1);//왼쪽, 오른쪽 자식 노드들도 나눠준다.
    }

    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth)
        {
            // 노드가 리프노드인 경우 방 생성
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);
            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);
            GetRoomsInOrder(tree, orderedRooms, rect);
            FillRoom(rect);
        }
        else
        {
            // 노드가 리프노드가 아닌 경우 하위 노드 생성
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        // 중위순회 방식으로 정렬된 방의 리스트 반환
        return rect;
    }

    public void GetRoomsInOrder(Node root, List<RectInt> rooms, RectInt rect)
    {
        if (root != null)
        {
            // 왼쪽 서브트리의 방을 리스트에 추가
            GetRoomsInOrder(root.leftNode, rooms, rect);
            // 현재 노드의 방을 리스트에 추가
            if (root.leftNode == null && root.rightNode == null)
            {
                rooms.Add(rect);
            }
            // 오른쪽 서브트리의 방을 리스트에 추가
            GetRoomsInOrder(root.rightNode, rooms, rect);
        }
    }

    void CreateObjectInRoom(List<RectInt> rooms)
    {
        // 리스트에서 해당 방의 정보 가져오기
        for (int i = 0; i < rooms.Count; i++)
        {
            RectInt room = rooms[i];
            Vector2 center = room.center;
            if (i == 0)
            {
                Instantiate(enter, new Vector3(center.x - mapSize.x / 2 - room.width / 2 + 5f + transform.position.x, center.y - mapSize.y / 2 - room.height / 2, -1), Quaternion.identity, transform);
            }
            else if (i == rooms.Count - 1)
            {
                Instantiate(exit, new Vector3(center.x - mapSize.x / 2 + room.width / 2 - 5f + transform.position.x, center.y - mapSize.y / 2 - room.height / 2, -1), Quaternion.identity, transform);
            }
            else
            {
                int monsterset = Random.Range(1, 3);
                switch (monsterset)
                {
                    case 1:
                        Instantiate(monsterlist.GetRandomFlyMonster(), new Vector3(center.x - (mapSize.x / 2) + transform.position.x - room.width / 3, center.y - mapSize.y / 2 - room.height / 2 + 6f / 2, -1), Quaternion.identity, transform);
                        Instantiate(monsterlist.GetRandomFlyMonster(), new Vector3(center.x - (mapSize.x / 2) + transform.position.x + room.width / 3, center.y - mapSize.y / 2 - room.height / 2 + 6f / 2, -1), Quaternion.identity, transform);
                        for (int j = -5; j < 6; j += 5)
                        {
                            Instantiate(monsterlist.GetRandomWalkMonster(), new Vector3(center.x - (mapSize.x / 2) + 5f + transform.position.x + j, center.y - mapSize.y / 2 - room.height / 2 + 2f / 2, -1), Quaternion.identity, transform);
                        }
                        break;
                    case 2:
                        Instantiate(monsterlist.GetRandomMiddleMonster(), new Vector3(center.x - (mapSize.x / 2) + 5f + transform.position.x, center.y - mapSize.y / 2 - room.height / 2 + 2f / 2, -1), Quaternion.identity, transform);
                        break;
                }
            }
        }
    }

    /*
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth) //해당 노드가 리프노드라면 방을 만들어 줄 것이다.
        {
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width -1);
            //방의 가로 최소 크기는 노드의 가로길이의 절반, 최대 크기는 가로길이보다 1 작게 설정한 후 그 사이 값중 랜덤한 값을 구해준다.
            int height = Random.Range(rect.height / 2, rect.height-1);
            //높이도 위와 같다.
            int x = rect.x + Random.Range(1, rect.width - width);
            //방의 x좌표이다. 만약 0이 된다면 붙어 있는 방과 합쳐지기 때문에,최솟값은 1로 해주고, 최댓값은 기존 노드의 가로에서 방의 가로길이를 빼 준 값이다.
            int y = rect.y + Random.Range(1, rect.height - height);
            //y좌표도 위와 같다.
            rect = new RectInt(x, y, width, height);
            FillRoom(rect);
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }
    */
    private void GenerateLoad(Node tree, int n)
    {
        if (n == maximumDepth) //리프 노드라면 이을 자식이 없다.
            return;
        Vector2Int leftNodeCenter = tree.leftNode.center;
        Vector2Int rightNodeCenter = tree.rightNode.center;
        Vector2Int LeftRoomSide = tree.leftNode.LeftRoomSide;
        Vector2Int RightRoomSide = tree.rightNode.RightRoomSide;
        

        for (int i = Mathf.Min(LeftRoomSide.x, RightRoomSide.x); i <= Mathf.Max(LeftRoomSide.x, RightRoomSide.x); i++)
        {
            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, LeftRoomSide.y - mapSize.y / 2 - 1, 0), roomTile);
            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, LeftRoomSide.y - mapSize.y / 2, 0), roomTile);
            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, LeftRoomSide.y - mapSize.y / 2 + 1, 0), roomTile);
        }

        for (int j = Mathf.Min(LeftRoomSide.y, RightRoomSide.y); j <= Mathf.Max(LeftRoomSide.y, RightRoomSide.y); j++)
        {
            tileMap.SetTile(new Vector3Int(RightRoomSide.x - mapSize.x / 2 - 1, j - mapSize.y / 2, 0), roomTile);
            tileMap.SetTile(new Vector3Int(RightRoomSide.x - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            tileMap.SetTile(new Vector3Int(RightRoomSide.x - mapSize.x / 2 + 1, j - mapSize.y / 2, 0), roomTile);
        }
        //이전 포스팅에서 선으로 만들었던 부분을 room tile로 채우는 과정
        GenerateLoad(tree.leftNode, n + 1); //자식 노드들도 탐색
        GenerateLoad(tree.rightNode, n + 1);
    }

    void FillBackground() //배경을 채우는 함수, 씬 load시 가장 먼저 해준다.
    {
        for (int i = -10; i < mapSize.x + 10; i++) //바깥타일은 맵 가장자리에 가도 어색하지 않게
        //맵 크기보다 넓게 채워준다.
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }
    void FillWall() //룸 타일과 바깥 타일이 만나는 부분
    {
        for (int i = -1; i < mapSize.x; i++) //타일 전체를 순회
        {
            for (int j = -1; j < mapSize.y; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //바깥타일 일 경우
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;//바깥 타일 기준 8방향을 탐색해서 room tile이 있다면 wall tile로 바꿔준다.
                            if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    private void FillRoom(RectInt rect)
    { //room의 rect정보를 받아서 tile을 set해주는 함수
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }

}