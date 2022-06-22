Imports System.IO

Class MainWindow
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        Me.DataContext = New DirectoryStructureViewModel()
    End Sub

End Class
