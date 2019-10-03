Namespace Global.MathEvaluatorTest.Commands

    Public Class BaseCommand
        Implements ICommand

        Private _canExecute As Func(Of Boolean)

        ' <summary>
        ' Intializes BaseCommand for relay commands.
        ' </summary>
        ' <param name="canExecute">If set to true, command can be executed. Otherwise, false.</param>

        Protected Sub New(canExecute As Func(Of Boolean))
            _canExecute = canExecute
        End Sub

        ' <summary>
        ' Determines whether this <see cref="RelayCommand" /> can execute in its current state.
        ' </summary>
        ' <param name="parameter">
        ' Data used by the command. If the command does Not require data to be passed, this object can be set to null.
        ' </param>
        ' <returns>true if this command can be executed; otherwise, false.</returns>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return _canExecute Is Nothing OrElse _canExecute()
        End Function

        ' Raised when RaiseCanExecuteChanged is called.
        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        ' <summary>
        ' Method used to raise the <see cref="CanExecuteChanged" /> event
        ' to indicate that the return value of the <see cref="CanExecute" />
        ' method has changed.
        ' </summary>
        Public Sub RaiseCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End Sub

        Public Overridable Sub Execute(parameter As Object) Implements ICommand.Execute
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace
