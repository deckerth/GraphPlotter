Imports ExpressionEvaluator
Imports MathEvaluatorTest.Domains

Namespace Global.MathEvaluatorTest.PlotterTools

    Public Class FunctionPlotter3D

        Private _plotter3D As Plotter3D = Nothing
        Private _expression As MathExpression
        Public ReadOnly Property Expression As MathExpression
            Get
                Return _expression
            End Get
        End Property

        Public Sub ClearExpression()
            _expression = Nothing
            Clear()
        End Sub

        Public Property AspectAlpha As Double
            Get
                If _plotter3D Is Nothing Then
                    Return 0
                End If
                Return _plotter3D.AspectAlpha
            End Get
            Set(value As Double)
                If _plotter3D IsNot Nothing Then
                    _plotter3D.AspectAlpha = value
                    If XPoints.Count > 0 Then
                        PlotGraph()
                    End If
                End If
            End Set
        End Property

        Public Sub SetPlottingArea(plottingArea As Canvas)
            If _plotter3D Is Nothing OrElse Not _plotter3D.Area.Equals(plottingArea) Then
                _plotter3D = New Plotter3D(plottingArea)
            End If
        End Sub

        Public Function IsValidExpression(fun As String) As Boolean
            Dim convertedFun = fun.Replace("x", "[X]").Replace("y", "[Y]")
            Return MathExpression.IsValid(convertedFun)
        End Function

        Dim XPoints As New List(Of Vector)
        Dim YPoints As New List(Of Vector)

        Public Sub PlotFunction(fun As String, xMin As Double, xMax As Double, yMin As Double, yMax As Double, n As Integer)
            If _plotter3D Is Nothing Then
                Return
            End If

            XPoints.Clear()
            YPoints.Clear()
            Dim minZ = Double.MaxValue
            Dim maxZ = Double.MinValue
            Dim convertedFun = fun.Replace("x", "[X]").Replace("y", "[Y]")
            _expression = New MathExpression(convertedFun)
            Dim m = n * 10 '_plotter3D.Area.Width
            Dim xDelta = (xMax - xMin) / m
            Dim yDelta = (yMax - yMin) / n
            Dim x As Double
            Dim y = yMin
            For l = 1 To n
                x = xMin
                For i = 1 To m
                    Dim z = _expression.EvaluateAt(x, y)
                    If z < minZ Then
                        minZ = z
                    End If
                    If z > maxZ Then
                        maxZ = z
                    End If
                    XPoints.Add(New Vector(x, y, z))
                    x = x + xDelta
                Next
                y = y + yDelta
            Next

            If minZ = maxZ Then ' Constant function
                minZ = minZ - 1
                maxZ = maxZ + 1
            End If

            _plotter3D.SetWorld(xMin, xMax, yMin, yMax, minZ, maxZ)

            yDelta = (yMax - yMin) / m
            xDelta = (xMax - xMin) / n
            x = xMin
            For l = 1 To n
                y = yMin
                For i = 1 To m
                    Dim z = _expression.EvaluateAt(x, y)
                    YPoints.Add(New Vector(x, y, z))
                    y = y + yDelta
                Next
                x = x + xDelta
            Next
            PlotGraph()
        End Sub

        Public Sub PlotGraph()
            If _plotter3D Is Nothing Then
                Return
            End If

            _plotter3D.Clear()
            Dim iter = XPoints.GetEnumerator
            While iter.MoveNext()
                _plotter3D.PlotAtX(iter.Current.X, iter.Current.Y, iter.Current.Z)
            End While
            iter = YPoints.GetEnumerator
            While iter.MoveNext()
                _plotter3D.PlotAtY(iter.Current.X, iter.Current.Y, iter.Current.Z)
            End While
            _plotter3D.DrawCoordinates()
        End Sub

        Public Sub Clear()
            If _plotter3D IsNot Nothing Then
                _plotter3D.Clear()
            End If
        End Sub

        Public Function EvaluateExpression(x As Double, y As Double) As Double
            If _expression IsNot Nothing Then
                Return _expression.EvaluateAt(x, y)
            Else
                Return Double.NaN
            End If
        End Function

        Public Function GetArea() As Canvas
            If _plotter3D Is Nothing Then
                Return Nothing
            End If
            Return _plotter3D.Area
        End Function

        Public Function IsExpressionDefined() As Boolean
            Return _expression IsNot Nothing
        End Function

        Public Sub RenderXAreaAt(x As Double)
            If _plotter3D IsNot Nothing Then
                _plotter3D.RenderXAreaAt(x)
            End If
        End Sub

        Public Sub RenderYAreaAt(y As Double)
            If _plotter3D IsNot Nothing Then
                _plotter3D.RenderYAreaAt(y)
            End If
        End Sub

        Public Sub Hide3DArea()
            If _plotter3D IsNot Nothing Then
                _plotter3D.Hide3DArea()
            End If
        End Sub

        Public Function GetXPosition(x As Double) As Double
            If _plotter3D IsNot Nothing Then
                Return _plotter3D.GetXPosition(x)
            Else
                Return 0
            End If
        End Function

        Public Function GetYPosition(y As Double) As Double
            If _plotter3D IsNot Nothing Then
                Return _plotter3D.GetYPosition(y)
            Else
                Return 0
            End If
        End Function

    End Class

End Namespace
