using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Room : MonoBehaviour
{
    protected RoomType type;

    [Header("Room Info")]
    [SerializeField] public float height;
    [SerializeField] public float width;
    [SerializeField] public Access upperAccess;
    [SerializeField] public Access lowerAccess;
    [SerializeField] public Access leftAccess;
    [SerializeField] public Access rightAccess;

    [Header("Room Element Generate Info")]
    [SerializeField] protected Tilemap groundTilemap;

    protected List<Vector2> flatPositions = null;
    protected int usedFlatPositionIndexEnd = 0;

    public void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        PreGenerateRoom(_manager, _currentLine, _index);
        GenerateCurrentRoom(_manager, _currentLine, _index);
        GenerateNextRoom(_manager, _currentLine, _index);
    }
    protected virtual void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        flatPositions = GetFlatPositionsInRoomByRadius(1);
        usedFlatPositionIndexEnd = 0;
    }

    protected virtual void GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        GenerateDecorations(_manager);
    }

    protected void GenerateDecorations(MapGenerateManager _manager)
    {
        int randomDecorationAmount = Random.Range(_manager.minDecorationAmount, _manager.maxDecorationAmount);
        int currentDecorationAmount = 0;

        while (currentDecorationAmount < randomDecorationAmount)
        {
            Sprite randomDecorationSprite = _manager.decorations[Random.Range(0, _manager.decorations.Count)];

            if (!HaveFreePosition())
            {
                break;//�޿���λ��
            }
            Vector3 randomPosition = GetRandomNonOverlapPosition(Mathf.CeilToInt(randomDecorationSprite.bounds.size.x));
            randomPosition += new Vector3(0, 1f);//��Ƭ��С
            randomPosition += new Vector3(0, randomDecorationSprite.bounds.size.y / 2);//sprite�߶�

            GameObject newDecoration = Instantiate(_manager.decorationPrefab, randomPosition, Quaternion.identity);
            newDecoration.GetComponent<SpriteRenderer>().sprite = randomDecorationSprite;
            ++currentDecorationAmount;
        }
    }
    private bool HaveFreePosition()
    {
        if (usedFlatPositionIndexEnd >= flatPositions.Count)
        {
            return false;//�޿���λ��
        }
        else
        {
            return true;
        }
    }
    private Vector3 GetRandomNonOverlapPosition(int _width)
    {
        int randomPositionIndex = Random.Range(usedFlatPositionIndexEnd, flatPositions.Count);
        Vector3 randomPosition = flatPositions[randomPositionIndex];

        int usedPositionIndex = randomPositionIndex - _width / 2;
        if(usedPositionIndex < usedFlatPositionIndexEnd)
        {
            usedPositionIndex = usedFlatPositionIndexEnd;
        }
        int end = randomPositionIndex + _width / 2;
        if(end >= flatPositions.Count)
        {
            end = flatPositions.Count - 1;
        }
        for (;usedPositionIndex <= end; usedPositionIndex++)
        {
            Vector3 usedPosition = flatPositions[usedPositionIndex];
            flatPositions[usedPositionIndex] = flatPositions[usedFlatPositionIndexEnd];
            flatPositions[usedFlatPositionIndexEnd] = usedPosition;
            ++usedFlatPositionIndexEnd;
        }

        return randomPosition;
    }

    protected virtual void GenerateNextRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        Debug.Log(_index);

        Room newRoom = null;
        //�ж���һ����������
        RoomType roomType = _currentLine.GetNextRoomType(_index, type);

        Debug.Log(roomType);

        switch (roomType)
        {
            case RoomType.Battle:
                newRoom = GetNewRoomByPrefabList(_manager.battleRoomPrefabs);
                break;
            case RoomType.Passage:
                newRoom = GetNewRoomByPrefabList(_manager.passageRoomPrefabs);
                break;
            case RoomType.Exit:
                newRoom = GetNewRoomByPrefabList(_manager.exitRoomPrefabs);
                break;
            case RoomType.Reward:
                newRoom = GetNewRoomByPrefabList(_manager.rewardRoomPrefabs);
                break;
        }
        newRoom.GenerateRoom(_manager, _currentLine, _index + 1);
    }

    protected Room GetNewRoomByPrefabList(List<GameObject> _list)
    {
        //ѡһ������
        int battleRoomIndex = Random.Range(0, _list.Count);

        //���㷿��λ��
        Room nextBattleRoom = _list[battleRoomIndex].GetComponent<Room>();
        Vector3 nextBattleRoomPosition = GetNextRoomPosition(rightAccess, nextBattleRoom);

        //���ɷ���
        Room newBattleRoom =
            Instantiate(_list[battleRoomIndex], nextBattleRoomPosition, Quaternion.identity).GetComponent<Room>();
        return newBattleRoom;
    }

    protected Vector3 GetNextRoomPosition(Access _exitAccess, Room _nextRoom)
    {
        Transform nextRoomEnterTransform;
        if (_exitAccess == upperAccess)
        {
            nextRoomEnterTransform = _nextRoom.lowerAccess.transform;
        }
        else if (upperAccess == lowerAccess)
        {
            nextRoomEnterTransform = _nextRoom.upperAccess.transform;
        }
        else if (upperAccess == leftAccess)
        {
            nextRoomEnterTransform = _nextRoom.rightAccess.transform;
        }
        else
        {
            nextRoomEnterTransform = _nextRoom.leftAccess.transform;
        }

        return _exitAccess.transform.position - nextRoomEnterTransform.position;
    }

    protected List<Vector2> GetFlatPositionsInRoomByRadius(int _flatRadius)
    {
        List<Vector2> flatPositions = new List<Vector2>();

        Vector3Int lowerLeftCoo = groundTilemap.cellBounds.min;

        //���������ڵ�ÿ����Ƭλ��
        for (int x = lowerLeftCoo.x + _flatRadius; x < (this.height + lowerLeftCoo.x - _flatRadius); x++)
        {
            for (int y = lowerLeftCoo.y; y < (this.width + lowerLeftCoo.y - 1); y++)//��߲������Ȼû�з��飬����Ҫ�ж�
            {
                

                //��һ����Ƭλ�ü�������flatRadius���ڵ�������Ƭλ�ö������������˴�����Ƭ�Ҵ˴��Ϸ�û����Ƭ
                //�򣬴˴���һ��ƽ̹λ��
                bool isSuit = true;
                for(int flatCheckX = x - _flatRadius; flatCheckX <= x + _flatRadius; flatCheckX++)
                {
                    if (groundTilemap.GetTile(new Vector3Int(x, y, 0)) == null 
                        || groundTilemap.GetTile(new Vector3Int(x, y + 1, 0)) != null
                        )//����˴���Ϊ�շ���
                    {
                        isSuit = false;
                    }
                }
                if(isSuit)
                {
                    flatPositions.Add((Vector2)groundTilemap.CellToWorld(new Vector3Int(x, y)));
                }
            }
        }

        return flatPositions;
    }
}
