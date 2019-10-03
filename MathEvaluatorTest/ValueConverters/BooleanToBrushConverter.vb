Imports Windows.UI

Namespace Global.MathEvaluatorTest.ValueConverters

    Public Class BooleanToBrushConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If targetType.Equals(GetType(Brush)) Then
                Dim b As Boolean = DirectCast(value, Boolean)
                If parameter IsNot Nothing Then
                    Dim invert As Boolean = System.Convert.ToBoolean(parameter)
                    If invert Then
                        b = Not b
                    End If
                End If
                If b Then
                    Return App.StandardForegroundBrush
                Else
                    Return App.ErrorColorBrush
                End If
            Else
                Throw New ArgumentException("Unsuported type {0}", targetType.FullName)
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
