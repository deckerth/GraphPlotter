
Public Class Bisection

    Public Shared Function FindRoot(fun As MathExpression, x1 As Double, x2 As Double, precision As Double, Optional maxIterations As Double = 100)

        Dim y1 As Double = fun.EvaluateAt(x1)
        Dim y2 As Double = fun.EvaluateAt(x2)
        Dim min As Double = x1
        Dim max As Double = x2

        If min > max Then
            max = x1
            min = x2
        End If

        If y1 = 0 Then
            Return x1
        End If
        If y2 = 0 Then
            Return x2
        End If
        If Double.IsNaN(y1) Or Double.IsNaN(y2) Then
            Return Double.NaN
        End If
        If min = max Then
            Return Double.NaN
        End If
        If y1 < 0 And y2 > 0 Or y1 > 0 And y2 < 0 Then
            Return FindSingularRoot(fun, min, max, precision, maxIterations)
        Else
            Return FindExtremum(fun, min, max, precision, maxIterations)
        End If

    End Function


    Public Shared Function FindSingularRoot(fun As MathExpression, x1 As Double, x2 As Double, precision As Double, Optional maxIterations As Double = 100)

        Dim y1 As Double = fun.EvaluateAt(x1)
        Dim y2 As Double = fun.EvaluateAt(x2)
        Dim iterCount As Integer = 0
        Dim relation As Integer = Math.Sign(y2 - y1)

        Do
            If y1 = 0 Then
                Return x1
            End If
            If y2 = 0 Then
                Return x2
            End If
            Dim midPoint As Double = x1 + (x2 - x1) / 2
            If x2 - x1 <= precision Then
                Return midPoint
            End If
            If iterCount > maxIterations Then
                Return Double.NaN
            End If
            Dim y As Double = fun.EvaluateAt(midPoint)
            If relation * y <= 0 Then
                x1 = midPoint
                y1 = y
            Else
                x2 = midPoint
                y2 = y
            End If
            iterCount = iterCount + 1
        Loop

    End Function

    Public Shared Function FindExtremum(fun As MathExpression, x1 As Double, x2 As Double, precision As Double, Optional maxIterations As Double = 100)

        Dim epsilon As Double = (x2 - x1) / 1000
        Dim y1a As Double = fun.EvaluateAt(x1)
        Dim y1b As Double = fun.EvaluateAt(x1 + epsilon)
        Dim m1 As Double = Math.Sign(y1b - y1a)
        Dim y2a As Double = fun.EvaluateAt(x2 - epsilon)
        Dim y2b As Double = fun.EvaluateAt(x2)
        Dim m2 As Double = Math.Sign(y2b - y2a)

        Dim iterCount As Integer = 0

        Do
            If m1 = 0 Then
                Return x1
            End If
            If m2 = 0 Then
                Return x2
            End If
            If m1 = m2 Then
                Return Double.NaN
            End If
            Dim midPoint As Double = x1 + (x2 - x1) / 2
            If iterCount > maxIterations Or x2 - x1 <= precision Then
                Return midPoint
            End If

            Dim ya As Double = fun.EvaluateAt(midPoint)
            Dim yb As Double = fun.EvaluateAt(midPoint + epsilon)
            Dim m As Double = Math.Sign(yb - ya)
            If m = 0 Then
                Return midPoint
            ElseIf m = m1 Then
                x1 = midPoint
            Else
                x2 = midPoint
            End If

            epsilon = (x2 - x1) / 1000

            iterCount = iterCount + 1
        Loop

    End Function

End Class
