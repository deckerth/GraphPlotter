' Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

Public NotInheritable Class AboutDialog
    Inherits ContentDialog

    Public Sub New()
        InitializeComponent()
        InfoBox.NavigateToString(App.Texts.GetString("AboutText"))
    End Sub

    Private Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Hide()
    End Sub

End Class
