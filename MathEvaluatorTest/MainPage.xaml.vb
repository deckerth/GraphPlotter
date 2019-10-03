' Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

Imports MathEvaluatorTest.ViewModels
''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public Property ViewModel As MathEvaluatorViewModel

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ViewModel = New MathEvaluatorViewModel(GraphArea, PlotCanvas, PlotCanvas2DDetail)
    End Sub

    Private Sub PlotCanvas_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas.PointerExited
        ViewModel.PlotCanvas_PointerExited(e)
    End Sub

    Private Sub PlotCanvas_PointerMoved(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas.PointerMoved
        ViewModel.PlotCanvas_PointerMoved(e)
    End Sub

    Private Sub PlotCanvas_PointerPressed(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas.PointerPressed
        ViewModel.PlotCanvas_PointerPressed(e)
    End Sub

    Private Sub PlotCanvas_PointerReleased(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas.PointerReleased
        ViewModel.PlotCanvas_PointerReleased(e)
    End Sub

    Private Sub PlottingArea_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles PlottingArea.SizeChanged
        ViewModel.PlottingArea_SizeChanged(e)
    End Sub

    Private Sub PlotCanvas2DDetail_PointerExited(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas2DDetail.PointerExited
        ViewModel.DetailViewModel.PlotCanvas_PointerExited(e)
    End Sub

    Private Sub PlotCanvas2DDetail_PointerMoved(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas2DDetail.PointerMoved
        ViewModel.DetailViewModel.PlotCanvas_PointerMoved(e)
    End Sub

    Private Sub PlotCanvas2DDetail_PointerPressed(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas2DDetail.PointerPressed
        ViewModel.DetailViewModel.PlotCanvas_PointerPressed(e)
    End Sub

    Private Sub PlotCanvas2DDetail_PointerReleased(sender As Object, e As PointerRoutedEventArgs) Handles PlotCanvas2DDetail.PointerReleased
        ViewModel.DetailViewModel.PlotCanvas_PointerReleased(e)
    End Sub

    Private Sub PlotCanvas2DDetail_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles PlotCanvas2DDetail.SizeChanged
        If ViewModel.Mode3D Then
            ViewModel.DetailViewModel.PlottingArea_SizeChanged(e)
        End If
    End Sub

End Class
