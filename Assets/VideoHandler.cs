using UnityEngine;
using System.Collections;

public class VideoHandler : MonoBehaviour {

    private GUIObject introVideo;
    private int introVideoLimit = 305;
    private menuButton introVideoButton;
    private Texture2D[] textureIntroVideo = new Texture2D[305];
	private int whichFrame = 0;
    public string activatedVideo = "";
    public string pathFiles = "";

    void Awake () 
    {
        introVideo = new GUIObject (Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + pathFiles + "_00000"),
                                                             0.5F, 0.4F, 0.8F, 1.1F, 1F, 1F, true, false, false, true, true);
        introVideo.Render (false);

        LoadTextures (activatedVideo);
	}

    void OnEnable() 
    {
        whichFrame = 0;
        introVideo.Render (true);
    }

    void OnDisable ()
    {
        DefaultTrackableEventHandler.anotherAnimation = false;
        introVideo.Render (false);
    }
	
	void LateUpdate () {
	
    
        string numberOfFrame = whichFrame.ToString ();
      
        if (whichFrame >= introVideoLimit) 
        {
            gameObject.SetActive(false);
        }
        else 
        {
            introVideo.ReplaceTexture (textureIntroVideo [whichFrame]);
        }
        
        whichFrame++;  
	}

    public void LoadTextures (string whichVideo)
    {
        string numberOfFrame = "";
        textureIntroVideo = new Texture2D[305];

        for (int index = 0; index < introVideoLimit; index ++) 
        {    
            numberOfFrame = index.ToString ();
            if (numberOfFrame.Length == 1) {
                    textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + pathFiles + "_00000" + numberOfFrame);
            } else if (numberOfFrame.Length == 2) {
                    textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + pathFiles + "_000" + numberOfFrame);
            } else if (numberOfFrame.Length == 3) {
                    textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + pathFiles + "_00" + numberOfFrame);
            }
        }
           
    }
}
