/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;

public class DefaultTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES
 
    private TrackableBehaviour mTrackableBehaviour;
    

    private GameObject options;
    private int firstTime = 0;
    public static bool anotherAnimation = false;

    #endregion 

    #region UNTIY_MONOBEHAVIOUR_METHODS
        
    void Awake ()
    {
        options = GameObject.Find("Options");
    }

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
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
        }
        else
        {
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
		
        if (firstTime  < 2 ) {
            firstTime++;
        } 
        else 
        {
            
            if (!anotherAnimation) 
            {
                SelectOption();
            }
                
        }
    }

    private void SelectOption ()
    {
        int selectedOption = Random.Range(0, options.transform.childCount);
        options.transform.GetChild(selectedOption).gameObject.SetActive(true);
    }

    #endregion 
}
