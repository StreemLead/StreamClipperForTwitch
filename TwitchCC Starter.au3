#NoTrayIcon
#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Icon=F:\Daten\Business\Freddelus.io\Dev\Twitch CC\Icon.ico
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
If FileExists(@ScriptDir & "\bin\Twitch CC.exe") == 0 Then
	MsgBox(16, "Error", "File/Folder not found!")
Else
	Run(@ScriptDir & "\bin\Twitch CC.exe")
EndIf