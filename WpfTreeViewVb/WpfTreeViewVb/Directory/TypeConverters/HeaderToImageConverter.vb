Imports System.Globalization
Imports System.IO

<ValueConversion(GetType(DirectoryItemType), GetType(BitmapImage))>
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
        ' By default we set the ImageUri to FileImageUri
        Dim imageUri As String = FILE_IMAGE_URI
        Dim itemType As DirectoryItemType = CType(value, DirectoryItemType)

        Select Case itemType
            Case DirectoryItemType.Drive
                imageUri = DRIVE_IMAGE_URI
            Case DirectoryItemType.Folder
                imageUri = FOLDER_IMAGE_URI
        End Select

        Return New BitmapImage(New Uri($"pack://application:,,,/{imageUri}"))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
