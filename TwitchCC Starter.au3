#NoTrayIcon
#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
If FileExists(@ScriptDir & "\bin\Twitch CC.exe") == 0 Then
	MsgBox(16, "Error", "File/Folder not found!")
Else
	Run(@ScriptDir & "\bin\Twitch CC.exe")
EndIf