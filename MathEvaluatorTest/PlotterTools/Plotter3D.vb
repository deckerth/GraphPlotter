
Imports MathEvaluatorTest.Domains
Imports Windows.UI.Text

Namespace Global.MathEvaluatorTest.PlotterTools

    Public Class Plotter3D

        Private _area As Canvas
        Public ReadOnly Property Area As Canvas
            Get
                Return _area
            End Get
        End Property

        Private _plotter2D As Plotter
        Public ReadOnly Property Plotter2D As Plotter
            Get
                Return _plotter2D
            End Get
        End Property

        Private _aspectAlpha As Double = 1.159 ' 66°
        Public Property AspectAlpha As Double
            Get
                Return _aspectAlpha
            End Get
            Set(value As Double)
                _aspectAlpha = value
            End Set
        End Property

        Private _xMin As Double
        Private _xMax As Double
        Private _yMin As Double
        Private _yMax As Double
        Private _zMin As Double
        Private _zMax As Double

        Private _lastY As Double
        Private _lastYDefined As Boolean = False
        Private _lastX As Double
        Private _lastXDefined As Boolean = False

        Public Sub New(plottingArea As Canvas)
            _area = plottingArea
            _plotter2D = New Plotter(plottingArea)
        End Sub

        Public Sub Clear()
            _plotter2D.Clear()
            _lastYDefined = False
            _lastXDefined = False
        End Sub

        Public Sub SetWorld(xMin As Double, xMax As Double, yMin As Double, yMax As Double, zMin As Double, zMax As Double)
            _xMin = xMin
            _xMax = xMax
            _yMin = yMin
            _yMax = yMax
            _zMin = zMin
            _zMax = zMax

            Dim min = ScaleVector(xMin, yMin, zMin)
            Dim max = ScaleVector(xMax, yMax, zMax)

            _plotter2D.SetWorld(min.X, max.X, min.Y, max.Y)
        End Sub

        Public Function ScaleVector(v As Vector) As Point
            Return ScaleVector(v.X, v.Y, v.Z)
        End Function

        Public Function ScaleVector(x As Double, y As Double, z As Double) As Point
            ' x(2) = x + y * sin((α)
            ' y(2) = z + y * cos((α)
            Return New Point(x + y * Math.Sin(_aspectAlpha), z + y * Math.Cos(_aspectAlpha))
        End Function

        Public Sub StartNewLine()
            _plotter2D.StartNewLine()
        End Sub

        Public Sub PlotAtX(x As Double, y As Double, z As Double)
            If Not Double.IsNaN(z) Then
                If _lastYDefined AndAlso y <> _lastY Then
                    _lastYDefined = False
                End If
                If Not _lastYDefined Then
                    _lastY = y
                    _lastYDefined = True
                    _plotter2D.StartNewLine()
                End If
                _plotter2D.Plot(x + y * Math.Sin(_aspectAlpha), z + y * Math.Cos(_aspectAlpha))
            End If
        End Sub

        Public Sub PlotAtY(x As Double, y As Double, z As Double)
            If Not Double.IsNaN(z) Then
                If _lastXDefined AndAlso x <> _lastX Then
                    _lastXDefined = False
                End If
                If Not _lastXDefined Then
                    _lastX = x
                    _lastXDefined = True
                    _plotter2D.StartNewLine()
                End If
                _plotter2D.Plot(x + y * Math.Sin(_aspectAlpha), z + y * Math.Cos(_aspectAlpha), App.SecondaryGraphColorBrush)
            End If
        End Sub

        Public Sub DrawCoordinates()
            Dim zero As New Vector
            If _xMin < 0 Then
                If _xMax > 0 Then
                    zero.X = 0
                Else
                    zero.X = _xMax
                End If
            Else
                zero.X = _xMin
            End If
            If _yMin < 0 Then
                If _yMax > 0 Then
                    zero.Y = 0
                Else
                    zero.Y = _yMax
                End If
            Else
                zero.Y = _yMin
            End If
            If _zMin < 0 Then
                If _zMax > 0 Then
                    zero.Z = 0
                Else
                    zero.Z = _zMax
                End If
            Else
                zero.Z = _zMin
            End If

            Dim scaledZero = ScaleVector(zero.X, zero.Y, zero.Z)
            Dim scaledXMin = ScaleVector(_xMin, zero.Y, zero.Z)
            Dim scaledXMax = ScaleVector(_xMax, zero.Y, zero.Z)
            Dim scaledYMin = ScaleVector(zero.X, _yMin, zero.Z)
            Dim scaledYMax = ScaleVector(zero.X, _yMax, zero.Z)
            Dim scaledZMin = ScaleVector(zero.X, zero.Y, _zMin)
            Dim scaledZMax = ScaleVector(zero.X, zero.Y, _zMax)

            _plotter2D.StartNewLine()
            _plotter2D.Plot(scaledXMin.X, scaledXMin.Y, App.CoordinateSystemColorBrush, 4)
            _plotter2D.Plot(scaledXMax.X, scaledXMax.Y, App.CoordinateSystemColorBrush, 4)

            _plotter2D.StartNewLine()
            _plotter2D.Plot(scaledYMin.X, scaledYMin.Y, App.CoordinateSystemColorBrush, 4)
            _plotter2D.Plot(scaledYMax.X, scaledYMax.Y, App.CoordinateSystemColorBrush, 4)

            _plotter2D.StartNewLine()
            _plotter2D.Plot(scaledZMin.X, scaledZMin.Y, App.CoordinateSystemColorBrush, 4)
            _plotter2D.Plot(scaledZMax.X, scaledZMax.Y, App.CoordinateSystemColorBrush, 4)

            Dim scaledZeroCanvasCoordinates = _plotter2D.ScalePoint(scaledZero.X, scaledZero.Y)
            Dim zeroTextPos As New Point()

            If scaledZeroCanvasCoordinates.X < _area.ActualWidth - 50 Then
                zeroTextPos.X = scaledZeroCanvasCoordinates.X + 10 ' Right to the Y-Axis
            Else
                zeroTextPos.X = scaledZeroCanvasCoordinates.X - 20 ' Left to the Y-Axis
            End If

            If scaledZeroCanvasCoordinates.Y < _area.ActualHeight - 50 Then
                zeroTextPos.Y = scaledZeroCanvasCoordinates.Y + 10 ' Under the X-Axis
            Else
                zeroTextPos.Y = scaledZeroCanvasCoordinates.Y - 20 ' Over the Y-Axis
            End If

            If zero.X.Equals(0) And zero.Y.Equals(0) Then
                Dim zeroText As New TextBlock
                zeroText.FontSize = 10
                zeroText.FontWeight = FontWeights.Black
                zeroText.Text = "0"
                Canvas.SetLeft(zeroText, zeroTextPos.X)
                Canvas.SetTop(zeroText, zeroTextPos.Y)
                _area.Children.Add(zeroText)
            End If

            Dim axesText As TextBlock

            If _area.ActualWidth - zeroTextPos.X > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                ' x
                Dim scaledXMaxCanvasCoordinates = _plotter2D.ScalePoint(scaledXMax.X, scaledXMax.Y)
                axesText = New TextBlock With {.FontSize = 12, .FontWeight = FontWeights.Black, .FontStyle = FontStyle.Italic, .Text = "x"}
                Canvas.SetLeft(axesText, scaledXMaxCanvasCoordinates.X - 10)
                Canvas.SetTop(axesText, zeroTextPos.Y)
                _area.Children.Add(axesText)
            End If

            ' y
            Dim scaledYMaxCanvasCoordinates = _plotter2D.ScalePoint(scaledYMax.X, scaledYMax.Y)
            axesText = New TextBlock With {.FontSize = 12, .FontWeight = FontWeights.Black, .FontStyle = FontStyle.Italic, .Text = "y"}
            Canvas.SetLeft(axesText, scaledYMaxCanvasCoordinates.X - 10)
            Canvas.SetTop(axesText, scaledYMaxCanvasCoordinates.Y + 10)
            _area.Children.Add(axesText)

            ' z
            If zeroTextPos.Y > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                Dim scaledZMaxCanvasCoordinates = _plotter2D.ScalePoint(scaledZMax.X, scaledZMax.Y)
                axesText = New TextBlock With {.FontSize = 12, .FontWeight = FontWeights.Black, .FontStyle = FontStyle.Italic, .Text = "z"}
                Canvas.SetLeft(axesText, zeroTextPos.X)
                Canvas.SetTop(axesText, scaledZMaxCanvasCoordinates.Y + 10)
                _area.Children.Add(axesText)
            End If
        End Sub

        Public Sub RenderXAreaAt(x As Double)
            Dim lowerFront As New Vector(x, _yMin, _zMin)
            Dim upperFront As New Vector(x, _yMin, _zMax)
            Dim lowerBack As New Vector(x, _yMax, _zMin)
            Dim upperBack As New Vector(x, _yMax, _zMax)
            Dim p1 = ScaleVector(lowerFront)
            Dim p2 = ScaleVector(upperFront)
            Dim p3 = ScaleVector(upperBack)
            Dim p4 = ScaleVector(lowerBack)
            _plotter2D.RenderPolygon(p1, p2, p3, p4)
        End Sub

        Public Sub RenderYAreaAt(y As Double)
            Dim lowerLeft As New Vector(_xMin, y, _zMin)
            Dim upperLeft As New Vector(_xMin, y, _zMax)
            Dim lowerRight As New Vector(_xMax, y, _zMin)
            Dim upperRight As New Vector(_xMax, y, _zMax)
            Dim p1 = ScaleVector(lowerLeft)
            Dim p2 = ScaleVector(upperLeft)
            Dim p3 = ScaleVector(upperRight)
            Dim p4 = ScaleVector(lowerRight)
            _plotter2D.RenderPolygon(p1, p2, p3, p4)
        End Sub

        Public Sub Hide3DArea()
            _plotter2D.HidePolygonArea()
        End Sub

        Public Function GetXPosition(x As Double) As Double
            Dim worldPoint = _plotter2D.GetWorldCoords(x, 0)

            ' x(2) = x + y * sin(α) => x = x(2) - y * sin(α)

            Return worldPoint.X
        End Function

        Public Function GetYPosition(y As Double) As Double
            Dim worldPoint = _plotter2D.GetWorldCoords(0, y)

            ' y(2) = z + y * cos(α) => y = ( y(2) - z ) / cos(α)

            Return worldPoint.Y / Math.Cos(AspectAlpha)
        End Function
    End Class

End Namespace
