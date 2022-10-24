
class CameraLink : UnityEngine.MonoBehaviour
{
    public static UnityEngine.Camera Instance;

    void Awake()
    {
        Instance = GetComponent<UnityEngine.Camera>();
    }
}
