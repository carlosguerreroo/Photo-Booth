using UnityEngine;
using System.Collections;

[System.Serializable]
public class GUIObject {
	public Texture2D texture;					//Texture to display
	public float xpos;						//X position
	public float ypos;						//Y position
	public float size;						//Size relative to screen size (x or y depending on relativeToY float) 
	public float zLayer;						//Texture layer ()
	public float widthPropotion;				//Texture width acording with screen size
	public float heightPropotion;				//Texture height acording with screen size
	public bool relativeToY;				//Size is now relative to X axis
	public bool xInverted;					//Is x position inverted?
	public bool yInverted;					//Is y position inverted?
	public bool centerX;					//Center the image in x
	public bool centerY;					//Center the image in y
	public iTween.EaseType animation;
	public float alphaAnimated;						//Alpha of the object
	private GameObject targetGameObject;		//The game object that holds the texture
	private bool enabled;					//display the image?
	private float width;						//Local texture width
	private float height;						//Local texture height
	private bool rendering;					//Indicates is the Object is being rendered
		
	public GUIObject (Texture2D texture, float xpos, float ypos, float size, float zLayer, 
	                  float widthPropotion = 1f, float heightPropotion = 1f, 
	                  bool relativeToY = false, bool xInverted = false, bool yInverted = false, 
	                  bool centerX = false, bool centerY = false, 
	                  iTween.EaseType animation = iTween.EaseType.easeOutBounce) {
		this.texture = texture;
		this.widthPropotion = widthPropotion;
		this.heightPropotion = heightPropotion;
		this.size = size;
		this.xpos = xpos;
		this.ypos = ypos + (((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion * .5f);
		this.ypos = ypos;
		this.zLayer = zLayer;
		this.xInverted = xInverted;
		this.yInverted = yInverted;
		this.relativeToY = relativeToY;
		this.centerX = centerX;
		this.centerY = centerY;
		this.animation = animation;
		this.alphaAnimated = -1;

		this.enabled = false;
		Enable();
	}
	
	//This creates the target gameObject when GUIObject was not created in the inspector
	public void Enable () {
		if (!enabled) {
			this.targetGameObject = new GameObject(this.texture.name);
			this.targetGameObject.layer = 17;
			this.targetGameObject.transform.parent = GameObject.FindGameObjectWithTag("GUIHolder").transform;

			GUITexture guiTexture = this.targetGameObject.AddComponent<GUITexture>();
			guiTexture.texture = this.texture;

			CalculatePosition();
			enabled = true;
		}
	}
	
	////////////////////////
	//Position calculation//
	////////////////////////
	////////
	////////
	////////
	////////
	////////
	////////
	////////////////
	///////////
	///////
	////
	
	
	//Reclalulate the texture position
	public void CalculatePosition () {
		if (xInverted) {
			if (centerX) {
				if (relativeToY) {
					//xInverted & centerX & relativeToY
					width = 1 - this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//xInverted & CenterX
					width = 1 - this.xpos;
				}
			} else {
				if (relativeToY) {
					//xInverted & relativeToY
					width = 1 - this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//xInverted
					width = 1 - this.xpos + ((this.size * this.widthPropotion)/2);
				}
			}
		} else {
			if (centerX) {
				if (relativeToY) {
					//CenterX & relativeToY
					width = this.xpos;
				} else {
					//CenterX
					width = this.xpos;
				}
			} else {
				if (relativeToY) {
					//relativeToY
					width = this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//None
					width = this.xpos + ((this.size * this.widthPropotion)/2);
				}
			}
		}
		if (yInverted) {
			if (centerY) {
				if (relativeToY) {
					//yInverted & centerY & relativeToY
					height = 1 - this.ypos;
				} else {
					//yInverted & CenterY
					height = 1 - this.ypos;
				}
			} else {
				if (relativeToY) {
					//yInverted & relativeToY
					height = 1 - this.ypos - (this.size * this.heightPropotion) * .5f;
				} else {
					//yInverted
					height = 1 - (this.ypos + (((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion * .5f));
				}
			}
		} else {
			if (centerY) {
				if (relativeToY) {
					//CenterY & relativeToY
					height = this.ypos;
					//==================================height = 1 - this.ypos;
				} else {
					//CenterY
					height = this.ypos;
				}
			} else {
				if (relativeToY) {
					//relativeToY
					height = this.ypos - (this.size * this.heightPropotion) * .5f;
					//-------------------------------height = this.ypos - .5f;
				} else {
					//None
					height = this.ypos - ((((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion)/2);
				}
			}
		}
		
		/////////////////////
		//Scale calculation//
		/////////////////////
		
		Vector3 localScale = this.targetGameObject.transform.localScale;
		Vector3 position = this.targetGameObject.transform.position;

		if (relativeToY) {
			localScale.x = ((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion;
			localScale.y = this.size * this.heightPropotion;
		} else {
			localScale.x = this.size * this.widthPropotion;
			localScale.y = ((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion;
			// Debug.Log("Texture size X = >" + localScale.x );
			// Debug.Log("Texture size Y = >" + localScale.y );
			// Debug.Log("Texture width =>" + texture.width);
			// Debug.Log("Texture height =>" + texture.height);

		}

		position.x = width;
		position.y = height;
		position.z = zLayer;

		this.targetGameObject.transform.position = position;
		this.targetGameObject.transform.localScale = localScale;
		
	}
	
	public void TransformObject (float time, float delay) {
		//Recalculate positio (width ad height 	//Recalculate position (width and height variables) before animationiables) before aimatio
		if (xInverted) {
			if (centerX) {
				if (relativeToY) {
					//xInverted & centerX & relativeToY
					width = 1 - this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//xInverted & CenterX
					width = 1 - this.xpos;
				}
			} else {
				if (relativeToY) {
					//xInverted & relativeToY
					width = 1 - this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//xInverted
					width = 1 - this.xpos + ((this.size * this.widthPropotion)/2);
				}
			}
		} else {
			if (centerX) {
				if (relativeToY) {
					//CenterX & relativeToY
					width = this.xpos;
				} else {
					//CenterX
					width = this.xpos;
				}
			} else {
				if (relativeToY) {
					//relativeToY
					width = this.xpos + ((((((Screen.height * this.size)/this.texture.height) * this.texture.width)/Screen.width) * this.widthPropotion)/2);
				} else {
					//None
					width = this.xpos + ((this.size * this.widthPropotion)/2);
				}
			}
		}
		if (yInverted) {
			if (centerY) {
				if (relativeToY) {
					//yInverted & centerY & relativeToY
					height = 1 - this.ypos;
				} else {
					//yInverted & CenterY
					height = 1 - this.ypos;
				}
			} else {
				if (relativeToY) {
					//yInverted & relativeToY
					height = 1 - this.ypos - (this.size * this.heightPropotion) * .5f;
				} else {
					//yInverted
					height = 1 - (this.ypos + (((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion * .5f));
				}
			}
		} else {
			if (centerY) {
				if (relativeToY) {
					//CenterY & relativeToY
					height = this.ypos;
					//==================================height = 1 - this.ypos;
				} else {
					//CenterY
					height = this.ypos;
				}
			} else {
				if (relativeToY) {
					//relativeToY
					height = this.ypos - (this.size * this.heightPropotion) * .5f;
					//-------------------------------height = this.ypos - .5f;
				} else {
					//None
					height = this.ypos - ((((((Screen.width * this.size)/this.texture.width) * this.texture.height)/Screen.height) * this.heightPropotion)/2);
				}
			}
		}
		
		//Once width and height are correct is time to move that ass baby (the animation)
		iTween.Stop(targetGameObject);
		//Changes iTween animation
		if (size > 0) {
			if(this.alphaAnimated > 0){
				iTween.FadeTo(this.targetGameObject, new Hashtable () {{"alpha", this.alphaAnimated}, {"time", time}, {"delay", delay}});
				iTween.MoveTo (this.targetGameObject, new Hashtable () {{"position", new Vector3(width, height, zLayer)}, {"time", time}, {"delay", delay}, {"easeType", animation}});
				iTween.ScaleTo (this.targetGameObject, new Hashtable () {{"scale", new Vector3(widthPropotion * size, heightPropotion * size, zLayer)}, {"time", time}, {"delay", delay}});

			}else{
				iTween.MoveTo (this.targetGameObject, new Hashtable () {{"position", new Vector3(width, height, zLayer)}, {"time", time}, {"delay", delay}, {"easeType", animation}});
				iTween.ScaleTo (this.targetGameObject, new Hashtable () {{"scale", new Vector3(widthPropotion * size, heightPropotion * size, size)}, {"time", time}, {"delay", delay}});
			}
		} else {
			if(this.alphaAnimated > 0){
				iTween.FadeTo(this.targetGameObject, new Hashtable () {{"alpha", this.alphaAnimated}, {"time", time}, {"delay", delay}});
				iTween.MoveTo (this.targetGameObject, new Hashtable () {{"position", new Vector3(width, height, zLayer)}, {"time", time}, {"delay", delay}, {"easeType", animation}});
			}else{
				iTween.MoveTo (this.targetGameObject, new Hashtable () {{"position", new Vector3(width, height, zLayer)}, {"time", time}, {"delay", delay}, {"easeType", animation}});
			}
		}
	}

	public void TransformObject (float size){
		this.size = size;
		TransformObject(1f, 0f);
	}

	public void TransformObject (float time, float delay, float x, float y, float size, float alpha) {
		
		this.xpos += x;
		this.ypos += y;
		this.size = size;
		CalculatePosition ();
		this.alphaAnimated = alpha;
		TransformObject(time, delay);
	}

	public void TransformObject (float time, float delay, float x, float y, float size) {
		
		this.xpos += x;
		this.ypos += y;
		this.size = size;
		CalculatePosition ();
		TransformObject(time, delay);
	}

	public void TransformObject (float time, float delay, float x, float y) {
		
		this.xpos += x;
		this.ypos += y;
		
		TransformObject(time, delay);
	}

	public void TransformObject () {
		TransformObject(1f, 0f);
	}

	public void SetAlphaAnimated(float alphaValue){
		alphaAnimated = alphaValue;
	}

	//Set texture alpha
	public void SetAlpha (float alphaValue) {
		Color color = targetGameObject.guiTexture.color; 
		color.a = alphaValue;
		targetGameObject.guiTexture.color = color;
	}
	
	//Toggle rendering status
	public void Render (bool render) {
		targetGameObject.SetActive(render);
		rendering = render;
	}

	public bool isRendering (){
		return rendering;
	}
	
	//Return target game object
	public GameObject GetTarget () {
		return this.targetGameObject;
	}

	//Destroy targetGameObject
	public void DeleteTarget () {
		//GameObject destroy itself
		Object.Destroy(this.targetGameObject);
	}
	
	public float GetWidth () {
		return this.targetGameObject.transform.localScale.x;
	}
	
	public float GetHeight () {
		return this.targetGameObject.transform.localScale.y;
	}

	public float GetZLayer(){
		return this.zLayer;
	}

	public void ReplaceTexture (Texture2D texture) {
		this.targetGameObject.GetComponent<GUITexture>().texture = this.texture = texture;
	}
}