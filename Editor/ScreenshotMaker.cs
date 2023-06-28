using UnityEngine;
using UnityEditor;

public class ScreenshotMaker : EditorWindow
{
    private string filePath = "Screenshots";
    private string fileName = "screenshot";
    private string imageFormat = "PNG";

    [MenuItem("Tools/Screenshot Maker")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotMaker>("Screenshot Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Screenshot Maker", EditorStyles.boldLabel);

        filePath = EditorGUILayout.TextField("File Path", filePath);
        fileName = EditorGUILayout.TextField("File Name", fileName);
        imageFormat = EditorGUILayout.TextField("Image Format (PNG or JPG)", imageFormat);

        if (GUILayout.Button("Take Screenshot"))
        {
            TakeScreenshot();
        }
    }

    private void TakeScreenshot()
    {
        if (!System.IO.Directory.Exists(filePath))
        {
            System.IO.Directory.CreateDirectory(filePath);
        }

        string fullPath = string.Format("{0}/{1}.{2}", filePath, fileName, imageFormat);
        int width = 3840; // 4K width
        int height = 2160; // 4K height

        // Render the screenshot to a RenderTexture
        Camera camera = Camera.main;
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        // Save the screenshot to disk
        byte[] bytes;
        if (imageFormat.ToUpper() == "PNG")
        {
            bytes = screenshot.EncodeToPNG();
        }
        else
        {
            bytes = screenshot.EncodeToJPG();
        }
        System.IO.File.WriteAllBytes(fullPath, bytes);

        // Reset the camera and render texture
        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        Debug.Log("Screenshot saved to: " + fullPath);
    }
}
