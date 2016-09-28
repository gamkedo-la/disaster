using UnityEngine;
using System.Collections;

public class Screenshot_Handler : MonoBehaviour {
    static int nIndex = 2;

    public bool isScreenshotPressed = false;

    [Header("Screenshot Display Objects")]
    [SerializeField]
    private GameObject theDisplay;
    [SerializeField]
    private GameObject readyTextGO;
    [SerializeField]
    private GameObject successTextGO;

    public void PressDown()
    {
        if (!isScreenshotPressed)
        {
            //Display capture ready message in the center of the screen
            theDisplay.SetActive(true);
            readyTextGO.SetActive(true);
            successTextGO.SetActive(false);
            print("Create GUI Canvas... Ready to capture screen. Release trackpad when ready.");
        }
        isScreenshotPressed = true;
    }

    public void PressUp()
    {
        if (isScreenshotPressed)
        {
            //isScreenshotPressed was previously true, we are now taking the screen shot. 
            theDisplay.SetActive(false);
            //Take screenshot
            TakeScreenshot();
            isScreenshotPressed = false;
        }
    }

    void TakeScreenshot()
    {
        string fileName = GetScreenshotFilename();
        string fileExtension = ".png";
        string combinedFileName = fileName + fileExtension;
        string customPath = GetScreenshotDirectory(); //Load custom path here
        string directoryPath = "";
        if (customPath == "")
        {
            directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);

        }
        else
            directoryPath = customPath;
        directoryPath = System.IO.Path.Combine(directoryPath, "DivineDisaster_Screenshots");
        string pngPath = System.IO.Path.Combine(directoryPath, combinedFileName);

        //Check if directory exists
        if (!System.IO.Directory.Exists(directoryPath))
        {
            //Failed to find directory, lets create it
            System.IO.Directory.CreateDirectory(directoryPath);
        }
        while (System.IO.File.Exists(pngPath))
        {
            //File already exists, create new index at end
            combinedFileName = fileName + "_" + nIndex.ToString() + fileExtension;
            pngPath = System.IO.Path.Combine(directoryPath, combinedFileName);
            nIndex++;
        }

        Application.CaptureScreenshot(pngPath);
        TextEditor te = new TextEditor();
        te.text = pngPath;
        te.SelectAll();
        te.Copy();

        StartCoroutine(DisplayScreenshotPostText());
        print("Create GUI Canvas... Screenshot saved to : " + pngPath);
    }

    IEnumerator DisplayScreenshotPostText()
    {
        //Screenshot being taken, wait a little bit to post message, so the message isn't in the screenshot
        yield return new WaitForSeconds(0.5f);
        theDisplay.SetActive(true);
        readyTextGO.SetActive(false);
        successTextGO.SetActive(true);
        theDisplay.SendMessage("DisableAfterFade", true);
        theDisplay.SendMessage("StartFade");

    }

    string GetScreenshotFilename()
    {
        //Create interface for user to set file name preference
        return "Test_Capture";
    }

    string GetScreenshotDirectory()
    {
        //Create interface for user to set directory path 
        return "";
    }
}
