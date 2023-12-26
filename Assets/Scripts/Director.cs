using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField]
    public float StepableRate;

    [SerializeField]
    public int StartMapSize;

    [SerializeField]
    private TMP_Text ScoreDisplay;

    [SerializeField]
    private int StartX;
    [SerializeField]
    private int StartY;

    public static int Score;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private CameraController cameraController;

    private Dictionary<BlockPos, GameObject> BlockMap;
    private CreateBlock Create;

    private void Awake()
    {
        Create = GetComponent<CreateBlock>();
        BlockMap = new();
        Score = 0;
    }

    private void Start()
    {
        var pc = Player.AddComponent<PlayerController>();
        pc.BlockSize = Create.BlockSize;
        pc.SetPos(new BlockPos { x = StartX, y = StartY });

        for (int x = 0; x < StartMapSize; x++)
        {
            for (int y = 0; y < StartMapSize; y++)
            {
                CreateBlock(
                    new BlockPos
                    {
                        x = x,
                        y = y
                    }
                );
            }
        }
    }

    private void Update()
    {
        ScoreDisplay.text = $"{Score:N0}";
    
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

            var bs = hit.transform?.gameObject.GetComponent<BlockStatus>();

            if (bs != null && bs.Status == BlockType.MOVEABLE)
            {
                Score += 1;
                Player.GetComponent<PlayerController>().SetPos(bs.Pos);
                StartCoroutine(PlayerMoveAfter());
            }
        }
    }

    private void CreateBlock(BlockPos pos)
    {
        if (BlockMap.ContainsKey(pos))
        {
            print($"Already genrated block pos x={pos.x}, y={pos.y}");
            return;
        }

        GameObject block = Create.GetNewBlock(in StepableRate, in pos.x, in pos.y);

        block.transform.position = new Vector2(
            pos.x * Create.BlockSize,
            pos.y * Create.BlockSize
        );

        if (pos.x == 0 || pos.y == 0)
        {
            block.GetComponent<BlockStatus>().Status = BlockType.BORDER;
        }
        else if (pos.x == StartX && pos.y == StartY)
        {
            block.GetComponent<BlockStatus>().Status = BlockType.STEPABLE;
        }

        BlockMap[pos] = block;
    }

    private IEnumerator PlayerMoveAfter()
    {
        yield return null;

        if (GameObject.Find("Marker") == null)
        {
            cameraController.SetGameOver(AfterGameOver);
        }
    }

    private IEnumerator AfterGameOver()
    {
        yield return null;
        print("GameOver Finish!");
    }
}
