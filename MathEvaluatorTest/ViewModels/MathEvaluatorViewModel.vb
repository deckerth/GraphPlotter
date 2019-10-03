Imports ExpressionEvaluator
Imports MathEvaluatorTest.Commands
Imports MathEvaluatorTest.PlotterTools

Namespace Global.MathEvaluatorTest.ViewModels

    Public Class MathEvaluatorViewModel
        Inherits BindableBase

        Public Shared Current As MathEvaluatorViewModel

        Private _expressionIsValid As Boolean = True
        Public Property ExpressionIsValid As Boolean
            Get
                Return _expressionIsValid
            End Get
            Set(value As Boolean)
                If value <> _expressionIsValid Then
                    SetProperty(Of Boolean)(_expressionIsValid, value)
                End If
            End Set
        End Property

        Private _expression As String = "Sin(x)"
        Public Property Expression As String
            Get
                Return _expression
            End Get
            Set(value As String)
                If Not value.Equals(_expression) Then
                    SetProperty(Of String)(_expression, value)
                    If _mode3D Then
                        ExpressionIsValid = currentPlotter3D.IsValidExpression(_expression)
                    Else
                        ExpressionIsValid = currentPlotter.IsValidExpression(_expression)
                    End If
                End If
            End Set
        End Property

        Private _min As String = "-1"
        Public Property Min As String
            Get
                Return _min
            End Get
            Set(value As String)
                If Not value.Equals(_min) Then
                    SetProperty(Of String)(_min, value)
                End If
            End Set
        End Property

        Private _max As String = "1"
        Public Property Max As String
            Get
                Return _max
            End Get
            Set(value As String)
                If Not value.Equals(_max) Then
                    SetProperty(Of String)(_max, value)
                End If
            End Set
        End Property

        Private _minY As String = "-1"
        Public Property MinY As String
            Get
                Return _minY
            End Get
            Set(value As String)
                If Not value.Equals(_minY) Then
                    SetProperty(Of String)(_minY, value)
                End If
            End Set
        End Property

        Private _maxY As String = "1"
        Public Property MaxY As String
            Get
                Return _maxY
            End Get
            Set(value As String)
                If Not value.Equals(_maxY) Then
                    SetProperty(Of String)(_maxY, value)
                End If
            End Set
        End Property

        Private _aspectAlpha As Double = 66
        Public Property AspectAlpha As Double
            Get
                Return _aspectAlpha
            End Get
            Set(value As Double)
                If value <> _aspectAlpha Then
                    SetProperty(Of Double)(_aspectAlpha, value)
                    currentPlotter3D.AspectAlpha = value / 180 * Math.PI
                End If
                _aspectAlpha = value
            End Set
        End Property

        Private _n As String = "10"
        Public Property N As String
            Get
                Return _n
            End Get
            Set(value As String)
                SetProperty(Of String)(_n, value)
            End Set
        End Property

        Private _xValue As String
        Public Property XValue As String
            Get
                Return _xValue
            End Get
            Set(value As String)
                If Not value.Equals(_xValue) Then
                    SetProperty(Of String)(_xValue, value)
                    If currentPlotter IsNot Nothing AndAlso Not _mode3D Then
                        Dim x As Double = Double.NaN
                        Double.TryParse(_xValue, x)
                        If Not Double.IsNaN(x) Then
                            FxValue = currentPlotter.EvaluateExpression(x).ToString
                            currentPlotter.RenderCursor(x)
                        End If
                    End If
                End If
            End Set
        End Property

        Private _fxValue As String
        Public Property FxValue As String
            Get
                Return _fxValue
            End Get
            Set(value As String)
                If Not value.Equals(_fxValue) Then
                    SetProperty(Of String)(_fxValue, value)
                End If
            End Set
        End Property

        Private _xMode As Boolean = True
        Public Property XMode As Boolean
            Get
                Return _xMode
            End Get
            Set(value As Boolean)
                If value <> _xMode Then
                    SetProperty(Of Boolean)(_xMode, value)
                    If Not _is2DDetailViewer Then
                        DetailViewModel.Clear()
                        If XMode Then
                            ConstantValueHeader = "x ="
                            DetailPositionHeader = "y ="
                        Else
                            ConstantValueHeader = "y ="
                            DetailPositionHeader = "x ="
                        End If
                    End If
                End If
            End Set
        End Property

        Private Sub Clear()
            XYValue = ""
            FxValue = ""
            XValue = ""
            If Mode3D Then
                currentPlotter3D.ClearExpression()
            Else
                currentPlotter.ClearExpression()
            End If
        End Sub

        Private _xYValue As String
        Public Property XYValue As String
            Get
                Return _xYValue
            End Get
            Set(value As String)
                If Not value.Equals(_xYValue) Then
                    SetProperty(Of String)(_xYValue, value)
                    If currentPlotter3D IsNot Nothing AndAlso _mode3D Then
                        Dim xy As Double = Double.NaN
                        Double.TryParse(_xYValue, xy)
                        If Not Double.IsNaN(xy) Then
                            If XMode Then
                                currentPlotter3D.RenderXAreaAt(xy)
                            Else
                                currentPlotter3D.RenderYAreaAt(xy)
                            End If
                        End If
                    End If
                End If
            End Set
        End Property

        Private _fxHeader As String = "f(x) ="
        Public Property FxHeader As String
            Get
                Return _fxHeader
            End Get
            Set(value As String)
                If Not value.Equals(_fxHeader) Then
                    SetProperty(Of String)(_fxHeader, value)
                End If
            End Set
        End Property

        Private _constanValueHeader As String = "x ="
        Public Property ConstantValueHeader As String
            Get
                Return _constanValueHeader
            End Get
            Set(value As String)
                If Not value.Equals(_constanValueHeader) Then
                    SetProperty(Of String)(_constanValueHeader, value)
                End If

            End Set
        End Property

        Private _detailPositionHeader As String = "y ="
        Public Property DetailPositionHeader As String
            Get
                Return _detailPositionHeader
            End Get
            Set(value As String)
                If Not value.Equals(_detailPositionHeader) Then
                    SetProperty(Of String)(_detailPositionHeader, value)
                End If
            End Set
        End Property

        Private currentPlotter As FunctionPlotter
        Private currentPlotter3D As FunctionPlotter3D

        Private _pointerPressedPosition As Point
        Private _pointerPressedPositionValid As Boolean = False

        Structure AreaDescriptor
            Dim x1 As Double
            Dim x2 As Double
        End Structure


        Private _areaSelection As New AreaDescriptor
        Private _areaSelectionValid As Boolean

        Public Property AreaSelectionValid As Boolean
            Get
                Return _areaSelectionValid
            End Get
            Set(value As Boolean)
                If value <> _areaSelectionValid Then
                    SetProperty(Of Boolean)(_areaSelectionValid, value)
                End If
            End Set
        End Property

        Private _areaStack As New Stack(Of AreaDescriptor)

        Public ReadOnly Property IsExpressionDefined As Boolean
            Get
                If _mode3D Then
                    Return currentPlotter3D.IsExpressionDefined
                Else
                    Return currentPlotter.IsExpressionDefined
                End If
            End Get
        End Property

        Private _mode3D As Boolean = False
        Private _modeChanged As Boolean = False
        Private _is2DDetailViewer As Boolean = False

        Public Property DetailViewModel As MathEvaluatorViewModel

        Public Property Mode3D As Boolean
            Get
                Return _mode3D
            End Get
            Set(value As Boolean)
                If value <> _mode3D Then
                    Clear()
                    SetProperty(Of Boolean)(_mode3D, value)
                    currentPlotter.Clear()
                    currentPlotter3D.Clear()
                    _areaStack.Clear()
                    _modeChanged = True
                    If _mode3D Then
                        AreaSelectionValid = False
                        FxHeader = "f(x,y) ="
                        AdjustGraphGrid()
                        ExpressionIsValid = currentPlotter3D.IsValidExpression(_expression)
                    Else
                        FxHeader = "f(x) ="
                        AdjustGraphGrid()
                        ExpressionIsValid = currentPlotter.IsValidExpression(_expression)
                    End If
                End If
            End Set
        End Property

        Public Property PlotCommand As RelayCommand
        Public Property EnlargeCommand As RelayCommand
        Public Property ShrinkCommand As RelayCommand
        Public Property FindRootCommand As RelayCommand
        Public Property ShowSettingsDialogCommand As RelayCommand
        Public Property ShowAboutDialogCommand As RelayCommand

        Private _plottingArea As Canvas
        Private _graphGrid As Grid

        Public Sub New(graphGrid As Grid, plottingArea As Canvas, plottingArea2DDetail As Canvas, Optional asDetailViewer As Boolean = False)
            _plottingArea = plottingArea
            _graphGrid = graphGrid
            If asDetailViewer Then
                _is2DDetailViewer = True
            Else
                AdjustGraphGrid()
                currentPlotter3D = New FunctionPlotter3D()
                DetailViewModel = New MathEvaluatorViewModel(graphGrid, plottingArea2DDetail, Nothing, True)
            End If
            currentPlotter = New FunctionPlotter()
            PlotCommand = New RelayCommand(AddressOf PlotGraph)
            EnlargeCommand = New RelayCommand(AddressOf Enlarge)
            ShrinkCommand = New RelayCommand(AddressOf Shrink)
            FindRootCommand = New RelayCommand(AddressOf FindRoot)
            ShowSettingsDialogCommand = New RelayCommand(AddressOf ShowSettingsDialog)
            ShowAboutDialogCommand = New RelayCommand(AddressOf ShowAboutDialog)
        End Sub

        Private Sub AdjustGraphGrid()
            If _mode3D Then
                _graphGrid.ColumnDefinitions.Item(0).Width = New GridLength(2, GridUnitType.Star)
                _graphGrid.ColumnDefinitions.Item(2).Width = New GridLength(1, GridUnitType.Star)
            Else
                _graphGrid.ColumnDefinitions.Item(0).Width = New GridLength(1, GridUnitType.Star)
                _graphGrid.ColumnDefinitions.Item(2).Width = New GridLength(0)
            End If
        End Sub

        Private Sub SetPlottingArea()
            If _plottingArea.Clip.Rect.Width <> _plottingArea.ActualWidth OrElse _plottingArea.Clip.Rect.Height <> _plottingArea.ActualHeight Then
                _plottingArea.Clip.Rect = New Rect(0, 0, _plottingArea.ActualWidth - 1, _plottingArea.ActualHeight - 1)
            End If
            currentPlotter.SetPlottingArea(_plottingArea)
            If Not _is2DDetailViewer Then
                currentPlotter3D.SetPlottingArea(_plottingArea)
            End If
        End Sub

        Public Sub SetExpression(ex As MathExpression, variable As String)
            If _is2DDetailViewer Then
                currentPlotter.SetExpression(ex, variable)
            End If
        End Sub

        Private Sub PlotGraph()
            'If Not IsExpressionDefined Then
            '    Return
            'End If

            SetPlottingArea()
            currentPlotter.Clear()
            If Not _is2DDetailViewer Then
                currentPlotter3D.Clear()
            End If
            _modeChanged = False
            Dim xMin As Double = 0
            Dim xMax As Double = 1
            Double.TryParse(Min, xMin)
            Double.TryParse(Max, xMax)
            If _mode3D Then
                Dim yMin As Double = 0
                Dim yMax As Double = 1
                Dim points As Integer = 10
                Double.TryParse(MinY, yMin)
                Double.TryParse(MaxY, yMax)
                Integer.TryParse(N, points)
                currentPlotter3D.PlotFunction(Expression, xMin, xMax, yMin, yMax, points)
            ElseIf _is2DDetailViewer Then
                currentPlotter.PlotFunction(xMin, xMax)
                AreaSelectionValid = False
                _pointerPressedPositionValid = False
            Else
                currentPlotter.SetExpression(Expression)
                currentPlotter.PlotFunction(xMin, xMax)
                AreaSelectionValid = False
                _pointerPressedPositionValid = False
            End If
            OnPropertyChanged("IsExpressionDefined")
        End Sub

        Public Sub PlotCanvas_PointerExited(e As PointerRoutedEventArgs)
            XValue = ""
            FxValue = ""
            currentPlotter.HideCursor()
            If Not _is2DDetailViewer Then
                XYValue = ""
                currentPlotter3D.Hide3DArea()
            End If
        End Sub

        Public Sub PlotCanvas_PointerMoved(e As PointerRoutedEventArgs)
            If (_mode3D AndAlso Not currentPlotter3D.IsExpressionDefined()) OrElse (Not _mode3D AndAlso Not currentPlotter.IsExpressionDefined()) Then
                Return
            End If

            Dim ptr As Pointer = e.Pointer
            If ptr.PointerDeviceType = Windows.Devices.Input.PointerDeviceType.Mouse Then
                Dim ptrPt As Windows.UI.Input.PointerPoint = e.GetCurrentPoint(currentPlotter.GetArea())
                If _mode3D Then
                    If XMode Then
                        XYValue = currentPlotter3D.GetXPosition(ptrPt.Position.X).ToString
                    Else
                        XYValue = currentPlotter3D.GetYPosition(ptrPt.Position.Y).ToString
                    End If
                Else
                    Dim worldPoint = currentPlotter.GetWorldCoords(ptrPt.Position.X, 0)
                    XValue = worldPoint.X.ToString

                    If _pointerPressedPositionValid Then
                        currentPlotter.RenderAreaAt(_pointerPressedPosition.X, ptrPt.Position.X)
                    End If
                End If
            End If
        End Sub

        Private Sub PointerPressed2D(e As PointerRoutedEventArgs, plotter As FunctionPlotter)
            Dim ptr As Pointer = e.Pointer
            If ptr.PointerDeviceType = Windows.Devices.Input.PointerDeviceType.Mouse Then
                Dim ptrPt As Windows.UI.Input.PointerPoint = e.GetCurrentPoint(currentPlotter.GetArea())
                _pointerPressedPosition = ptrPt.Position
                _pointerPressedPositionValid = True
                If _areaSelectionValid Then
                    plotter.HideArea()
                    _areaSelectionValid = False
                End If
            End If
        End Sub

        Private Sub PointerReleased2D(e As PointerRoutedEventArgs)
            Dim ptr As Pointer = e.Pointer
            If ptr.PointerDeviceType = Windows.Devices.Input.PointerDeviceType.Mouse AndAlso _pointerPressedPositionValid Then
                Dim ptrPt As Windows.UI.Input.PointerPoint = e.GetCurrentPoint(currentPlotter.GetArea())
                _areaSelection.x1 = _pointerPressedPosition.X
                _areaSelection.x2 = ptrPt.Position.X
                _pointerPressedPositionValid = False
                If _areaSelection.x1 <> _areaSelection.x2 Then
                    If _areaSelection.x2 < _areaSelection.x1 Then
                        _areaSelection.x2 = _pointerPressedPosition.X
                        _areaSelection.x1 = ptrPt.Position.X
                    End If
                    AreaSelectionValid = True
                End If
            End If
        End Sub

        Public Sub PlotCanvas_PointerPressed(e As PointerRoutedEventArgs)
            If _mode3D Then
                If Not currentPlotter3D.IsExpressionDefined() Then
                    Return
                End If
                Dim val As Double = Double.NaN
                Double.TryParse(XYValue, val)
                If Double.IsNaN(val) Then
                    Return
                End If
                DetailViewModel.XMode = XMode
                DetailViewModel.XYValue = val.ToString
                If _xMode Then
                    DetailViewModel.Min = MinY
                    DetailViewModel.Max = MaxY
                    DetailViewModel.SetExpression(currentPlotter3D.Expression.ConstantXValue(val), "Y")
                Else
                    DetailViewModel.Min = Min
                    DetailViewModel.Max = Max
                    DetailViewModel.SetExpression(currentPlotter3D.Expression.ConstantYValue(val), "X")
                End If
                DetailViewModel.PlotGraph()
            Else
                If Not currentPlotter.IsExpressionDefined() Then
                    Return
                End If
                PointerPressed2D(e, currentPlotter)
            End If
        End Sub

        Public Sub PlotCanvas_PointerReleased(e As PointerRoutedEventArgs)
            If Not currentPlotter.IsExpressionDefined() OrElse Not _pointerPressedPositionValid OrElse _mode3D Then
                Return
            End If
            PointerReleased2D(e)
        End Sub

        Public Sub PlottingArea_SizeChanged(e As SizeChangedEventArgs)
            If Not _modeChanged Then
                PlotGraph()
            End If
        End Sub

        Private Sub Enlarge()
            Dim point1 As Point
            Dim point2 As Point
            If _areaSelectionValid Then
                point1 = currentPlotter.GetWorldCoords(_areaSelection.x1, 0)
                point2 = currentPlotter.GetWorldCoords(_areaSelection.x2, 0)
            Else
                point1.X = Min / 10
                point2.X = Max / 10
                If _mode3D Then
                    point1.Y = MinY / 10
                    point2.Y = MaxY / 10
                End If
            End If
            If Not _mode3D Then
                Dim currentWorld As New AreaDescriptor With {.x1 = Min, .x2 = Max}
                _areaStack.Push(currentWorld)
            End If
            Dim x1 As Double
            Dim x2 As Double
            x1 = point1.X
            x2 = point2.X
            Min = x1.ToString
            Max = x2.ToString
            If _mode3D Then
                Dim y1 As Double
                Dim y2 As Double
                y1 = point1.Y
                y2 = point2.Y
                MinY = y1.ToString
                MaxY = y2.ToString
            End If
            PlotGraph()
        End Sub

        Private Sub Shrink()
            If _areaStack.Count > 0 AndAlso Not _mode3D Then
                Min = _areaStack.Peek().x1
                Max = _areaStack.Peek().x2
                _areaStack.Pop()
            Else
                Min = Min * 10
                Max = Max * 10
                If _mode3D Then
                    MinY = MinY * 10
                    MaxY = MaxY * 10
                End If
            End If
            PlotGraph()
        End Sub

        Private Sub FindRoot()
            If AreaSelectionValid Then
                Dim point1 = currentPlotter.GetWorldCoords(_areaSelection.x1, 0)
                Dim point2 = currentPlotter.GetWorldCoords(_areaSelection.x2, 0)
                Dim x = ExpressionEvaluator.Bisection.FindRoot(currentPlotter.Expression, point1.X, point2.X, 0.00000001)
                If Not Double.IsNaN(x) Then
                    XValue = x.ToString
                End If
            End If
        End Sub

        Private Async Sub ShowSettingsDialog()
            Dim dialog = New SettingsDialog()
            Await dialog.ShowAsync()
        End Sub

        Private Async Sub ShowAboutDialog()
            Dim dialog = New AboutDialog()
            Await dialog.ShowAsync()
        End Sub
    End Class

End Namespace
