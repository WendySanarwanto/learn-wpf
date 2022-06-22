Public Class DirectoryItem
    Public Property Type As DirectoryItemType

    Public Property FullPath As String

    Public Function GetName() As String
        Return If(Me.Type = DirectoryItemType.Drive, Me.FullPath, DirectoryStructure.GetFileFolderName(Me.FullPath))
    End Function
End Class
