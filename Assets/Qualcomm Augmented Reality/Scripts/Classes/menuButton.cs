using UnityEngine;


public class menuButton 
{

    public GUIObject button;
    public GUITexture buttonTexture;
    public Texture2D normal;
    public Texture2D highlighted;
	public string name;

	/// <summary>
	///constructor used for inherited classes
	/// </summary>
	public menuButton ()
	{
	}

	/// <summary>
	/// Constructor for a menuButton initializing all it's elements
	/// </summary>
	public menuButton (string name, float x, float y, float size, string path, float layer = 0F, iTween.EaseType animation = iTween.EaseType.easeOutBounce) 
    {
		this.name = name;
        this.normal = Resources.Load<Texture2D>(path + "/Normal" + name + "ButtonGUI");
        this.highlighted = Resources.Load<Texture2D>(path + "/Highlighted" + name + "ButtonGUI");
        this.button = new GUIObject (this.normal, x, y, size, 1.1F + layer, 1F, 1F, true, false, false, true, true, animation);
        this.buttonTexture = button.GetTarget().GetComponent<GUITexture>();
    }

    public void OnClickUp () 
    {
        this.button.ReplaceTexture(normal);
    }

    public void OnClickDown () 
    {
        this.button.ReplaceTexture(highlighted);
    }
}
