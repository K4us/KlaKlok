using UnityEditor;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
	

[CustomEditor(typeof(CubeAtlasUVManager))]	
class CubeAtlasUVEditor : Editor {
	Dictionary<string,UVPosition> UVPositions=new Dictionary<string,UVPosition>();
	private bool debugMode=false;
	
	void OnEnable(){				
		CubeAtlasUVManager cm=(CubeAtlasUVManager)target;
		UVPositions.Add("front",cm.front);
		UVPositions.Add("back",cm.back);
		UVPositions.Add("left",cm.left);
		UVPositions.Add("right",cm.right);
		UVPositions.Add("top",cm.top);
		UVPositions.Add("bottom",cm.bottom);	
		
		
		createMeshIfNull();
	}

	void createMeshIfNull ()
	{
		
		CubeAtlasUVManager cm=(CubeAtlasUVManager)target;
		if (cm.gameObject.GetComponent<MeshFilter>().sharedMesh ==null ){
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);		
			Mesh newMesh= cloneMesh(cube.GetComponent<MeshFilter>().sharedMesh);
			cm.gameObject.GetComponent<MeshFilter>().sharedMesh=newMesh;
			
			DestroyImmediate(cube);			
			
			AssetDatabase.CreateAsset(newMesh, AssetDatabase.GetAssetPath(target).Replace("prefab","mesh.asset"));
			AssetDatabase.SaveAssets();
		}
	}
	
	
	public override void OnInspectorGUI(){
		
		CubeAtlasUVManager cm=(CubeAtlasUVManager)target;
		
		int numberOfTiles=cm.rowCount * cm.columnCount-1;
		
		if(GUILayout.Button("Instantiate Shared Mesh"))
			instantiateMesh();
		
		
		int newRowCountIntValue=EditorGUILayout.IntField("Row Count",cm.rowCount);
		if (newRowCountIntValue!=cm.rowCount){
			cm.rowCount=newRowCountIntValue;
			updateTargetMesh();
		}
		
		int newColumnCountIntValue=EditorGUILayout.IntField("Column Count",cm.columnCount);
		if (newColumnCountIntValue!=cm.columnCount){
			cm.columnCount=newColumnCountIntValue;
			updateTargetMesh();
		}
		
		foreach(KeyValuePair<string,UVPosition> uvPosition in UVPositions){
			
			EditorGUILayout.LabelField("--------------------------------------------------------------");
			EditorGUILayout.PrefixLabel(uvPosition.Key);			
			int newPosition=EditorGUILayout.IntSlider("position",uvPosition.Value.position,0,numberOfTiles);
			if (newPosition!=uvPosition.Value.position){
				uvPosition.Value.position=newPosition;
				updateTargetMesh();					
			}
			
			int newRotation=EditorGUILayout.IntSlider("rotation",uvPosition.Value.rotation,0,3);
			if (newRotation!=uvPosition.Value.rotation){
				uvPosition.Value.rotation=newRotation;
				updateTargetMesh();				
			}						
		}		
		
		debugMode= EditorGUILayout.Toggle("debug mode (show vertex id)", debugMode);
		
	}

	public  void OnSceneGUI(){
		if (debugMode)
			showVertexIds();
	}

	public void showIntSlider (string label, ref int intFrom, int intTo,ref int actualIntValue)
	{
		int newIntValue=EditorGUILayout.IntSlider(label,actualIntValue,intFrom,intTo);
		if (newIntValue!=actualIntValue){
			actualIntValue=newIntValue;
			updateTargetMesh();
		}
	}
	
	
	
	public void showVertexIds ()
	{
		float vertexOffset=1.1f;
		float offsetStep=0.1f;
		
		
		CubeAtlasUVManager cm=(CubeAtlasUVManager)target;
		Vector3 goPosition=cm.transform.position;
		Mesh mesh= cm.gameObject.GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices=mesh.vertices;
		for (int i = 0; i < vertices.Length; i++) {
			Vector3 lablePosition=vertices[i] * (vertexOffset +offsetStep*i)  +goPosition;			
			Handles.Label(lablePosition,"["+i+"]");
		}
		
		
		string tRow="";
		int[] tris=mesh.triangles;
		for (int i = 0; i < tris.Length; ) {
			tRow+="[" + tris[i++] + "," + tris[i++] + "," + tris[i++] +"]\n";
		}
		Handles.Label(goPosition+Vector3.right,tRow);
		
		tRow="";
		Vector2[] uvs=mesh.uv;
		for (int i = 0; i < uvs.Length;i++ ) {
			tRow+=i+"="+uvs[i]+"\n";
		}
		Handles.Label(goPosition-Vector3.right,tRow);
		
		
		
	}

	void updateTargetMesh ()
	{
		CubeAtlasUVManager cm=(CubeAtlasUVManager)target;
		cm.updateMesh();
	}
	
	
	
	private Mesh  instantiateMesh(){		
		CubeAtlasUVManager cam=(CubeAtlasUVManager)target;
		GameObject gameObject = cam.gameObject;
		MeshFilter mf=gameObject.GetComponent<MeshFilter>();
		Mesh sharedMesh=mf.sharedMesh;
		if (sharedMesh!=null){			
			Mesh m= cloneMesh(sharedMesh);
			mf.sharedMesh=m;
			return m;
		}
		return sharedMesh;
	}

	Mesh cloneMesh (Mesh sharedMesh)
	{
		Mesh m=new Mesh();	
		m.vertices=sharedMesh.vertices;
		m.uv=sharedMesh.uv;
		m.triangles=sharedMesh.triangles;
		m.normals=sharedMesh.normals;
		m.tangents=sharedMesh.tangents;
		m.name=sharedMesh.name+" Instance";
		return m;
	}
}
