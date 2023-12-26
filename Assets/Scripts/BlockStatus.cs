using UnityEngine;

public class BlockStatus : MonoBehaviour
{
    public BlockType Status;
    public BlockPos Pos { get; set; }
    
    private GameObject Player;
    private SpriteRenderer sr;
    private BlockType Before;
    private float MoveableRadius;
    private bool IsStepped;

    private GameObject Marker;
    private Animator MarkerAnimator;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        Before = BlockType.NONE;
        MoveableRadius = sr.bounds.size.x * 2f;
        IsStepped = false;

        Marker = transform.Find("Marker").gameObject;
        MarkerAnimator = Marker.GetComponent<Animator>(); 
    }

    private void Start()
    {
        SpriteUpdate();
    }

    public void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);

        if (distance == 0)
        {
            // �� ����� ��� ����
            Status = BlockType.STEPABLE;
            IsStepped = true;
        }
        else if (IsStepped && distance != 0)
        {
            // �� �̻� �� ���� ��� ���� ����
            Status = BlockType.FILLED;
        }
        else if (distance < MoveableRadius)
        {
            // �� ������� �̵��� �� ����
            Status = BlockType.MOVEABLE;
        }
        else
        {
            Status = BlockType.STEPABLE;
        }

        SpriteUpdate();
    }

    private void DestroyBlock()
    {
        Destroy(Marker);
        Destroy(GetComponent<BlockStatus>());
    }

    private void SpriteUpdate()
    {
        if (Status != Before)
        {
            Before = Status;
        }
        else
        {
            return;
        }

        print($"[Sprite Updated] {Pos.x}, {Pos.y}");

        if (Status == BlockType.BORDER)
        {
            sr.sprite = Resources.Load<Sprite>("Tile/Border");
            DestroyBlock();
        }
        else if (Status == BlockType.FILLED)
        {
            sr.sprite = Resources.Load<Sprite>("Tile/Filled");
            DestroyBlock();
        }
        else if (Status == BlockType.STEPABLE || Status == BlockType.MOVEABLE)
        {
            sr.sprite = Resources.Load<Sprite>("Tile/Stepable");

            Marker.SetActive(Status == BlockType.MOVEABLE);
            MarkerAnimator.SetBool("Marker", Status == BlockType.MOVEABLE);
        }
    }
}
