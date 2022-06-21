Imports System.Globalization
Imports System.IO

<ValueConversion(GetType(String), GetType(BitmapImage))>
Public Class HeaderToImageConverter
    Implements IValueConverter
    Private Const DRIVE_IMAGE_URI As String = "Images/drive.png"
    Private Const FILE_IMAGE_URI As String = "Images/file.png"
    Private Const FOLDER_IMAGE_URI As String = "Images/folder-closed.png"
    Public Shared instance As HeaderToImageConverter = New HeaderToImageConverter()

    ''' <summary>
    ''' Convert a full path into a specific image type of a drive, folder or file
    ''' </summary>
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        ' Get the full path
        Dim path As String = CType(value, String)

        ' If the path is null, ignore
        If (path Is Nothing) Then
            Return Nothing
        End If

        ' Get the name of file/folder 
        Dim fileFolderName As String = MainWindow.GetFileFolderName(path)

        ' By default we set the ImageUri to FileImageUri
        Dim imageUri As String = FILE_IMAGE_URI

        ' If the fileFolderName is blank , we presume it is drive 
        If (String.IsNullOrEmpty(fileFolderName)) Then
            imageUri = DRIVE_IMAGE_URI
        ElseIf (New FileInfo(path).Attributes.HasFlag(FileAttributes.Directory)) Then
            imageUri = FOLDER_IMAGE_URI
        End If

        Return New BitmapImage(New Uri($"pack://application:,,,/{imageUri}"))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
