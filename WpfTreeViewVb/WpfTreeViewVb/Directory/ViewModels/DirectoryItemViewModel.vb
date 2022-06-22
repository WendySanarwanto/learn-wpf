Imports System.Collections.ObjectModel

Public Class DirectoryItemViewModel
    Inherits BaseViewModel

#Region "Properties"

    Public Property Type As DirectoryItemType

    Public Property Fullpath As String

    Public ReadOnly Property Name As String
        Get
            Return If(Me.Type = DirectoryItemType.Drive, Me.Fullpath, DirectoryStructure.GetFileFolderName(Me.Fullpath))
        End Get
    End Property

    Public Property Children As ObservableCollection(Of DirectoryItemViewModel)

    Public ReadOnly Property CanExpand() As Boolean
        Get
            Return Me.Type <> DirectoryItemType.File
        End Get
    End Property

    Public Property IsExpanded As Boolean
        Get
            'Return Children?.Count(Function(f As DirectoryItemViewModel) f <> Nothing) > 0
            Dim result As Boolean = Children?.LongCount(Function(f As DirectoryItemViewModel) f IsNot Nothing) > 0
            Return result
        End Get
        Set(value As Boolean)
            ' If the UI tells us to expand ...
            If (value) Then
                ' Find all children 
                Expand()
            Else
                ' If the UI tells us to close
                ClearChildren()
            End If
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New(fullpath As String, type As DirectoryItemType)
        ExpandCommand = New RelayCommand(AddressOf Expand)

        Me.Type = type
        Me.Fullpath = fullpath

        ' Setup the children as needed
        Me.ClearChildren()
    End Sub

#End Region

#Region "Public Commands"

    ''' <summary>
    ''' Command to expand this item.
    ''' </summary>
    ''' <returns></returns>
    Public Property ExpandCommand As ICommand

#End Region

#Region "Helper methods"

    ''' <summary>
    ''' Remove all children from the list, and adding a dummy item to show the expand icon if required
    ''' </summary>
    Private Sub ClearChildren()
        ' Clear items
        Me.Children?.Clear()
        Me.Children = New ObservableCollection(Of DirectoryItemViewModel)()

        ' Show the expand arrow if we are not file
        If Me.Type <> DirectoryItemType.File Then
            Me.Children.Add(Nothing)
        End If

    End Sub

#End Region

    ''' <summary>
    ''' Expand this directory and finds all children
    ''' </summary>
    Private Sub Expand()
        If (Type = DirectoryItemType.File) Then
            Return
        End If

        Dim directoryContent As IList(Of DirectoryItem) = DirectoryStructure.GetDirectoryContent(Fullpath)
        Children = New ObservableCollection(Of DirectoryItemViewModel)(directoryContent.Select(Function(dirItem As DirectoryItem) New DirectoryItemViewModel(dirItem.FullPath, dirItem.Type)))
    End Sub
End Class
