using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class ProcessFetch : MonoBehaviour {

	// addresses: 0x19BEB94, 0x1DBEB94
	const int PROCESS_WM_READ = 0x0010;
	IntPtr processHandle;
	byte[] buffer;
	Int32 memoryAddress;
	InputField inputField;
	Text igtText;


	void Awake () {
		Process process = Process.GetProcessesByName("sh2pc")[0]; 
		processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
		inputField = GameObject.Find ("InputField").GetComponent<InputField>();
		igtText = GameObject.Find ("IGT").GetComponent<Text> ();
	}

	void Update() {
		
	}

	public void ScanProcessInMemory() {
		memoryAddress = Convert.ToInt32 (inputField.text, 16);
		if (memoryAddress != null) {
			buffer = ReadMemory (memoryAddress, sizeof(float), (int)processHandle);
			float myFloat = System.BitConverter.ToSingle (buffer, 0);
			UnityEngine.Debug.Log (myFloat);
			igtText.text = "IGT: " + myFloat;
		}
	}

	// Other methods
	public byte[] ReadMemory(int address, int processSize, int processHandle) {
		long bytesRead = 0;
		byte[] buffer = new byte[processSize];
		bool result = ReadProcessMemory(processHandle, address, buffer, processSize, ref bytesRead);
		UnityEngine.Debug.Log ("There is something on this address: " + ((result == true) ? "Yes" : "No") );
		return buffer;
	}
		
	// Helper methods
	void LoopProcesses(Process[] pArray) {
		int length = pArray.Length;

		for (int i = 0; i < length; i++) {
			UnityEngine.Debug.Log (pArray [i]);
		}
	}

	// Kernel methods
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	[DllImport("kernel32.dll")]
	public static extern bool ReadProcessMemory(int hProcess, 
		int lpBaseAddress, byte[] lpBuffer, int dwSize, ref long lpNumberOfBytesRead);
}
