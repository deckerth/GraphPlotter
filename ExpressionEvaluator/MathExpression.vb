Imports NCalc2

Public Class MathExpression

    'NCalc documentation: https://archive.codeplex.com/?p=ncalc

    Private _expression As Expression
    Private _formular As String

    Public Sub New(ex As String)
        _formular = ex
        _expression = New Expression(ex)
    End Sub

    Public Sub New(formular As String, ex As Expression)
        _formular = formular
        _expression = ex
    End Sub

    Public Function ConstantXValue(x As Double, Optional xVariable As String = "X") As MathExpression
        Dim funFixX = New Expression(_formular)
        funFixX.Parameters(xVariable) = x
        Return New MathExpression(_formular, funFixX)
    End Function

    Public Function ConstantYValue(y As Double, Optional yVariable As String = "Y") As MathExpression
        Dim funFixY = New Expression(_formular)
        funFixY.Parameters(yVariable) = y
        Return New MathExpression(_formular, funFixY)
    End Function

    Public Shared Function IsValid(ex As String) As Boolean
        Try
            Dim _test As New MathExpression(ex)
            Try
                _test.EvaluateAt(0)
                Return True
            Catch ex2 As Exception
                Try
                    _test.EvaluateAt(0, 0)
                    Return True
                Catch ex3 As Exception
                    Return False
                End Try
            End Try
        Catch e As Exception
            Return False
        End Try
    End Function

    Public Function Evaluate() As Object
        Return _expression.Evaluate()
    End Function

    Public Function EvaluateAt(x As Double, Optional variable As String = "X") As Object
        _expression.Parameters(variable) = x
        Return _expression.Evaluate()
    End Function

    Public Function EvaluateAt(x As Double, y As Double, Optional xVariable As String = "X", Optional yVariable As String = "Y") As Object
        _expression.Parameters(xVariable) = x
        _expression.Parameters(yVariable) = y
        Return _expression.Evaluate()
    End Function

End Class
