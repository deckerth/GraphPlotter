Namespace Global.MathEvaluatorTest.ViewModels

    ' Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    Public Class BindableBase
        Implements INotifyPropertyChanged

        ' Occurs when a property value changes.
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overridable Sub OnPropertyChanged(ByVal PropertyName As String)
            ' Raise the event, and make this procedure
            ' overridable, should someone want to inherit from
            ' this class and override this behavior:
            Dim e = New PropertyChangedEventArgs(PropertyName)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

        ' Checks if a property already matches a desired value. Sets the property And
        ' notifies listeners only when necessary.

        ' <typeparam name="T">Type of the property.</typeparam>
        ' <param name="storage">Reference to a property with both getter And setter.</param>
        ' <param name="value">Desired value for the property.</param>
        ' <param name="propertyName">Name of the property used to notify listeners. This
        ' value Is optional And can be provided automatically when invoked from compilers that
        ' support CallerMemberName.</param>
        ' <returns>True if the value was changed, false if the existing value matched the
        ' desired value.</returns>

        Protected Function SetProperty(Of T)(ByRef storage As T, value As T, Optional propertyName As String = Nothing) As Boolean
            If Object.Equals(storage, value) Then
                Return False
            Else
                storage = value
                OnPropertyChanged(propertyName)
                Return True
            End If
        End Function


        'Private errorMessages As List(Of String)

        'Public Event ErrorListChanged As EventHandler(Of DataErrorsChangedEventArgs)

        'Protected Sub ClearErrors(Optional propertyName As String = Nothing)
        '    If Validate Then
        '        MyBase.RemoveErrors(propertyName)
        '    ElseIf errorMessages IsNot Nothing Then
        '        errorMessages = Nothing
        '        RaiseEvent ErrorListChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        '    End If
        'End Sub

        'Protected Sub AddErrorMessage(propertyName As String, errorMessage As Object)
        '    If Validate Then
        '        MyBase.AddError(propertyName, errorMessage)
        '    Else
        '        If errorMessages Is Nothing Then
        '            errorMessages = New List(Of String)
        '        End If
        '        errorMessages.Add(errorMessage)
        '        RaiseEvent ErrorListChanged(Me, New DataErrorsChangedEventArgs(propertyName))
        '    End If
        'End Sub

        'Public Function GetErrorMessages(propertyName As String) As IEnumerable
        '    If Validate Then
        '        Return GetErrors(propertyName)
        '    Else
        '        Return errorMessages
        '    End If
        'End Function

        'Public ReadOnly Property ErrorsExist As Boolean
        '    Get
        '        Return errorMessages IsNot Nothing
        '    End Get
        'End Property

    End Class


End Namespace
