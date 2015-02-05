/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
// using UIImage = UnityEngine.UI.Image;

public class CameraEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES
 
    private TrackableBehaviour mTrackableBehaviour;
    private int firstTime = 0;
    private bool takingPhoto = false;
    private int counter = 0;
    private int COUNTERLIMIT = 3;
    // private UIImage counterImage;
    // public  Sprite [] counterSprites;
    private GUIObject[] counterGUI= new GUIObject[3];
    private int photoCounter = 0;
    #endregion 

    #region UNTIY_MONOBEHAVIOUR_METHODS
        
    void Awake ()
    {
        // counterImage = GameObject.Find("Counter").gameObject.
            // GetComponent<UIImage>();
        // InvokeRepeating("ShowCounter", 1.0f, 1.0f);

        for (int index= 0; index < counterGUI.Length; index++) {
            counterGUI[index] = new GUIObject(
                Resources.Load("GUI/" + (index + 1), 
                typeof(Texture2D)) as Texture2D,
                0.5f, 0.5f, 0.2f, 1.2f, 1.0f, 1.0f, false, false, false, true, 
                true);
            counterGUI[index].Render(false);
            // counterGUI[index].xpos = 0.5f;
            // counterGUI[index].ypos = 0.5f;
            // counterGUI[index].CalculatePosition();
        }

        for (int index= 0; index < counterGUI.Length; index++) {
            // counterGUI[index].xpos = 0.5f;
            // counterGUI[index].ypos = 0.5f;
            // counterGUI[index].CalculatePosition();
        }

    }

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    void OnEnable() 
    {

    }

    void OnDisable ()
    {

    }

    #endregion 

    #region PUBLIC_METHODS

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        } else {
            OnTrackingLost();
        }
    }

    #endregion 



    #region PRIVATE_METHODS


    private void OnTrackingFound()
    {

    }


    private void OnTrackingLost()
    {
        
        if (firstTime  < 2 ) 
        {
            firstTime++;
        }
        else
        {   
            if (!takingPhoto) 
            {   
                takingPhoto = true;
                // counterImage.enabled = true;
                counter = 0;
                counterGUI[counter].Render(true);
                InvokeRepeating("ShowCounter", 1.0f, 1.0f);

            }
               
            Debug.Log("OnTrackingLost => Camera");
        }
    }

    public void ShowCounter()
    {   
        counter++;
        if (counter < COUNTERLIMIT) 
        {
            // counterImage.sprite = counterSprites[counter];
            counterGUI[counter - 1].Render(false);
            counterGUI[counter].Render(true);

        }
        else
        {   
            counterGUI[2].Render(false);
            // counterImage.enabled = false;
            CancelInvoke();
            TakePhoto();
        }
    }

    public void TakePhoto ()
    {   
        // counterImage.sprite = counterSprites[0];
        Application.CaptureScreenshot("Photo_" + photoCounter + ".png", 4);
        photoCounter++;
        takingPhoto = false;

    }

    #endregion 
}
