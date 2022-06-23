Imports System.Runtime.InteropServices
Imports Fasetto.Word.Fasetto.Word
''' <summary>
''' ViewModel for the custom flat window
''' </summary>
Public Class WindowViewModel
    Inherits BaseViewModel
#Region "Private fields"
    Private ReadOnly _window As Window
    ''' <summary>
    ''' The margin around the window to allow for a drop shadow
    ''' </summary>
    Private _outerMargin As Integer = 10

    ''' <summary>
    ''' The radius of edges of the Window
    ''' </summary>
    Private _windowRadius As Integer = 10

#End Region

#Region "Constructor"
    Public Sub New(window As Window)
        Me._window = window

        ' Listen out for Window Resizing's event
        AddHandler _window.StateChanged, AddressOf Window_StateChanged

        ' Create commands 
        MinimiseCommand = New RelayCommand(Sub() _window.WindowState = WindowState.Minimized)
        MaximiseCommand = New RelayCommand(Sub() If _window.WindowState = WindowState.Maximized Then _window.WindowState = WindowState.Normal Else _window.WindowState = WindowState.Maximized)
        CloseCommand = New RelayCommand(Sub() _window.Close())
        MenuCommand = New RelayCommand(Sub() SystemCommands.ShowSystemMenu(_window, GetMousePosition()))

        ' Fix window resize issue
        Dim resizer As WindowResizer = New WindowResizer(_window)

    End Sub

#End Region

#Region "Properties"
    Public Property ResizeBorder As Integer = 6

    ''' <summary>
    ''' The size of the resize border around the window, taking into account the outer margin
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ResizeBorderThickness As Thickness
        Get
            Return New Thickness(Me.ResizeBorder + Me.OuterMarginSize)
        End Get
    End Property

    ''' <summary>
    ''' The margin around the window to allow for a drop shadow
    ''' </summary>
    Public Property OuterMarginSize As Integer
        Get
            Return If(_window.WindowState = WindowState.Maximized, 0, _outerMargin)
        End Get
        Set(value As Integer)
            _outerMargin = value
        End Set
    End Property

    Public ReadOnly Property OuterMarginSizeThickness As Thickness
        Get
            Return New Thickness(OuterMarginSize)
        End Get
    End Property

    ''' <summary>
    ''' The radius of edges of the Window
    ''' </summary>
    Public Property WindowRadius As Integer
        Get
            Return If(_window.WindowState = WindowState.Maximized, 0, _windowRadius)
        End Get
        Set(value As Integer)
            _windowRadius = value
        End Set
    End Property

    ''' <summary>
    ''' The radius of edges of the Window
    ''' </summary>
    Public ReadOnly Property WindowCornerRadius As CornerRadius
        Get
            Return New CornerRadius(WindowRadius)
        End Get
    End Property

    ''' <summary>
    ''' The height of the title bar / caption of the window
    ''' </summary>
    ''' <returns></returns>
    Public Property TitleHeight As Integer = 42

    ''' <summary>
    ''' The height of the title bar / caption of the window
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TitleHeightGridLength As GridLength
        Get
            Return New GridLength(TitleHeight + ResizeBorder)
        End Get
    End Property

    ''' <summary>
    ''' The smallest width the window can be
    ''' </summary>
    ''' <returns></returns>
    Public Property WindowMinimumWidth As Double = 400

    ''' <summary>
    ''' The smallest height the window can be
    ''' </summary>
    ''' <returns></returns>
    Public Property WindowMinimumHeight As Double = 400

    ''' <summary>
    ''' The padding of inner conent of the main window
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property InnerContentPadding As Thickness
        Get
            Return New Thickness(ResizeBorder)
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Private Sub Window_StateChanged(sender As Object, e As EventArgs)
        ' Fire off events for all properties that are affected by a resize
        Me.OnPropertyChanged(NameOf(ResizeBorderThickness))
        Me.OnPropertyChanged(NameOf(OuterMarginSize))
        Me.OnPropertyChanged(NameOf(OuterMarginSizeThickness))
        Me.OnPropertyChanged(NameOf(WindowRadius))
        Me.OnPropertyChanged(NameOf(WindowCornerRadius))
    End Sub

#End Region

#Region "Commands"
    ''' <summary>
    ''' The command to minimise the window 
    ''' </summary>
    ''' <returns></returns>
    Public Property MinimiseCommand As ICommand

    ''' <summary>
    ''' The command to maximise the window 
    ''' </summary>
    ''' <returns></returns>
    Public Property MaximiseCommand As ICommand

    ''' <summary>
    ''' The command to close the window 
    ''' </summary>
    ''' <returns></returns>
    Public Property CloseCommand As ICommand

    ''' <summary>
    ''' The command to show System Menu 
    ''' </summary>
    ''' <returns></returns>
    Public Property MenuCommand As ICommand
#End Region

#Region "Helpers"
    Private Function GetMousePosition() As Windows.Point
        Dim position As Windows.Point = Mouse.GetPosition(_window)

        Return New Windows.Point(position.X + _window.Left, position.Y + _window.Top)
    End Function
#End Region

End Class
