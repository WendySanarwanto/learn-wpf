Imports System.IO

Class MainWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        ' Get every logical drives On the machine
        Dim logicalDrives As String() = Directory.GetLogicalDrives()
        For Each logicalDrive As String In logicalDrives
            'Create new item for each logical drives
            Dim item As TreeViewItem = New TreeViewItem()
            With item
                ' Set item's Header
                .Header = logicalDrive
                ' And the fullpath
                .Tag = logicalDrive
            End With

            ' Listen out for item being expanded
            AddHandler item.Expanded, AddressOf Folder_Expanded

            ' Add a dummy item
            item.Items.Add(Nothing)

            FolderView.Items.Add(item)
        Next
    End Sub

    ''' <summary>
    ''' When a folder is expanded, find the subfolders/files
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Folder_Expanded(sender As Object, e As RoutedEventArgs)
        Dim item As TreeViewItem = CType(sender, TreeViewItem)

        ' If the item only contains dummy data 
        If item.Items.Count = 1 And item.Items(0) Is Nothing Then
            ' Claer dummy data
            item.Items.Clear()

            ' Get fullpath
            Dim fullpath As String = item.Tag

            ' Try and get files from the folder, ignoring any issues doing so
            Dim directoriesOfFullpath As String()
            Dim fileNames As String()

            Try
                ' Get directories
                directoriesOfFullpath = Directory.GetDirectories(fullpath)
                For Each directoryPath As String In directoriesOfFullpath
                    ' Create directory item
                    Dim subItem As TreeViewItem = New TreeViewItem()
                    With subItem
                        ' Set header as folder's name
                        .Header = GetFileFolderName(directoryPath)
                        ' And tag as fullpath
                        .Tag = directoryPath
                    End With

                    ' Add dummy item so we can expand folder
                    subItem.Items.Add(Nothing)

                    ' Handle expanding event
                    AddHandler subItem.Expanded, AddressOf Folder_Expanded

                    ' Adding subitem to current item
                    item.Items.Add(subItem)



                Next

                ' Get Files of current directory
                For Each directoryPath As String In directoriesOfFullpath
                    fileNames = Directory.GetFiles(directoryPath)
                    For Each filename As String In fileNames
                        ' Create File item
                        Dim fileItem As TreeViewItem = New TreeViewItem()
                        With fileItem
                            .Header = GetFileFolderName(filename)
                            .Tag = filename
                        End With

                        ' add this fileItem into parent
                        item.Items.Add(fileItem)
                    Next
                Next

            Catch ex As Exception
                ' Ignore the exception. It happened because the item does not have sub items (sub folders)
                'MsgBox(ex.Message, "Error when populating directories")
            End Try
        End If

    End Sub

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
