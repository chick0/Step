using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float BlockSize;
    public BlockPos Pos { get; private set; }

    public void SetPos(BlockPos pos)
    {
        Pos = pos;
        transform.position = new Vector3(pos.x * BlockSize, pos.y * BlockSize, -1);
    }
}
