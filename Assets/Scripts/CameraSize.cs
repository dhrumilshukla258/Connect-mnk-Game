using UnityEngine;

namespace ConnectFour
{
	public class CameraSize : MonoBehaviour 
	{
		Camera cam;
		
		void Awake () 
		{
			cam = GetComponent<Camera>();
			cam.orthographic = true;
		}
		
		void LateUpdate()
		{
            float maxX = GameBoard.numColumns;
			float maxY = GameBoard.numRows + 3;

			cam.orthographicSize = maxY > maxX ? maxY / 2f : maxX / 2f ;
		}
	}
}
