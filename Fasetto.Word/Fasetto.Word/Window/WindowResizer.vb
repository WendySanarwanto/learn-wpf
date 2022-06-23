Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Interop

Namespace Fasetto.Word
    ''' <summary>
    ''' Fixes the issue with Windows of Style <seecref="WindowStyle.None"/> covering the taskbar
    ''' </summary>
    Public Class WindowResizer
#Region "Private Members"

        ''' <summary>
        ''' The window to handle the resizing for
        ''' </summary>
        Private mWindow As Window

#End Region

#Region "Dll Imports"

        <DllImport("user32.dll", ExactSpelling:=True, SetLastError:=True)>
        Private Shared Function GetCursorPos(<Out> ByRef lpPoint As POINT) As Boolean
        End Function

        <DllImport("user32.dll")>
        Private Shared Function GetMonitorInfo(ByVal hMonitor As IntPtr, ByVal lpmi As MONITORINFO) As Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function MonitorFromPoint(ByVal pt As POINT, ByVal dwFlags As MonitorOptions) As IntPtr
        End Function

#End Region

#Region "Constructor"

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        ''' <paramname="window">The window to monitor and correctly maximize</param>
        ''' <paramname="adjustSize">The callback for the host to adjust the maximum available size if needed</param>
        Public Sub New(ByVal window As Window)
            mWindow = window

            ' Listen out for source initialized to setup
            AddHandler mWindow.SourceInitialized, AddressOf Window_SourceInitialized
        End Sub

#End Region

#Region "Initialize"

        ''' <summary>
        ''' Initialize and hook into the windows message pump
        ''' </summary>
        ''' <paramname="sender"></param>
        ''' <paramname="e"></param>
        Private Sub Window_SourceInitialized(ByVal sender As Object, ByVal e As EventArgs)
            ' Get the handle of this window
            Dim handle = (New WindowInteropHelper(mWindow)).Handle
            Dim handleSource = HwndSource.FromHwnd(handle)

            ' If not found, end
            If handleSource Is Nothing Then Return

            ' Hook into it's Windows messages
            handleSource.AddHook(AddressOf WindowProc)
        End Sub

#End Region

#Region "Windows Proc"

        ''' <summary>
        ''' Listens out for all windows messages for this window
        ''' </summary>
        ''' <paramname="hwnd"></param>
        ''' <paramname="msg"></param>
        ''' <paramname="wParam"></param>
        ''' <paramname="lParam"></param>
        ''' <paramname="handled"></param>
        ''' <returns></returns>
        Private Function WindowProc(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByRef handled As Boolean) As IntPtr
            Select Case msg
                ' Handle the GetMinMaxInfo of the Window
                Case &H24 ' WM_GETMINMAXINFO 
                    WmGetMinMaxInfo(hwnd, lParam)
                    handled = True
            End Select

            Return CType(0, IntPtr)
        End Function

#End Region

        ''' <summary>
        ''' Get the min/max window size for this window
        ''' Correctly accounting for the taskbar size and position
        ''' </summary>
        ''' <paramname="hwnd"></param>
        ''' <paramname="lParam"></param>
        Private Sub WmGetMinMaxInfo(ByVal hwnd As IntPtr, ByVal lParam As IntPtr)
            Dim lMousePosition As POINT
            GetCursorPos(lMousePosition)

            Dim lPrimaryScreen As IntPtr = MonitorFromPoint(New POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY)
            Dim lPrimaryScreenInfo As MONITORINFO = New MONITORINFO()
            If GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) = False Then
                Return
            End If

            Dim lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST)

            Dim lMmi As MINMAXINFO = Marshal.PtrToStructure(lParam, GetType(MINMAXINFO))

            If lPrimaryScreen.Equals(lCurrentScreen) = True Then
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top
            Else
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top
            End If

            ' Now we have the max size, allow the host to tweak as needed
            Marshal.StructureToPtr(lMmi, lParam, True)
        End Sub
    End Class

#Region "Dll Helper Structures"

    Friend Enum MonitorOptions As UInteger
        MONITOR_DEFAULTTONULL = &H0
        MONITOR_DEFAULTTOPRIMARY = &H1
        MONITOR_DEFAULTTONEAREST = &H2
    End Enum


    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Class MONITORINFO
        Public cbSize As Integer = Marshal.SizeOf(GetType(MONITORINFO))
        Public rcMonitor As Rectangle = New Rectangle()
        Public rcWork As Rectangle = New Rectangle()
        Public dwFlags As Integer = 0
    End Class


    <StructLayout(LayoutKind.Sequential)>
    Public Structure Rectangle
        Public Left, Top, Right, Bottom As Integer

        Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
            Me.Left = left
            Me.Top = top
            Me.Right = right
            Me.Bottom = bottom
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure MINMAXINFO
        Public ptReserved As POINT
        Public ptMaxSize As POINT
        Public ptMaxPosition As POINT
        Public ptMinTrackSize As POINT
        Public ptMaxTrackSize As POINT
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure POINT
        ''' <summary>
        ''' x coordinate of point.
        ''' </summary>
        Public X As Integer
        ''' <summary>
        ''' y coordinate of point.
        ''' </summary>
        Public Y As Integer

        ''' <summary>
        ''' Construct a point of coordinates (x,y).
        ''' </summary>
        Public Sub New(ByVal x As Integer, ByVal y As Integer)
            Me.X = x
            Me.Y = y
        End Sub
    End Structure

#End Region
End Namespace
