Namespace Global.MathEvaluatorTest.ValueConverters

    Public Class BooleanOperator

        Public Shared Function OpAnd(a As Boolean, b As Boolean) As Boolean
            Return a AndAlso b
        End Function

        Public Shared Function OpAndToVis(a As Boolean, b As Boolean) As Visibility
            If a AndAlso b Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Function

        Public Shared Function OpAndNot(a As Boolean, b As Boolean) As Boolean
            Return a AndAlso Not b
        End Function

        Public Shared Function OpOr(a As Boolean, b As Boolean) As Boolean
            Return a OrElse b
        End Function

        Public Shared Function OpNot(a As Boolean) As Boolean
            Return Not a
        End Function

        Public Shared Function OpNotNullAndNot(a As Object, b As Boolean) As Boolean
            Return a IsNot Nothing AndAlso Not b
        End Function

    End Class

End Namespace
