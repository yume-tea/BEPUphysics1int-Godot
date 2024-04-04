using Godot;
using System;
using System.IO; // For getting file extensions
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUutilities;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;


[Tool]
public partial class AnimFix64Converter : EditorScript
{
	// Folder path takes precedence over anim path
	string animPath = "";
	string folderPath = "res://actors/player/anims/";

	public override void _Run()
	{
		if (folderPath != "") {
			ConvertAnimsFolder();
		}
		else if (animPath != "") {
			ConvertAnim(animPath);
		}
		else {
			GD.Print("Please enter a valid animation path or animation folder path");
		}
	}

	private void ConvertAnimsFolder() {
		if (folderPath != "") {

		}
		 var dir = DirAccess.Open(folderPath);  // Open directory in DirAccess object
		
		 if (dir != null) {
		 		dir.ListDirBegin();  // Initialize the DirAccess stream
		 		var fileName = dir.GetNext();
		 		while (fileName != "") {
		 			if (dir.CurrentIsDir()) {
		 				GD.Print("dir " + '"' + fileName + '"' + " found in AnimFix64Converter.cs");
		 			}
		 			else {
		 				if (Path.GetExtension(fileName) == ".anim" && !Path.GetFileNameWithoutExtension(animPath).Contains("Fix64")) {
		 					ConvertAnim(folderPath + fileName);
		 				}
		 			}

		 			fileName = dir.GetNext();  // Get next item in directory
		 		}
		 }
		 else {
		 	GD.Print("unable to access " + '"' + folderPath + '"');
		 }
	}

	private void ConvertAnim(string animPath) {
		ConvertTracksToFix64(animPath);
	}

	private void ConvertTracksToFix64(string animPath) {
		Animation anim = GD.Load<Animation>(animPath);
		Animation animNew = null;

		for (int i = 0; i < anim.GetTrackCount(); i++) {
			if (anim.TrackGetType(i) == Animation.TrackType.Position3D) {
				animNew = ConvertPosition3DTracksToFix64Tracks(anim, i);
			}
		}

		if (animNew != null) {
			// Make new filename
			string fileName = Path.GetFileNameWithoutExtension(animPath);
			string fileNameNew = fileName + "Fix64" + ".anim";
			string folderPathNew = Path.GetDirectoryName(animPath).Remove(0, 5).Insert(0, "res://");
			GD.Print(folderPathNew);

			Godot.ResourceSaver.Save(animNew, folderPathNew + "/" + fileNameNew);
		}
		else {
			GD.Print("no valid tracks found to convert for " + animPath);
		}
	}

	private Animation ConvertPosition3DTracksToFix64Tracks(Animation anim, int trackIdx)
	{
		Animation animNew = new Animation();
		animNew.Length = anim.Length;
		animNew.LoopMode = anim.LoopMode;
		animNew.Step = anim.Step;
		List<Godot.Vector3> keyValues = new List<Godot.Vector3>();
		// int trackIdxNew = anim.GetTrackCount();

		// Add a new method track
		animNew.AddTrack(Animation.TrackType.Method, 0);
		animNew.TrackSetPath(0, "./");

		// Create method track keys based on original track
		int keyFrameTimeLast = 0;
		for (int i = 0; i < anim.TrackGetKeyCount(trackIdx); i++) {
			int keyFrameTime = (int)Math.Round(anim.TrackGetKeyTime(trackIdx, i) * 60);
			int frameWindow = keyFrameTime - keyFrameTimeLast;

			if (frameWindow == 0) {  // Avoid dividing by 0
				continue;
			}

			// Get value for each individual frame before the keyframe
			Godot.Vector3 keyFrameValue = (((Godot.Vector3)anim.TrackGetKeyValue(trackIdx, i)) / frameWindow) * 60;
			long x = Util.Float64ToFix64Raw(keyFrameValue.X);
			long y = Util.Float64ToFix64Raw(keyFrameValue.Y);
			long z = Util.Float64ToFix64Raw(keyFrameValue.Z);

			Godot.Collections.Dictionary frameValue = new Godot.Collections.Dictionary();
			Godot.Collections.Array valueArray = new Godot.Collections.Array();
			valueArray.Add(x);
			valueArray.Add(y);
			valueArray.Add(z);

			frameValue.Add("method", "Move");
			frameValue.Add("args", valueArray);

			// Set velocity value for all frames before current keyframe
			for (int f = keyFrameTimeLast; f < keyFrameTime; f++) {
				animNew.TrackInsertKey(0, (double)f / (double)60, frameValue);
			}

			keyFrameTimeLast = keyFrameTime;
		}

		return animNew;
	}
}

