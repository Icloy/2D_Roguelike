using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minimumDevideRate; //������ �������� �ּ� ����
    [SerializeField] float maximumDevideRate; //������ �������� �ִ� ����
    [SerializeField] private int maximumDepth; //Ʈ���� ����, ���� ���� ���� �� �ڼ��� ������ ��
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; //���� �����ϴ� Ÿ��
    [SerializeField] Tile wallTile; //��� �ܺθ� ���������� �� Ÿ��
    [SerializeField] Tile outTile; //�� �ܺ��� Ÿ��
    [SerializeField] GameObject enter;
    [SerializeField] GameObject exit;
    [SerializeField] private List<RectInt> orderedRooms = new List<RectInt>();
    private MonsterList monsterlist;

    void Start()
    {
        FillBackground();//�� �ε� �� ���δ� �ٱ�Ÿ�Ϸ� ����
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y));
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall(); //�ٱ��� ���� ������ ������ ������ ĥ���ִ� �Լ�
        monsterlist = GetComponent<MonsterList>();
        monsterlist.insertmonster();
        CreateObjectInRoom(orderedRooms);
    }

    void update()
    {

    }

    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; //���� ���ϴ� ���̿� �����ϸ� �� �������� �ʴ´�.
                                       //�� ���� ��쿡��

        int maxLength = tree.nodeRect.width;
        //���ο� ������ �� ����� ������, ���ΰ� ��ٸ� �� ��, ��� ���ΰ� �� ��ٸ� ��, �Ʒ��� �����ְ� �� ���̴�.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDevideRate));
        //���� �� �ִ� �ִ� ���̿� �ּ� �����߿��� �������� �� ���� ����
        if (tree.nodeRect.width >= tree.nodeRect.height) //���ΰ� �� ����� ��쿡�� �� ��� ������ �� ���̸�, �� ��쿡�� ���� ���̴� ������ �ʴ´�.
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴ� �����̹Ƿ� ������ ������, ���� ���̴� ������ ���� �������� �־��ش�.
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴܿ��� ���������� ���� ���̸�ŭ �̵��� ��ġ�̸�, ���� ���̴� ���� ���α��̿��� ���� ���� ���ΰ��� �� ������ �κ��� �ȴ�. 
        }
        else
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
        }
        tree.leftNode.parNode = tree; //�ڽĳ����� �θ��带 �������� ���� ����
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1); //����, ������ �ڽ� ���鵵 �����ش�.
        Divide(tree.rightNode, n + 1);//����, ������ �ڽ� ���鵵 �����ش�.
    }

    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth)
        {
            // ��尡 ��������� ��� �� ����
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
            // ��尡 ������尡 �ƴ� ��� ���� ��� ����
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        // ������ȸ ������� ���ĵ� ���� ����Ʈ ��ȯ
        return rect;
    }

    public void GetRoomsInOrder(Node root, List<RectInt> rooms, RectInt rect)
    {
        if (root != null)
        {
            // ���� ����Ʈ���� ���� ����Ʈ�� �߰�
            GetRoomsInOrder(root.leftNode, rooms, rect);
            // ���� ����� ���� ����Ʈ�� �߰�
            if (root.leftNode == null && root.rightNode == null)
            {
                rooms.Add(rect);
            }
            // ������ ����Ʈ���� ���� ����Ʈ�� �߰�
            GetRoomsInOrder(root.rightNode, rooms, rect);
        }
    }

    void CreateObjectInRoom(List<RectInt> rooms)
    {
        // ����Ʈ���� �ش� ���� ���� ��������
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
        if (n == maximumDepth) //�ش� ��尡 ��������� ���� ����� �� ���̴�.
        {
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width -1);
            //���� ���� �ּ� ũ��� ����� ���α����� ����, �ִ� ũ��� ���α��̺��� 1 �۰� ������ �� �� ���� ���� ������ ���� �����ش�.
            int height = Random.Range(rect.height / 2, rect.height-1);
            //���̵� ���� ����.
            int x = rect.x + Random.Range(1, rect.width - width);
            //���� x��ǥ�̴�. ���� 0�� �ȴٸ� �پ� �ִ� ��� �������� ������,�ּڰ��� 1�� ���ְ�, �ִ��� ���� ����� ���ο��� ���� ���α��̸� �� �� ���̴�.
            int y = rect.y + Random.Range(1, rect.height - height);
            //y��ǥ�� ���� ����.
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
        if (n == maximumDepth) //���� ����� ���� �ڽ��� ����.
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
        //���� �����ÿ��� ������ ������� �κ��� room tile�� ä��� ����
        GenerateLoad(tree.leftNode, n + 1); //�ڽ� ���鵵 Ž��
        GenerateLoad(tree.rightNode, n + 1);
    }

    void FillBackground() //����� ä��� �Լ�, �� load�� ���� ���� ���ش�.
    {
        for (int i = -10; i < mapSize.x + 10; i++) //�ٱ�Ÿ���� �� �����ڸ��� ���� ������� �ʰ�
        //�� ũ�⺸�� �а� ä���ش�.
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }
    void FillWall() //�� Ÿ�ϰ� �ٱ� Ÿ���� ������ �κ�
    {
        for (int i = -1; i < mapSize.x; i++) //Ÿ�� ��ü�� ��ȸ
        {
            for (int j = -1; j < mapSize.y; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //�ٱ�Ÿ�� �� ���
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;//�ٱ� Ÿ�� ���� 8������ Ž���ؼ� room tile�� �ִٸ� wall tile�� �ٲ��ش�.
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
    { //room�� rect������ �޾Ƽ� tile�� set���ִ� �Լ�
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }

}