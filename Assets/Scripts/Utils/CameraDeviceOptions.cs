using UnityEngine;
using System.Collections;
using System;

public class CameraDeviceOptions : MonoBehaviour
{
	#region CONST_MEMBER_VARIABLES
		private const float TIMEBETWEENINTERVALS = 0.002604166667F;

	#endregion


	#region PUBLIC_MEMBER_VARIABLES
	
		public string whichModel = "";
		public int whichAudio = 0;
		public bool tracking = false;
	
	#endregion 

	#region PRIVATE_MEMBER_VARIABLES


		/*********************Videos********************/
		private GUIObject introVideo;
		int introVideoLimit = 134;
		private menuButton introVideoButton;
		Texture2D[] textureIntroVideo = new Texture2D[135];
		private GUIObject singingVideo;
		int singingVideoLimit = 575;
		private menuButton singingVideoButton;
		Texture2D[] textureSingingVideoFirst = new Texture2D[192];
		Texture2D[] textureSingingVideoSecond = new Texture2D[192];
		Texture2D[] textureSingingVideoThird = new Texture2D[192];
		private GUIObject selfieVideo;
		int selfieVideoLimit = 272;
		private menuButton selfieVideoButton;
		Texture2D[] textureSelfieVideo = new Texture2D[273];


		///Helper videos
		private int whichFrame = 0;
		private float timer = TIMEBETWEENINTERVALS;
		private string activatedVideo = "";
		private float delayedFirstAudio = 0.04166666667F;
		private bool firstTime = true;
		/**********************************************/

		// Checks if a button has been pressed.
		private bool mButtonPressed = false;

		// Checks if flash is currently enabled.
		private bool mFlashEnabled = false;
	
		// Contains the currently set auto focus mode.
		private CameraDevice.FocusMode mFocusMode =
		CameraDevice.FocusMode.FOCUS_MODE_NORMAL;
	
		// Contains the rectangle for the camera options menu.
		private Rect mAreaRect;

		//Button to return to the previous scene
		private menuButton back;

		//Button to manage the flash
		private menuButton cameraFlash;

		//Variables to take a screenshot
		private GUIObject takePic;
		private GameObject guiHolder;
		private bool takingPicture = false;

	#endregion // PRIVATE_MEMBER_VARIABLES
	

	
	#region UNTIY_MONOBEHAVIOUR_METHODS

		public void Awake ()
		{
				
				guiHolder = GameObject.Find ("GUIHolder");

		}
	
		void OnEnable ()
		{
				
		}
    
		
		void OnDisable ()
		{

		}

		public void Start ()
		{
				// Setup position and size of the camera menu.
				computePosition ();

				//Initialize all the elements to display
				InitializeGUIObjects ();
		}

	
		public void OnApplicationPause (bool pause)
		{
				// Upon resuming reactivate the torch if it was active before pausing:
				if (!pause) {
						if (mFlashEnabled) {
								mFlashEnabled = CameraDevice.Instance.SetFlashTorchMode (true);
						}
				}
		}

		public void Update ()
		{

				if (delayedFirstAudio > 0) {
						delayedFirstAudio -= Time.deltaTime;
				} else if (firstTime) {
						firstTime = false;
						singingVideo.Render (false);
						GameObject.Find (activatedVideo).audio.Play ();
				}

				if (timer > 0) {
						timer -= Time.deltaTime;

				} else {
						string numberOfFrame = whichFrame.ToString ();
						string sequence = "";
						if (activatedVideo == "Intro") {
								introVideo.Render (true);
								selfieVideo.Render (false);
								singingVideo.Render (false);
								if (whichFrame > introVideoLimit) {
										activatedVideo = "";
								} else {
										introVideo.ReplaceTexture (textureIntroVideo [whichFrame]);
								}

						} else if (activatedVideo == "Selfie") {
								introVideo.Render (false);
								selfieVideo.Render (true);
								singingVideo.Render (false);
								if (whichFrame > selfieVideoLimit) {
										activatedVideo = "";
								} else {
										selfieVideo.ReplaceTexture (textureSelfieVideo [whichFrame]);
								}

						} else if (activatedVideo == "Singing") {
								introVideo.Render (false);
								selfieVideo.Render (false);
								singingVideo.Render (true);
								if (whichFrame > singingVideoLimit) {
										activatedVideo = "";
								} else {

										if (whichFrame < 192) {
												singingVideo.ReplaceTexture (textureSingingVideoFirst [whichFrame]);
										} else if (whichFrame < 384) {
												textureSingingVideoFirst = new Texture2D[1];
												singingVideo.ReplaceTexture (textureSingingVideoSecond [whichFrame - 192]);
										} else {
												textureSingingVideoSecond = new Texture2D[1];
												singingVideo.ReplaceTexture (textureSingingVideoThird [whichFrame - 384]);
										}

								}

						}

						whichFrame++;
						timer = TIMEBETWEENINTERVALS;
				}

				if (activatedVideo != "" && (!GameObject.Find (activatedVideo).audio.isPlaying) && takingPicture) {
						GameObject.Find (activatedVideo).audio.Play ();
						takingPicture = false;
				}

				#region ANDROID_PLATFORM
				#if UNITY_ANDROID
				if (Input.GetKey (KeyCode.Escape))
				{
			Application.Quit ();
			return;
				}
				#endif
				#endregion // ANDROID_PLATFORM

				if (tracking) {
						introVideo.Render (false);
						selfieVideo.Render (false);
						singingVideo.Render (false);
						if (activatedVideo != "") {
								GameObject.Find (activatedVideo).audio.Stop ();
				
						}
				}
		
				if (tracking && !GameObject.Find ("SingAlong").audio.isPlaying) {
						if (activatedVideo != "") {
								GameObject.Find (activatedVideo).audio.Pause ();
				
						}
						activatedVideo = "";
						GameObject.Find ("SingAlong").audio.Play ();
				} else if (!tracking && GameObject.Find ("SingAlong").audio.isPlaying) {
						GameObject.Find ("SingAlong").audio.Stop ();
				}

				// If the touch event results from a button press it is ignored.
				if (!mButtonPressed) {
						// If finger is removed from screen.
						if (Input.GetMouseButtonUp (0)) {
								if (back.buttonTexture.HitTest (Input.mousePosition)) {
										Application.Quit ();
										return;				
								} else if (cameraFlash.buttonTexture.HitTest (Input.mousePosition)) {

										if (!mFlashEnabled) {
												// Turn on flash if it is currently disabled.
												CameraDevice.Instance.SetFlashTorchMode (true);
												mFlashEnabled = true;
										} else {
												// Turn off flash if it is currently enabled.
												CameraDevice.Instance.SetFlashTorchMode (false);
												mFlashEnabled = false;
										}
						
										mButtonPressed = true;
										return;

								} else if (takePic.GetTarget ().GetComponent<GUITexture> ().HitTest (Input.mousePosition)) {
										//guiHolder.SetActive (false);
										if (activatedVideo != "") {
												GameObject.Find (activatedVideo).audio.Pause ();

										}
										StartCoroutine (ScreenshotManager.Save ("JAPlayer", "JAPlayer"));
										StartCoroutine (ActiveShare ());
										takingPicture = true;
										GameObject.Find ("camera-shutter").audio.Play ();
					
								} else if (selfieVideoButton.buttonTexture.HitTest (Input.mousePosition) && !tracking) {
										if (activatedVideo != "") {
												GameObject.Find (activatedVideo).audio.Stop ();	
										}

										activatedVideo = "Selfie";
										LoadTextures (activatedVideo);
										whichFrame = 0;
										GameObject.Find (activatedVideo).audio.Play ();
								} else if (CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO)) {
										mFocusMode = CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO;
								}
								OnClickUpButtons ();
						} else if (Input.GetMouseButtonDown (0)) {
								if (back.buttonTexture.HitTest (Input.mousePosition)) {
										back.OnClickDown ();
										//mButtonPressed = true;
								} else if (cameraFlash.buttonTexture.HitTest (Input.mousePosition)) {
										cameraFlash.OnClickDown ();
								} else if (selfieVideoButton.buttonTexture.HitTest (Input.mousePosition) && !tracking) {
										selfieVideoButton.OnClickDown ();
								}
						}
				} else {
						mButtonPressed = false;
				}
		}

	#endregion 
	#region PRIVATE_METHODS

		private void InitializeGUIObjects ()
		{
				
			back = new menuButton ("BackCamera", 0.08F, 0.95F, 0.08F, "ManyViews");

			cameraFlash = new menuButton ("FlashCamera", 0.9F, 0.95F, 0.08F, "ManyViews");

			
			selfieVideoButton = new menuButton ("Selfie", 0.9F, 0.08F, 0.08F, "MainCamera");

			takePic = new GUIObject (Resources.Load<Texture2D> ("ManyViews/cameraGUI"),
	                         0.1F, 0.06F, 0.08F, 1.1F, 1F, 1F, true, false, false, true, true);

			{

					///////////Videos///////////
					introVideo = new GUIObject (Resources.Load<Texture2D> ("MainCamera/Intro/Secuencia2_00" + whichFrame.ToString ()),
		                                                     0.5F, 0.4F, 0.8F, 1.1F, 1F, 1F, true, false, false, true, true);
					
					singingVideo = new GUIObject (Resources.Load<Texture2D> ("MainCamera/Singing/Secuencia1_00" + whichFrame.ToString ()),
		                            0.5F, 0.35F, 0.7F, 1.1F, 1F, 1F, true, false, false, true, true);
					singingVideo.Render (false);


					selfieVideo = new GUIObject (Resources.Load<Texture2D> ("MainCamera/Selfie/Secuencia3_00" + whichFrame.ToString ()),
		                              0.8F, 0.3F, 0.8F, 1.1F, 1F, 1F, true, false, false, true, true);

					//Initialize correct video
					activatedVideo = "Intro";
					LoadTextures (activatedVideo);

			}
		}
	
		private void OnClickUpButtons ()
		{
				back.OnClickUp ();
				cameraFlash.OnClickUp ();
				selfieVideoButton.OnClickUp ();
		}

		
		/// Compute the coordinates of the menu depending on the current orientation.
		private void computePosition ()
		{
				int areaWidth = Screen.width;
				int areaHeight = (Screen.height / 5) * 2;
				int areaLeft = (int)(Screen.width * 0.9);
				int areaTop = (int)(Screen.height * 0.03);
				mAreaRect = new Rect (areaLeft, areaTop, areaWidth, areaHeight);
		}
	
		

	#endregion

	#region PUBLIC_METHODS

		public void LoadTextures (string whichVideo)
		{
				string numberOfFrame = "";
				string sequence = "";

				textureIntroVideo = new Texture2D[135];
				textureSingingVideoFirst = new Texture2D[192];
				textureSingingVideoSecond = new Texture2D[192];
				textureSingingVideoThird = new Texture2D[192];
				textureSelfieVideo = new Texture2D[273];

				if (whichVideo == "Intro") {
						activatedVideo = "Intro";
						for (int index = 0; index < introVideoLimit; index ++) {
								numberOfFrame = index.ToString ();
								sequence = "Secuencia2";
								if (numberOfFrame.Length == 1) {
										textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_00" + numberOfFrame);
								} else if (numberOfFrame.Length == 2) {
										textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_0" + numberOfFrame);
								} else if (numberOfFrame.Length == 3) {
										textureIntroVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_" + numberOfFrame);
								}
						}
				} else if (whichVideo == "Singing") {
						activatedVideo = "Singing";
						for (int index = 0; index < singingVideoLimit; index ++) {
								numberOfFrame = index.ToString ();
								sequence = "Secuencia1";
								if (index == 170) {
										LoadSecondPart ();
								}
								if (index < 192) {
										if (numberOfFrame.Length == 1) {
												textureSingingVideoFirst [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_00" + numberOfFrame);
										} else if (numberOfFrame.Length == 2) {
												textureSingingVideoFirst [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_0" + numberOfFrame);
										} else if (numberOfFrame.Length == 3) {
												textureSingingVideoFirst [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_" + numberOfFrame);
										}
								}

						}
				} else if (whichVideo == "Selfie") {
						activatedVideo = "Selfie";
						for (int index = 0; index < selfieVideoLimit; index ++) {
								numberOfFrame = index.ToString ();
								sequence = "Secuencia3";
								if (numberOfFrame.Length == 1) {
										textureSelfieVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_00" + numberOfFrame);
								} else if (numberOfFrame.Length == 2) {
										textureSelfieVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_0" + numberOfFrame);
								} else if (numberOfFrame.Length == 3) {
										textureSelfieVideo [index] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_" + numberOfFrame);
								}
						}
				}

		}

		public void LoadSecondPart ()
		{
				for (int index = 192; index < (singingVideoLimit - 192); index ++) {

						if (index == 360) {
								LoadThirdPart ();
						}
						string numberOfFrame = index.ToString ();
						string sequence = "Secuencia1";
						if (numberOfFrame.Length == 1) {
								textureSingingVideoSecond [index - 192] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_00" + numberOfFrame);
						} else if (numberOfFrame.Length == 2) {
								textureSingingVideoSecond [index - 192] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_0" + numberOfFrame);
						} else if (numberOfFrame.Length == 3) {
								textureSingingVideoSecond [index - 192] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_" + numberOfFrame);
						}
				}
		}

		public void LoadThirdPart ()
		{
				for (int index = 384; index < singingVideoLimit; index ++) {

						string numberOfFrame = index.ToString ();
						string sequence = "Secuencia1";
						if (numberOfFrame.Length == 1) {
								textureSingingVideoThird [index - 384] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_00" + numberOfFrame);
						} else if (numberOfFrame.Length == 2) {
								textureSingingVideoThird [index - 384] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_0" + numberOfFrame);
						} else if (numberOfFrame.Length == 3) {
								textureSingingVideoThird [index - 384] = Resources.Load<Texture2D> ("MainCamera/" + activatedVideo + "/" + sequence + "_" + numberOfFrame);
						}
				}
		}
		
		public IEnumerator ActiveShare ()
		{
				yield return new WaitForEndOfFrame ();
				guiHolder.SetActive (true);
		}
		
	#endregion 
}
