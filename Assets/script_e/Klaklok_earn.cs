using UnityEngine;
using System.Collections;
using System;
public class Klaklok_earn : MonoBehaviour
{
		public Texture text_tiger;
		public Texture text_chicken;
		public Texture text_crab;
		public Texture text_fish;
		public Texture text_gourd;
		public Texture text_prawn;
	
		public GameObject cube_shack_1;
		public GameObject cube_shack_2;
		public GameObject cube_shack_3;
		public GameObject cube_cover;
	
		private bool play = false;
		private int[] arr_result = new int[]{0,0,0};
		private float[] arr_history_put = new float[]{0,0,0,0,0,0};
	
		private int tmp_money = 0;
	
		public GUISkin Skin;
	
		private int num_money = 5;
		private int level = 0;
	
		private bool Is_info = false;
		private bool Is_music = true;
		bool Is_over = false;
		float camSize;
		int guiVerticleRight;
		public event Action showAds;

		static Klaklok_earn instance;
		public static Klaklok_earn Instance {
				get {
						if (instance == null) {
								instance = FindObjectOfType (typeof(Klaklok_earn)) as Klaklok_earn;
								if (instance == null)
										throw new InvalidOperationException ("No instance in scene!");
						}
						return instance;
				}
		}

		void Start ()
		{
				camSize = Camera.main.orthographicSize;
		}
		void OnGUI ()
		{
				AutoResize ();	
				guiVerticleRight = -1;
				GUI.skin = Skin;
				GUI.Label (new Rect (5, 5, 200, 70), "<size=50><color=wite>$</color> : <color=green><b>" + (num_money - tmp_money) + "</b></color></size>");
				if (Is_over) {
						GUI.Window (0, new Rect (Screen.width / 2 - 150, 50, 300, 200),
			            DoMyWindowOver, "Your money have gone!", GUI.skin.GetStyle ("window"));
						return;
				}
				if (play) {
						int margin_left = 185;
						int margin_top = 250;			
						if (GUI.Button (new Rect (5, 95, 70, 25), "Stop"))
								calculateMoney ();
						AutoPosition ();
						bool b = (num_money - tmp_money) > 0 ? true : false;
						for (int i=0; i<6; i++) {
								arr_history_put [i] = (int)GUI.HorizontalSlider (new Rect (margin_left + i * 80, margin_top, 70, 50), arr_history_put [i], 0f, (float)(num_money - tmp_money) + arr_history_put [i]);
						}
						Texture[] aTexture = {
				text_tiger,
				text_chicken,
				text_crab,
				text_fish,
				text_gourd,
				text_prawn
			};
						margin_top -= 100;
						for (int i=0; i<6; i++) {
								GUI.DrawTexture (new Rect (margin_left + i * 80, margin_top, 70, 70), aTexture [i], ScaleMode.ScaleToFit, true, 0.0f);
								GUI.Label (new Rect (margin_left + i * 80, margin_top, 70, 70), "<size=25><b>" + (int)arr_history_put [i] + "</b></size>");
						}
						tmp_money = Total_money ();
						AutoResize ();
				} else {
						AutoPosition ();			
						Texture[] aTexture = {
				text_tiger,
				text_chicken,
				text_crab,
				text_fish,
				text_gourd,
				text_prawn
			};

						int margin_left = 185;
						int margin_top = 150;
						for (int i=0; i<6; i++) {
								GUI.DrawTexture (new Rect (margin_left + i * 80, margin_top, 70, 70), aTexture [i], ScaleMode.ScaleToFit, true, 0.0f);
								GUI.Label (new Rect (margin_left + i * 80, margin_top, 70, 70), "<size=25><b>" + (int)arr_history_put [i] + "</b></size>");
						}
						AutoResize ();
						if (GUI.Button (new Rect (5, 95, 70, 25), "Play") && num_money > 0)
								onPlay ();
				}
		
//				if (GUI.Button (new Rect (5, 155, 70, 25), "Buy level")) {
//						if (num_money >= 15) {
//								num_money -= 15;
//								level++;
//						}
//				}
//				if (GUI.Button (new Rect (5, 185, 70, 25), "Sell level")) {
//						if (level > 0) {
//								level--;
//								num_money += 10;
//						}
//				}
				/*if (GUI.Button (new Rect (5, 185, 70, 25), Is_music ? "No music" : "Music")) {
						Is_music = true;
						if (Is_music)
								gameObject.audio.Play ();
						else
								gameObject.audio.Stop ();
				}
				if (GUI.Button (new Rect (5, 215, 70, 25), "Info"))
						Is_info = true;
				if (Is_info) {
						if (GUI.Button (new Rect (0, 0, Screen.width, 25), "Back"))
								Is_info = false;
						GUI.TextArea (new Rect (0, 30, Screen.width, Screen.height), "I'm <color=red>Eng Raksa</color>. I'm founder and group leader. Now all members try all their " +
								"best to generate ideas to make people fun. <b>eKlaklok</b> is our game for. " +
								"<color=green>www.k4us.net</color>");
				}
*/
				if (Camera.main.orthographicSize < camSize * 2f && GUI.Button (new Rect (Screen.width - 75, (++guiVerticleRight * 50) + 5, 70, 25), "Zoom -"))
						zoomCamera (true);
				if (Camera.main.orthographicSize > camSize + 0.1f && GUI.Button (new Rect (Screen.width - 75, (++guiVerticleRight * 50) + 5, 70, 25), "Zoom +"))
						zoomCamera (false);
				++guiVerticleRight;
				if (GUI.Button (new Rect (Screen.width - 75, (++guiVerticleRight * 40) + 5, 70, 25), "Quit")) {
//						if (showAds != null)
//								showAds ();
						Application.Quit ();
				}
		
		}
		void DoMyWindowOver (int windowID)
		{
				GUILayout.BeginVertical ();
				GUILayout.Space (8);
				if (GUILayout.Button ("Play again")) {
						Is_over = false;
						Init ();
				}
				if (GUILayout.Button ("Quit"))
						Application.Quit ();
				GUILayout.EndVertical ();
				GUI.DragWindow ();
		}
		public void AutoResize ()
		{
				float screenHeight = 380f;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (1f, Screen.height / screenHeight, 1f));
		}
		public void AutoPosition ()
		{
				float originalWidth = 850f;
				float originalHeight = 400f;
				GUI.matrix = Matrix4x4.TRS (new Vector3 (0f, 1f, 0f), Quaternion.identity, new Vector3 (1f, 1f, 1f));
				Vector3 scale = new Vector3 ();
				scale.y = Screen.height / originalHeight;
				scale.x = scale.y;
				scale.z = 1f;
				float scaleX = Screen.width / originalWidth;
				Matrix4x4 svMat = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS (new Vector3 ((scaleX - Screen.height / originalHeight) / 2 * originalWidth, 0, 0), Quaternion.identity, scale);
		}
		void onPlay ()
		{
				cube_cover.transform.position = new Vector3 (cube_cover.transform.position.x, cube_cover.transform.position.y, 7);
				;
				cube_shack_1.animation.Play ();
				cube_shack_2.animation.Play ();
				cube_shack_3.animation.Play ();
				play = true;
		}
		void calculateMoney ()
		{
				Stop_pos ();
				num_money -= tmp_money;
				for (int i=0; i<3; i++) {

						int tmp = (int)arr_history_put [arr_result [i]] * 2;
						if (tmp != 0)
								ParticleSystem_earn.Instance.incMoney (Camera.main.ScreenToWorldPoint (new Vector3 (111, Screen.height - 15, Camera.main.nearClipPlane)));
						num_money += tmp;
				}
				arr_history_put = new float[]{0,0,0,0,0,0};
				play = false;
				if (num_money == 0) {
						if (showAds != null)
								showAds ();
						Invoke ("setIsOver", 2);
				}
		}
		private void setIsOver ()
		{
				Is_over = true;
		}
	
		int Total_money ()
		{
				return (int)arr_history_put [0] + (int)arr_history_put [1] + (int)arr_history_put [2] + (int)arr_history_put [3] + (int)arr_history_put [4] + (int)arr_history_put [5];
		}
	
		void Stop_pos ()
		{	
				cube_cover.transform.position = new Vector3 (cube_cover.transform.position.x, cube_cover.transform.position.y, 20);
				GameObject[] arr_cubes = new GameObject[] {
			cube_shack_1,
			cube_shack_2,
			cube_shack_3
		};
				int[,] arr_co_rotate = new int[,] {
			{0,0,0},
			{0,90,180},
			{270,0,0},
			{90,0,0},
			 {
				180,
				0,
				0
			},
			 {
				180,
				90,
				0
			}
		};
				int ind = 0;
				for (int i=0; i<3; i++) {
						ParticleSystem_earn.Instance.onCube (new Vector3 (arr_cubes [i].transform.position.x, arr_cubes [i].transform.position.y, 8f));
						arr_cubes [i].animation.Stop ();
						ind = rand_six ();
						arr_result [i] = ind;
						arr_cubes [i].transform.rotation = Quaternion.identity;
						arr_cubes [i].transform.Rotate (new Vector3 (arr_co_rotate [ind, 0], arr_co_rotate [ind, 1], arr_co_rotate [ind, 2]));
				}
		}
		int rand_six ()
		{
				int tmp = UnityEngine.Random.Range (0, 6);
				int l = (int)System.DateTime.Now.Second / 6;
				for (int i=0; i<l; i++) {
						tmp = UnityEngine.Random.Range (0, 6);
				}
				return tmp;
		}
		void Init ()
		{
				tmp_money = 0;
				num_money = 5;
				level = 0;
				Is_over = false;
		}
		void zoomCamera (bool zi)
		{
				Camera.main.orthographicSize *= zi ? 1.05f : 0.95f;
		}
}
