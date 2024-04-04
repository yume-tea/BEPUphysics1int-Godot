using Godot;
using System;

using FixMath.NET;
using BEPUutilities;

[Tool]
public partial class ColShapeMesh : Shape
{
	string materialPhysicsBodyPath = "res://-custom_nodes/ColShape/Shape/materials_collision/debug_physicsbody.material";

	public ArrayMesh MeshCollision;
	[Export]
	private ArrayMesh meshCollision {
		get => MeshCollision;
		set {
			MeshCollision = value;

			if (Engine.IsEditorHint()) {  // Avoid any float values changing fixed point raw values when the game runs
				Mesh = MeshCollision;

				if (Mesh == null) { 
					Indices = new int[0];
					VertexPoints = new long[0];
					return;
				}

				// Store indices and vertices
				Godot.Vector3[] faces = ((Mesh)MeshCollision).GetFaces();


				VertexPoints = PackedVector3ArrayToFix64RawValueArray(faces);  // Convert vertices to fixed point raw values

				Indices = new int[faces.Length];
				for (int i = 0; i < faces.Length; i++) {  // Store indices
					Indices[i] = i;
				}
			}

			// Apply default material to shape
			if (Mesh != null) {
				if (FileAccess.FileExists(materialPhysicsBodyPath)) {
					Material materialCollision = GD.Load<Material>(materialPhysicsBodyPath);
					Mesh.SurfaceSetMaterial(0, materialCollision);
				}
			}
		}
	}

	[Export]
	public int[] Indices;

	public BEPUutilities.Vector3[] Vertices;

	private long [] VertexPoints;
	[Export]
	private long[] vertexPoints {
		get => VertexPoints;
		set {
			VertexPoints = value;
			BEPUutilities.Vector3[] vertices = new BEPUutilities.Vector3[VertexPoints.Length / 3];

			for (int i = 0; i < vertices.Length; i++) {  // Create fixed point vertices using array of raw values of each point
				vertices[i] = new BEPUutilities.Vector3(Fix64.FromRaw(VertexPoints[(3*i)]),Fix64.FromRaw(VertexPoints[(3*i)+1]),Fix64.FromRaw(VertexPoints[(3*i)+2]));
			}

			Vertices = vertices;
		}
	}

	private long[] PackedVector3ArrayToFix64RawValueArray(Godot.Vector3[] verticesFloat)
	{
		long[] points = new long[verticesFloat.Length * 3];

		// Extract the 3 points from each vertex, convert to raw 64bit values
		for (int i = 0; i < verticesFloat.Length; i++) {
			points[(3*i)] = ((Fix64)verticesFloat[i].X).RawValue;
			points[(3*i)+1] = ((Fix64)verticesFloat[i].Y).RawValue;
			points[(3*i)+2] = ((Fix64)verticesFloat[i].Z).RawValue;
		}

		return points;
	}
}
