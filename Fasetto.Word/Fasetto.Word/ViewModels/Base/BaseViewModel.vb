
Imports System.ComponentModel
Imports PropertyChanged

<AddINotifyPropertyChangedInterface>
Public MustInherit Class BaseViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Call this to fire <see cref="PropertyChanged"/> event.
    ''' </summary>
    ''' <param name="name"></param>
    Public Sub OnPropertyChanged(name As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
