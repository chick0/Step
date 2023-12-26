using UnityEngine;

public class CreateBlock : MonoBehaviour
{
    private GameObject Prefab;

    private void Awake()
    {
        Prefab = Resources.Load<GameObject>("Block");
    }

    public float BlockSize => Prefab.GetComponent<SpriteRenderer>().bounds.size.x;

    public GameObject GetNewBlock(in float stepableRate, in int x, in int y)
    {
        GameObject block = Instantiate(Prefab);

        BlockStatus bs = block.AddComponent<BlockStatus>();
        bs.name = $"{x}, {y}";

        bs.Pos = new BlockPos
        {
            x = x,
            y = y
        };

        if (Random.Range(0f, 100f) <= stepableRate)
        {
            bs.Status = BlockType.STEPABLE;
        }
        else
        {
            bs.Status = BlockType.FILLED;
        }

        return block;
    }
}
