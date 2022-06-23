Imports System.Runtime.InteropServices
Public Class Win32API
    <StructLayout(LayoutKind.Sequential)>
    Public Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure

    <DllImport("user32.dll", ExactSpelling:=True, SetLastError:=True)>
    Public Shared Function GetCursorPos(ByRef lpPoint As POINTAPI) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
End Class
