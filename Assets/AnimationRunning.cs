using UnityEngine;
using System.Collections;

public class AnimationRunning : MonoBehaviour {
	
    void OnEnable() 
    {
        StartCoroutine(WaitForAnimation( animation ));
    }

    void OnDisable ()
    {
        DefaultTrackableEventHandler.anotherAnimation = false;
    }

    private IEnumerator WaitForAnimation ( Animation animation )
    {
        do
        {
            yield return null;
        
        } while ( animation.isPlaying );

        gameObject.SetActive(false);
    }
}
