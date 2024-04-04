using Godot;
using System;
using System.Collections.Generic;

using FixMath.NET;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.IO.Compression;


public partial class DeterminismVerifier : Node
{
	public class VerificationLog {
		public int frameCount = 0;
		public List<List<Dictionary<string, BEPUutilities.Vector3>>> VerificationData = new List<List<Dictionary<string, BEPUutilities.Vector3>>>();

		public void Save(string filePath, string compressedFileName)
   		{
			string jsonString = JsonConvert.SerializeObject(this);
			File.WriteAllText(filePath, jsonString);
			CompressFile(filePath, compressedFileName);
    	}

		public static VerificationLog Load(string filePath, string compressedFileName)
    	{
			DecompressFile(filePath, compressedFileName);
			string jsonString = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<VerificationLog>(jsonString);
    	}

		private void CompressFile(string filePath, string compressedFileName)
		{
			using FileStream originalFileStream = File.Open(filePath, FileMode.Open);
			using FileStream compressedFileStream = File.Create(compressedFileName);
			using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
			originalFileStream.CopyTo(compressor);
		}

		private static void DecompressFile(string filePath, string compressedFileName)
		{
			using FileStream compressedFileStream = File.Open(compressedFileName, FileMode.Open);
			using FileStream outputFileStream = File.Create(filePath);
			using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
			decompressor.CopyTo(outputFileStream);
		}
	}

	public VerificationLog currentLog = new VerificationLog();
	string verificationLogFileName = "determinism_data.json";
	string verificationLogCompressedFileName = "determinism_data.gz";

	public VerificationLog verificationLog = new VerificationLog();

	public enum VerifierState {
		STOP,
		RECORD,
		VERIFY,
	}

	public VerifierState currentState = VerifierState.STOP;
	int currentFrame = 0;
	

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		switch (currentState) {
			case VerifierState.STOP:
				break;
			case VerifierState.RECORD:
				RecordFrameData();
				currentFrame++;
				break;
			case VerifierState.VERIFY:
				VerifyRecord();
				break;
		}
	}

	public void StartRecord()
	{
		currentFrame = 0;

		currentLog.VerificationData = new List<List<Dictionary<string, BEPUutilities.Vector3>>>();
		currentLog.frameCount = 0;
		currentState = VerifierState.RECORD;
	}

	public void EndRecord()
	{
		currentState = VerifierState.STOP;
	}

	public void RecordFrameData()
	{
		if (GetTree().CurrentScene == null) {
			GD.Print("current scene is not yet set");
			return;
		}

		// Create list for this frame of obj data
		currentLog.VerificationData.Add(new List<Dictionary<string, BEPUutilities.Vector3>>());

		// Loop through all children nodes of the current scene
		foreach (Node node in GetTree().CurrentScene.GetChildren()) {
			if (node is PhysicsBody) {
				if (node is StaticBody) {
					continue;
				}

				PhysicsBody obj = (PhysicsBody) node;

				// Record position, velocity in dictionary. Add to list.
				Dictionary<string, BEPUutilities.Vector3> objData = new Dictionary<string, BEPUutilities.Vector3>();
				if (obj.Body != null) {
					objData.Add("position", obj.Body.Position);
					objData.Add("velocity", obj.Body.LinearVelocity);
				}
				

				currentLog.VerificationData[currentFrame].Add(objData);
			}
		}
	}

	public void VerifyRecord()
	{
		GD.Print("verifying...");
		verificationLog = LoadRecord();
		GD.Print("loaded verification log");

		bool match = true;
		int mismatchFrame = -1;
		int mismatchIndex = -1;

		for (int i = 0; i < verificationLog.VerificationData.Count; i++) {
			for (int j = 0; j < verificationLog.VerificationData[i].Count; j++) {
				// GD.Print("current pos: " + currentLog.VerificationData[i][j]["position"].ToString() + "     verification pos: " + verificationLog.VerificationData[i][j]["position"].ToString());
				// GD.Print("current vel: " + currentLog.VerificationData[i][j]["velocity"].ToString() + "     verification vel: " + verificationLog.VerificationData[i][j]["velocity"].ToString());

				if ((currentLog.VerificationData[i][j]["position"] != verificationLog.VerificationData[i][j]["position"]) || (currentLog.VerificationData[i][j]["velocity"] != verificationLog.VerificationData[i][j]["velocity"])) {
					match = false;
				}

				if (!match) {
					mismatchFrame = i;
					mismatchIndex = j;
					break;
				}
			}
			if (!match) {
				break;
			}
		}

		if (match) {
			GD.Print("positions and velocities match last simulation!");
			GD.Print("recorded frames: " + currentLog.VerificationData.Count.ToString());
			GD.Print("recorded objects: " + currentLog.VerificationData[currentLog.VerificationData.Count - 1].Count.ToString());
		}
		else {
			GD.Print("position or velocity mismatch on frame " + mismatchFrame.ToString());
			GD.Print("saved position: " + verificationLog.VerificationData[mismatchFrame][mismatchIndex]["position"].ToString());
			GD.Print("current position: " + currentLog.VerificationData[mismatchFrame][mismatchIndex]["position"].ToString());
			GD.Print("saved velocity: " + verificationLog.VerificationData[mismatchFrame][mismatchIndex]["velocity"].ToString());
			GD.Print("current velocity: " + currentLog.VerificationData[mismatchFrame][mismatchIndex]["velocity"].ToString());
		}

		GD.Print("");

		currentState = VerifierState.STOP;
	}

	public void SaveRecord()
	{
		currentLog.Save(verificationLogFileName, verificationLogCompressedFileName);
	}

	public VerificationLog LoadRecord()
	{
		GD.Print("loading verification log...");
		VerificationLog log = VerificationLog.Load(verificationLogFileName, verificationLogCompressedFileName);
		GD.Print("loaded verification log!");

		return log;
	}
}
