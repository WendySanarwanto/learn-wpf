Imports System.IO
Imports System.Linq

Public Class DirectoryStructure

    Public Shared Function GetLogicalDrives() As IList(Of DirectoryItem)
        Return Directory.GetLogicalDrives().Select(Function(drive) New DirectoryItem With {.FullPath = drive, .Type = DirectoryItemType.Drive}).ToList()
    End Function

    Public Shared Function GetDirectoryContent(fullpath As String) As IList(Of DirectoryItem)
        ' Get Folders
        Dim items As List(Of DirectoryItem) = New List(Of DirectoryItem)
        Try
            Dim directoryPaths As String() = Directory.GetDirectories(fullpath)
            If directoryPaths.Length > 0 Then
                items.AddRange(directoryPaths.Select(Function(directoryPath As String) New DirectoryItem With {.FullPath = directoryPath, .Type = DirectoryItemType.Folder}))
            End If
        Catch ex As Exception
            ' Suppress expected errors
        End Try

        ' Get files
        Try
            Dim files As String() = Directory.GetFiles(fullpath)
            If files.Length > 0 Then
                items.AddRange(files.Select(Function(filePath As String) New DirectoryItem With {.FullPath = filePath, .Type = DirectoryItemType.File}))
            End If
        Catch ex As Exception
            ' Suppress expected errors
        End Try

        Return items
    End Function

    ''' <summary>
    ''' A function to get folder's/file's name
    ''' </summary>
    ''' <param name="directoryPath"></param>
    ''' <returns></returns>
    Public Shared Function GetFileFolderName(directoryPath As String) As String
        ' C:\Something\a folder
        ' C:\Something/a file.png
        ' a file file.png
        If (String.IsNullOrEmpty(directoryPath)) Then
            Return String.Empty
        End If

        ' Make slashes as backslashes
        Dim normalisedPath As String = directoryPath.Replace("/", "\\")

        ' Find last index of backslashes
        Dim lastIndexOfBackslashes As Integer = normalisedPath.LastIndexOf("\")
        If (lastIndexOfBackslashes <= 0) Then
            Return directoryPath
        End If

        ' Return the name after the last back slash
        Return directoryPath.Substring(lastIndexOfBackslashes + 1)
    End Function

End Class
