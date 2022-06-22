Imports System.Collections.ObjectModel

''' <summary>
''' ViewModel for the application's main directory view
''' </summary>
Public Class DirectoryStructureViewModel
    Inherits BaseViewModel

#Region "Public Properties"

    ''' <summary>
    ''' A list of all directories on the machine
    ''' </summary>
    Public Property Items As ObservableCollection(Of DirectoryItemViewModel)


#End Region

#Region "Constructor"
    Public Sub New()
        Dim logicalDrivesViewModel As IEnumerable(Of DirectoryItemViewModel) = DirectoryStructure.GetLogicalDrives().Select(Function(drive As DirectoryItem) New DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive))
        Me.Items = New ObservableCollection(Of DirectoryItemViewModel)(logicalDrivesViewModel)
    End Sub

#End Region
End Class
