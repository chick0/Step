using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject FollowTarget;

    [SerializeField]
    private float Speed;

    private Camera cam;

    public delegate IEnumerator AsyncCallback();

    public void SetGameOver(AsyncCallback callback)
    {
        StartCoroutine(DecreaseCameraSize(callback));
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 next = FollowTarget.transform.position;
        next.z = -10;

        transform.position = next;
    }

    private IEnumerator DecreaseCameraSize(AsyncCallback callback)
    {
        while (cam.orthographicSize > 7f)
        {
            // 점점 가까워지는 연출
            cam.orthographicSize -= Speed * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(callback());
    }
}
