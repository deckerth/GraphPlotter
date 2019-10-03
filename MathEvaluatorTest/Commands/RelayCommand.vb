Namespace Global.MathEvaluatorTest.Commands

    ' <summary>
    ' A generic relay command that allows binding commands from the UI.
    ' </summary>
    ' <typeparam name="T">The target type.</typeparam>
    'Public Class RelayCommand<T> : BaseCommand, ICommand

    Public Class RelayCommand(Of T)
        Inherits BaseCommand
        Implements ICommand

        Private _action As Action(Of T)

        ' <summary>
        ' Creates a New command that can always execute.
        ' </summary>
        ' <param name="action">The execution logic.</param>
        Public Sub New(action As Action(Of T))
            Me.New(action, Nothing)
        End Sub

        ' <summary>
        ' Creates a New command.
        ' </summary>
        ' <param name="action">The execution logic.</param>
        ' <param name="canExecute">The execution status logic.</param>
        Public Sub New(action As Action(Of T), canExecute As Func(Of Boolean))
            MyBase.New(canExecute)

            If action Is Nothing Then
                Throw New ArgumentNullException(NameOf(action))
            End If

            _action = action
        End Sub

        ' <summary>
        ' Executes the <see cref="RelayCommand" /> on the current command target.
        ' </summary>
        ' <param name="parameter">
        ' Data used by the command. If the command does Not require data to be passed, this object can be set to null.
        ' </param>
        Public Overrides Sub Execute(parameter As Object) Implements ICommand.Execute
            If TypeOf parameter Is ItemClickEventArgs Then
                parameter = DirectCast(parameter, ItemClickEventArgs).ClickedItem
            End If

            If TypeOf parameter Is T Then
                _action(DirectCast(parameter, T))
            End If
        End Sub

    End Class

    ' <summary>
    ' A command whose sole purpose Is to relay its functionality
    ' to other objects by invoking delegates.
    ' The default return value for the CanExecute method Is 'true'.
    ' </summary>
    Public Class RelayCommand
        Inherits BaseCommand
        Implements ICommand

        Private _action As Action

        ' <summary>
        ' Creates a New command that can always execute.
        ' </summary>
        ' <param name="action">The execution logic.</param>
        Public Sub New(action As Action)
            Me.New(action, Nothing)
        End Sub

        ' <summary>
        ' Creates a New command.
        ' </summary>
        ' <param name="action">The execution logic.</param>
        ' <param name="canExecute">The execution status logic.</param>
        Public Sub New(action As Action, canExecute As Func(Of Boolean))
            MyBase.New(canExecute)
            If action Is Nothing Then
                Throw New ArgumentNullException(NameOf(action))
            End If
            _action = action
        End Sub

        ' <summary>
        ' Executes the <see cref="RelayCommand" /> on the current command target.
        ' </summary>
        ' <param name="parameter">
        ' Data used by the command. If the command does Not require data to be passed, this object can be set to null.
        ' </param>
        Public Overrides Sub Execute(parameter As Object)
            _action()
        End Sub

    End Class

End Namespace
