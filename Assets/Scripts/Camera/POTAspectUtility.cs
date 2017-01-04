using UnityEngine;

public class POTAspectUtility : MonoBehaviour
{

    public static int[] AspectRatio = new int[2] { 16, 11 };
    public static int PPU = 32;
    public static int multiplier {
        get {
            float multiplierW = (float)Screen.width / (AspectRatio[0] * PPU);
            float multiplierH = (float)Screen.height / (AspectRatio[1] * PPU);

            int intMultiplierW = Mathf.Max(1, Mathf.FloorToInt(multiplierW + 1.0f / AspectRatio[0]));
            int intMultiplierH = Mathf.Max(1, Mathf.FloorToInt(multiplierH + 1.0f / AspectRatio[1]));

            return intMultiplierW < intMultiplierH ? intMultiplierW : intMultiplierH;

        }
    }
    static Camera cam;
    static Camera backgroundCam;

    void Awake()
    {

        cam = GetComponent<Camera>();
        if (!cam)
        {
            cam = Camera.main;
        }
        if (!cam)
        {
            Debug.LogError("No camera available");
            return;
        }
        UpdateCamera();
    }

    void Update()
    {
        if (!cam)
            Awake();
        else //( condtionals regarding screen size changes)
            UpdateCamera();
    }

    void UpdateCamera()
    {
        float desiredAR = (float)AspectRatio[0] / AspectRatio[1];
        float currentAR = (float)Screen.width / Screen.height;

        float multiplierW = (float)Screen.width / (AspectRatio[0] * PPU);
        float multiplierH = (float)Screen.height / (AspectRatio[1] * PPU);

        int intMultiplierW = Mathf.Max(1, Mathf.FloorToInt(multiplierW + 1.0f / AspectRatio[0]));
        int intMultiplierH = Mathf.Max(1, Mathf.FloorToInt(multiplierH + 1.0f / AspectRatio[1]));

        int multiplier = intMultiplierW < intMultiplierH ? intMultiplierW : intMultiplierH;

        float insetW = 1.0f - (AspectRatio[0] * PPU * multiplier) / (float)Screen.width;
        float insetH = 1.0f - (AspectRatio[1] * PPU * multiplier) / (float)Screen.height;

        cam.rect = new Rect(insetW / 2, insetH / 2, 1.0f - insetW, 1.0f - insetH);
        cam.orthographicSize = AspectRatio[1] / 2.0f;

    }

    public static int screenHeight
    {
        get
        {
            return (int)(Screen.height * cam.rect.height);
        }
    }

    public static int screenWidth
    {
        get
        {
            return (int)(Screen.width * cam.rect.width);
        }
    }

    public static int xOffset
    {
        get
        {
            return (int)(Screen.width * cam.rect.x);
        }
    }

    public static int yOffset
    {
        get
        {
            return (int)(Screen.height * cam.rect.y);
        }
    }

    public static Rect screenRect
    {
        get
        {
            return new Rect(cam.rect.x * Screen.width, cam.rect.y * Screen.height, cam.rect.width * Screen.width, cam.rect.height * Screen.height);
        }
    }

    public static Vector3 mousePosition
    {
        get
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.y -= (int)(cam.rect.y * Screen.height);
            mousePos.x -= (int)(cam.rect.x * Screen.width);
            return mousePos;
        }
    }

    public static Vector2 guiMousePosition
    {
        get
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = Mathf.Clamp(mousePos.y, cam.rect.y * Screen.height, cam.rect.y * Screen.height + cam.rect.height * Screen.height);
            mousePos.x = Mathf.Clamp(mousePos.x, cam.rect.x * Screen.width, cam.rect.x * Screen.width + cam.rect.width * Screen.width);
            return mousePos;
        }
    }
}