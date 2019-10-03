' Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

Public NotInheritable Class SettingsDialog
    Inherits ContentDialog

    Public Sub New()
        InitializeComponent()

        Select Case App.SelectedApplicationTheme
            Case App.ApplicationThemeDark
                ThemeChooser.SelectedItem = ThemeDark
            Case App.ApplicationThemeLight
                ThemeChooser.SelectedItem = ThemeLight
        End Select
    End Sub

    Private Async Sub ContentDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Dim settings = Windows.Storage.ApplicationData.Current.LocalSettings
        Dim themeChanged As Boolean = False

        Select Case ThemeChooser.SelectedIndex
            Case 0 ' Light
                If App.SelectedApplicationTheme = App.ApplicationThemeDark Then
                    settings.Values("ApplicationTheme") = App.ApplicationThemeLight
                    themeChanged = True
                End If
            Case 1 'Dark
                If App.SelectedApplicationTheme = App.ApplicationThemeLight Then
                    settings.Values("ApplicationTheme") = App.ApplicationThemeDark
                    themeChanged = True
                End If
        End Select

        If themeChanged Then
            Dim msg = New Windows.UI.Popups.MessageDialog(App.Texts.GetString("RestartAppForThemeChangeRequired"))
            Await msg.ShowAsync()
        End If

        Hide()

    End Sub

    Private Sub ContentDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)
        Hide()
    End Sub
End Class
