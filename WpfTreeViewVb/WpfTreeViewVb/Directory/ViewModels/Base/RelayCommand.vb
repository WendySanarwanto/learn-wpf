Public Class RelayCommand
    Implements ICommand

    Private ReadOnly _action As Action

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub New(action As Action)
        Me._action = action
    End Sub

    ''' <summary>
    ''' Executes the command action
    ''' </summary>
    ''' <param name="parameter"></param>
    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Me._action()
    End Sub

    ''' <summary>
    ''' A relay command can always execute
    ''' </summary>
    ''' <param name="parameter"></param>
    ''' <returns></returns>
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
