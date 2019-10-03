Imports Windows.UI
Imports Windows.UI.Xaml.Shapes

Namespace Global.MathEvaluatorTest.PlotterTools

    Public Class Plotter

        Private _area As Canvas
        Public ReadOnly Property Area As Canvas
            Get
                Return _area
            End Get
        End Property

        Private _xMin As Double
        Private _xMax As Double
        Private _yMin As Double
        Private _yMax As Double

        Private _xScale As Double
        Private _xOffset As Double
        Private _yScale As Double
        Private _yOffset As Double

        Private _lastPoint As Point
        Private _lastPointDefined As Boolean = False

        Private _cursorLine As Line
        Private _areaRectangle As Rectangle
        Private _areaPolygon As Polygon

        Public Sub New(plottingArea As Canvas)
            _area = plottingArea
        End Sub

        Public Sub Clear()
            If _area IsNot Nothing Then
                _area.Children.Clear()
            End If
            _cursorLine = Nothing
            _areaRectangle = Nothing
            _lastPointDefined = False
        End Sub

        Public Sub StartNewLine()
            _lastPointDefined = False
        End Sub

        Public Sub SetWorld(xMin As Double, xMax As Double, yMin As Double, yMax As Double)
            _xMin = xMin
            _xMax = xMax
            _yMin = yMin
            _yMax = yMax

            Dim xSize As Double = _area.ActualWidth
            Dim ySize As Double = _area.ActualHeight

            '(0,0) (0,1) (0,2)...
            '(1,0) (1,1) (1,2)...
            '(2,0) (2,1) (2,2)...

            Dim xRange = xMax - xMin
            Dim yRange = yMax - yMin

            _xScale = xSize / xRange
            _yScale = ySize / yRange

            ' X: -1..1 -> 0..400
            ' Y: -1..1 -> 400..0

            _xOffset = -1 * xMin * _xScale
            _yOffset = -1 * yMin * _yScale
        End Sub

        Public Function ScalePoint(x As Double, y As Double) As Point
            Return New Point(x * _xScale + _xOffset, _area.ActualHeight - (y * _yScale + _yOffset))
        End Function

        Public Function GetWorldCoords(x As Double, y As Double) As Point
            Return New Point((x - _xOffset) / _xScale, -1 * (y - _area.ActualHeight + _yOffset) / _yScale)
        End Function

        Public Sub Plot(x As Double, y As Double)
            Plot(x, y, App.GraphColorBrush)
        End Sub

        Public Sub Plot(x As Double, y As Double, brush As Brush, Optional thickness As Double = 2)
            If Not Double.IsNaN(y) Then
                Dim currentPoint = ScalePoint(x, y)
                If _lastPointDefined Then
                    Dim line As New Line With {
                .X1 = _lastPoint.X,
                .X2 = currentPoint.X,
                .Y1 = _lastPoint.Y,
                .Y2 = currentPoint.Y,
                .Stroke = brush,
                .StrokeThickness = thickness
            }
                    _area.Children.Add(line)
                End If
                If Not Double.IsNaN(currentPoint.X) AndAlso Not Double.IsNaN(currentPoint.Y) Then
                    _lastPoint = currentPoint
                    _lastPointDefined = True
                End If
            End If
        End Sub

        Public Sub DrawCoordinates()
            Dim zero As New Point()
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
            Dim scaledZero = ScalePoint(zero.X, zero.Y)
            Dim xAxis As New Line With {
                .X1 = 0,
                .X2 = _area.ActualWidth,
                .Y1 = scaledZero.Y,
                .Y2 = scaledZero.Y,
                .Stroke = App.CoordinateSystemColorBrush,
                .StrokeThickness = 4
            }
            Dim yAxis As New Line With {
                .X1 = scaledZero.X,
                .X2 = scaledZero.X,
                .Y1 = 0,
                .Y2 = _area.ActualHeight,
                .Stroke = App.CoordinateSystemColorBrush,
                .StrokeThickness = 4
            }

            _area.Children.Add(xAxis)
            _area.Children.Add(yAxis)

            Dim zeroTextPos As New Point()

            If scaledZero.X < _area.ActualWidth - 50 Then
                zeroTextPos.X = scaledZero.X + 10 ' Right to the Y-Axis
            Else
                zeroTextPos.X = scaledZero.X - 20 ' Left to the Y-Axis
            End If

            If scaledZero.Y < _area.ActualHeight - 50 Then
                zeroTextPos.Y = scaledZero.Y + 10 ' Under the X-Axis
            Else
                zeroTextPos.Y = scaledZero.Y - 20 ' Over the Y-Axis
            End If

            If zero.X.Equals(0) And zero.Y.Equals(0) Then
                Dim zeroText As New TextBlock
                zeroText.FontSize = 10
                zeroText.Text = "0"
                Canvas.SetLeft(zeroText, zeroTextPos.X)
                Canvas.SetTop(zeroText, zeroTextPos.Y)
                _area.Children.Add(zeroText)
            End If

            If _area.ActualHeight - zeroTextPos.Y > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                ' Show lower bound of y-Axis
                Dim lowerYBoundText As New TextBlock
                lowerYBoundText.FontSize = 10
                lowerYBoundText.Text = _yMin.ToString
                Canvas.SetLeft(lowerYBoundText, zeroTextPos.X)
                Canvas.SetTop(lowerYBoundText, _area.ActualHeight - 20)
                _area.Children.Add(lowerYBoundText)
            End If

            If zeroTextPos.Y > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                ' Show upper bound of y-Axis
                Dim upperYBoundText As New TextBlock
                upperYBoundText.FontSize = 10
                upperYBoundText.Text = _yMax.ToString
                Canvas.SetLeft(upperYBoundText, zeroTextPos.X)
                Canvas.SetTop(upperYBoundText, 5)
                _area.Children.Add(upperYBoundText)
            End If

            If _area.ActualWidth - zeroTextPos.X > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                ' Show upper bound of x-Axis
                Dim upperXBoundText As New TextBlock
                upperXBoundText.FontSize = 10
                upperXBoundText.Text = _xMax.ToString
                Canvas.SetLeft(upperXBoundText, _area.ActualWidth - 20)
                Canvas.SetTop(upperXBoundText, zeroTextPos.Y)
                _area.Children.Add(upperXBoundText)
            End If

            If zeroTextPos.X > 50 OrElse Not zero.X.Equals(0) OrElse Not zero.Y.Equals(0) Then
                ' Show lower bound of x-Axis
                Dim lowerXBoundText As New TextBlock
                lowerXBoundText.FontSize = 10
                lowerXBoundText.Text = _xMin.ToString
                Canvas.SetLeft(lowerXBoundText, 5)
                Canvas.SetTop(lowerXBoundText, zeroTextPos.Y)
                _area.Children.Add(lowerXBoundText)
            End If
        End Sub

        Public Sub RenderCursorAt(x As Double)
            If _cursorLine IsNot Nothing Then
                _area.Children.Remove(_cursorLine)
            End If
            _cursorLine = New Line With {
                .X1 = x,
                .X2 = x,
                .Y1 = 0,
                .Y2 = _area.ActualHeight,
                .Stroke = App.CoordinateSystemColorBrush,
                .StrokeThickness = 1
            }
            _area.Children.Add(_cursorLine)
        End Sub

        Public Sub HideCursor()
            If _cursorLine IsNot Nothing Then
                _area.Children.Remove(_cursorLine)
                _cursorLine = Nothing
            End If
        End Sub

        Public Sub RenderAreaAt(x1 As Double, x2 As Double)
            HideArea()
            If x2 = x1 Then
                Return
            End If
            Dim min As Double = x1
            Dim max As Double = x2
            If x2 < x1 Then
                min = x2
                max = x1
            End If
            _areaRectangle = New Rectangle With {
        .Width = max - min,
        .Height = _area.ActualHeight,
        .Fill = App.AreaColorBrush,
        .Opacity = 0.5}
            Canvas.SetLeft(_areaRectangle, min)
            Canvas.SetTop(_areaRectangle, 0.0)
            _area.Children.Add(_areaRectangle)
        End Sub

        Public Sub HideArea()
            If _areaRectangle IsNot Nothing Then
                _area.Children.Remove(_areaRectangle)
                _areaRectangle = Nothing
            End If
        End Sub

        Public Sub RenderPolygon(p1 As Point, p2 As Point, p3 As Point, p4 As Point)
            Dim sp1 As Point = ScalePoint(p1.X, p1.Y)
            Dim sp2 As Point = ScalePoint(p2.X, p2.Y)
            Dim sp3 As Point = ScalePoint(p3.X, p3.Y)
            Dim sp4 As Point = ScalePoint(p4.X, p4.Y)

            HidePolygonArea()
            _areaPolygon = New Polygon With {
        .Fill = App.AreaColorBrush,
        .Opacity = 0.5}
            _areaPolygon.Points.Add(sp1)
            _areaPolygon.Points.Add(sp2)
            _areaPolygon.Points.Add(sp3)
            _areaPolygon.Points.Add(sp4)
            _area.Children.Add(_areaPolygon)
        End Sub

        Public Sub HidePolygonArea()
            If _areaPolygon IsNot Nothing Then
                _area.Children.Remove(_areaPolygon)
                _areaPolygon = Nothing
            End If
        End Sub

    End Class

End Namespace


