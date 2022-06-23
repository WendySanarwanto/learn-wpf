Class MainWindow

#Region "Constructor"
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = New WindowViewModel(Me)
    End Sub
#End Region

End Class
