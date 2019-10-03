Imports ExpressionEvaluator

Namespace Global.MathEvaluatorTest.PlotterTools

    Public Class FunctionPlotter

        Private _plotter As Plotter = Nothing
        Private _expression As MathExpression
        Private _variable As String

        Public ReadOnly Property Expression As MathExpression
            Get
                Return _expression
            End Get
        End Property

        Public Sub SetPlottingArea(plottingArea As Canvas)
            If _plotter Is Nothing OrElse Not _plotter.Area.Equals(plottingArea) Then
                _plotter = New Plotter(plottingArea)
            End If
        End Sub

        Public Function IsValidExpression(fun As String) As Boolean
            Dim convertedFun = fun.Replace("x", "[X]")
            Return MathExpression.IsValid(convertedFun)
        End Function

        Public Sub SetExpression(fun As String, Optional variabe As String = "X")
            Dim convertedFun = fun.Replace("x", "[X]")
            _variable = variabe
            _expression = New MathExpression(convertedFun)
        End Sub

        Public Sub SetExpression(ex As MathExpression, variable As String)
            _expression = ex
            _variable = variable
        End Sub

        Public Sub ClearExpression()
            _expression = Nothing
            Clear()
        End Sub

        Public Sub PlotFunction(xMin As Double, xMax As Double)
            If _plotter Is Nothing OrElse Not IsExpressionDefined() Then
                Return
            End If

            Dim values As New List(Of Point)
            Dim minY = Double.MaxValue
            Dim maxY = Double.MinValue
            Dim delta = (xMax - xMin) / _plotter.Area.ActualWidth
            Dim x = xMin
            For i = 0 To _plotter.Area.ActualWidth - 1
                Dim y = _expression.EvaluateAt(x, _variable)
                If y < minY Then
                    minY = y
                End If
                If y > maxY Then
                    maxY = y
                End If
                values.Add(New Point(x, y))
                x = x + delta
            Next

            If minY = maxY Then ' Constant function
                minY = minY - 1
                maxY = maxY + 1
            End If

            _plotter.SetWorld(xMin, xMax, minY, maxY)

            Dim iter = values.GetEnumerator
            While iter.MoveNext()
                _plotter.Plot(iter.Current.X, iter.Current.Y)
            End While

            _plotter.DrawCoordinates()
        End Sub

        Public Sub Clear()
            If _plotter IsNot Nothing Then
                _plotter.Clear()
            End If
        End Sub

        Public Function GetWorldCoords(x As Double, y As Double) As Point
            If _plotter IsNot Nothing Then
                Return _plotter.GetWorldCoords(x, y)
            Else
                Return New Point()
            End If
        End Function

        Public Function EvaluateExpression(x As Double) As Double
            If _expression IsNot Nothing Then
                Return _expression.EvaluateAt(x, _variable)
            Else
                Return Double.NaN
            End If
        End Function

        Public Sub RenderCursorAt(x As Double)
            If _plotter IsNot Nothing Then
                _plotter.RenderCursorAt(x)
            End If
        End Sub

        Public Sub RenderCursor(x As Double)
            If IsExpressionDefined() AndAlso _plotter IsNot Nothing Then
                Dim scaledPoint = _plotter.ScalePoint(x, 0)
                _plotter.RenderCursorAt(scaledPoint.X)
            End If
        End Sub

        Public Sub HideCursor()
            If _plotter IsNot Nothing Then
                _plotter.HideCursor()
            End If
        End Sub

        Public Sub RenderAreaAt(x1 As Double, x2 As Double)
            If _plotter IsNot Nothing Then
                _plotter.RenderAreaAt(x1, x2)
            End If
        End Sub

        Public Sub RenderArea(x1 As Double, x2 As Double)
            If IsExpressionDefined() AndAlso _plotter IsNot Nothing Then
                Dim scaledX1 = _plotter.ScalePoint(x1, 0)
                Dim scaledX2 = _plotter.ScalePoint(x2, 0)
                _plotter.RenderAreaAt(scaledX1.X, scaledX2.X)
            End If
        End Sub

        Public Sub HideArea()
            If _plotter IsNot Nothing Then
                _plotter.HideArea()
            End If
        End Sub

        Public Function GetArea() As Canvas
            If _plotter Is Nothing Then
                Return Nothing
            End If
            Return _plotter.Area
        End Function

        Public Function IsExpressionDefined() As Boolean
            Return _expression IsNot Nothing
        End Function
    End Class

End Namespace
