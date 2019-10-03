Namespace Global.MathEvaluatorTest.ValueConverters

    Public Class BooleanToVisibilityConverter
        Implements IValueConverter

        ' <summary>
        ' Converts a Boolean to a Visibility enumeration value.
        ' </summary>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If targetType.Equals(GetType(Windows.UI.Xaml.Visibility)) Then
                Dim b As Boolean = DirectCast(value, Boolean)
                If parameter IsNot Nothing Then
                    Dim invert As Boolean = System.Convert.ToBoolean(parameter)
                    If invert Then
                        b = Not b
                    End If
                End If
                If b Then
                    Return Visibility.Visible
                Else
                    Return Visibility.Collapsed
                End If
            Else
                Throw New ArgumentException("Unsuported type {0}", targetType.FullName)
            End If
        End Function

        ' <summary>
        ' No need to implement converting back on a one-way binding.
        ' </summary>
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
