//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Transition implementation that shows 3d tiles.
/// </summary>
[AddComponentMenu("Scripts/SceneManager/Tiles Transition")]
public class SMTilesTransition : SMPostRenderTransitionWithMobileShader
{
	
	/// <summary>
	/// The background texture.
	/// </summary>
	public Texture backgroundTexture;
	
	/// <summary>
	/// The size of the tiles in pixels (if geater than 1) or relative to the screen (otherwise).
	/// </summary>
	public Vector2 preferredTileSize = new Vector2 (100, 100);
	
	/// <summary>
	/// The duration of the effect.
	/// </summary>
	public float duration = 2f;
	
	/// <summary>
	/// The time to flip a single tile.
	/// </summary>
	public float tilesFlipTime = .5f;

	private float distance = 10f;
	private Vector2 actualTileSize;
	private int columns;
	private int rows;
	private float tileStartOffset;
	private Vector3 topLeft;
	private Vector3 bottomRight;
	private float width;
	private float height;
	private float effectTime;
	private Vector2[] uvSet1;
	private Vector2[] uvSet2;
	private Vector3[] tileVertices;
	private Vector3[] transformedVertices;

	protected override string FullShaderName {
		get {
			return "Scene Manager/Tiles Effect";
		}
	}

	protected override void DoPrepare ()
	{
		shaderMaterial.SetTexture("_Backface", holdMaterial.mainTexture);

		if (backgroundTexture) {
			backgroundMaterial.mainTexture = backgroundTexture;
		}
		
		topLeft = gameObject.camera.ScreenToWorldPoint (new Vector3 (0, Screen.height, distance));
		bottomRight = gameObject.camera.ScreenToWorldPoint (new Vector3 (Screen.width, 0, distance));
		
		width = bottomRight.x - topLeft.x;
		height = topLeft.y - bottomRight.y;
		
		columns = Mathf.FloorToInt (Screen.width / SMTransitionUtils.ToAbsoluteSize (preferredTileSize.x, Screen.width));
		rows = Mathf.FloorToInt (Screen.height / SMTransitionUtils.ToAbsoluteSize (preferredTileSize.y, Screen.height));	
		                        
		// recalculate size to avoid clipped tiles
		actualTileSize = new Vector2 (width / columns, height / rows);

		tileStartOffset = (duration - tilesFlipTime) / (columns + rows);	

		tileVertices = new Vector3[rows * columns * 4]; // 4 vertices per tile
		transformedVertices = new Vector3[rows * columns * 4]; // 4 transformed vertices per tile.
		uvSet1 = new Vector2[rows * columns * 4]; // 4 uv coords per tile (1 per vertex)
		uvSet2 = new Vector2[rows * columns * 4]; // 4 uv coords per tile (1 per vertex)

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				CalculateStaticTileData (x, y);
			}
		}

	}

	protected override bool Process (float elapsedTime)
	{
		effectTime = elapsedTime - simplifiedShaderLagCompensation;
		// calculation phase.
		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				float tileProgress = SMTransitionUtils.SmoothProgress ((x + y) * tileStartOffset, tilesFlipTime, effectTime);
				CalculateTile (x, y, tileProgress * 180);
			}
		}
		return effectTime < duration;
	}

	protected override void DoRender ()
	{
		// rendering phase
		GL.PushMatrix ();
		GL.LoadProjectionMatrix (gameObject.camera.projectionMatrix);
		GL.LoadIdentity ();

		for (int pass = 0; pass < shaderMaterial.passCount; pass++) {
				shaderMaterial.SetPass (pass);
				GL.Begin (GL.QUADS);
				for (int x = 0; x < columns; x++) {
					for (int y = 0; y < rows; y++) {
						DrawTile (x, y, pass);
					}
				}
				GL.End ();

		}

		// it's z-tested to be drawn behind the tiles.
		DrawBackground ();
		GL.PopMatrix (); 
	}

	/// <summary>
	/// Calculates the static tile data which never changes throughout the transition.
	/// </summary>
	private void CalculateStaticTileData (int xIndex, int yIndex)
	{
		float halfWidth = actualTileSize.x / 2f;
		float halfHeight = actualTileSize.y / 2f;
		
		float xOffset = actualTileSize.x * xIndex;
		float yOffset = actualTileSize.y * yIndex;
		float umin = xOffset / width;
		float umax = (xOffset + actualTileSize.x) / width;
		float vmin = (height - yOffset - actualTileSize.y) / height;
		float vmax = (height - yOffset) / height;

		var baseIndex = GetIndex (xIndex, yIndex);

		// Vector3 translation = new Vector3 (topLeft.x + xOffset + halfWidth, topLeft.y - yOffset - halfHeight, -distance);
		// var matrix = Matrix4x4.TRS (translation, Quaternion.identity, Vector3.one);

		tileVertices [baseIndex] = (new Vector3 (-halfWidth, -halfHeight, 0));
		tileVertices [baseIndex + 1] = (new Vector3 (-halfWidth, halfHeight, 0));
		tileVertices [baseIndex + 2] = (new Vector3 (halfWidth, halfHeight, 0));
		tileVertices [baseIndex + 3] = (new Vector3 (halfWidth, -halfHeight, 0));

		uvSet1 [baseIndex] = new Vector2 (umax, vmin);
		uvSet2 [baseIndex] = new Vector2 (umin, vmin);
		uvSet1 [baseIndex + 1] = new Vector2 (umax, vmax);
		uvSet2 [baseIndex + 1] = new Vector2 (umin, vmax);
		uvSet1 [baseIndex + 2] = new Vector2 (umin, vmax);
		uvSet2 [baseIndex + 2] = new Vector2 (umax, vmax);
		uvSet1 [baseIndex + 3] = new Vector2 (umin, vmin);
		uvSet2 [baseIndex + 3] = new Vector2 (umax, vmin);
	}

	/// <summary>
	/// Calculates the parts of the tile that change during the transition.
	/// </summary>
	private void CalculateTile (int xIndex, int yIndex, float progress)
	{
		float halfWidth = actualTileSize.x / 2f;
		float halfHeight = actualTileSize.y / 2f;
		
		float xOffset = actualTileSize.x * xIndex;
		float yOffset = actualTileSize.y * yIndex;

		var baseIndex = GetIndex (xIndex, yIndex);
		Vector3 translation = new Vector3 (topLeft.x + xOffset + halfWidth, topLeft.y - yOffset - halfHeight, -distance);
		Quaternion rotation = Quaternion.AngleAxis (progress + (state == SMTransitionState.In ? 180 : 0), Vector3.up);
		var matrix = Matrix4x4.TRS (translation, rotation, Vector3.one);

		transformedVertices [baseIndex] = matrix.MultiplyPoint (tileVertices [baseIndex]);
		transformedVertices [baseIndex + 1] = matrix.MultiplyPoint (tileVertices [baseIndex + 1]);
		transformedVertices [baseIndex + 2] = matrix.MultiplyPoint (tileVertices [baseIndex + 2]);
		transformedVertices [baseIndex + 3] = matrix.MultiplyPoint (tileVertices [baseIndex + 3]);

	}

	/// <summary>
	/// Draws the tile using pre-calculated data.
	/// </summary>
	private void DrawTile (int xIndex, int yIndex, int pass)
	{
		var baseIndex = GetIndex (xIndex, yIndex);
		for (int i = 0; i < 4; i++) {
			var texCoordSet = uvSet1;
			if ((!useSimplifiedShader && pass == 1) || (useSimplifiedShader && pass == 0)) {
				texCoordSet = uvSet2;
			}
			var texCoord =  texCoordSet[baseIndex + i];
			var vertex = transformedVertices [baseIndex + i];
			GL.TexCoord3 (texCoord.x, texCoord.y, 0);
			GL.Vertex3 (vertex.x, vertex.y, vertex.z);
		}
	}

	/// <summary>
	/// Provides the base index in the tile vertex buffer for the given x and y index.
	/// </summary>
	/// <returns>The index.</returns>
	/// <param name="xIndex">X index.</param>
	/// <param name="yIndex">Y index.</param>
	private int GetIndex (int xIndex, int yIndex)
	{
		var result = (columns * yIndex + xIndex ) * 4;
		return result;
	}

}

