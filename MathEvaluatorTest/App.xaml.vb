Imports Windows.ApplicationModel.Resources
''' <summary>
''' Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
''' </summary>
NotInheritable Class App
    Inherits Application

    Public Shared Texts As ResourceLoader = New Windows.ApplicationModel.Resources.ResourceLoader()

    Public Const ApplicationThemeLight As Integer = 0
    Public Const ApplicationThemeDark As Integer = 1
    Public Shared SelectedApplicationTheme As Integer = 0

    Private Shared _graphColorBrush As Brush
    Public Shared ReadOnly Property GraphColorBrush As Brush
        Get
            Return _graphColorBrush
        End Get
    End Property

    Private Shared _secondaryGraphColorBrush As Brush
    Public Shared ReadOnly Property SecondaryGraphColorBrush As Brush
        Get
            Return _secondaryGraphColorBrush
        End Get
    End Property

    Private Shared _coordinateSystemColorBrush As Brush
    Public Shared ReadOnly Property CoordinateSystemColorBrush As Brush
        Get
            Return _coordinateSystemColorBrush
        End Get
    End Property

    Private Shared _areaColorBrush As Brush
    Public Shared ReadOnly Property AreaColorBrush As Brush
        Get
            Return _areaColorBrush
        End Get
    End Property

    Private Shared _errorColorBrush As Brush
    Public Shared ReadOnly Property ErrorColorBrush As Brush
        Get
            Return _errorColorBrush
        End Get
    End Property

    Private Shared _standardForegroundBrush As Brush
    Public Shared ReadOnly Property StandardForegroundBrush As Brush
        Get
            Return _standardForegroundBrush
        End Get
    End Property

    Public Sub New()

        Dim settings = Windows.Storage.ApplicationData.Current.LocalSettings

        SelectedApplicationTheme = settings.Values("ApplicationTheme")
        If SelectedApplicationTheme = ApplicationThemeDark Then
            RequestedTheme = ApplicationTheme.Dark
        Else
            RequestedTheme = ApplicationTheme.Light
        End If

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
    ''' werden verwendet, wenn die Anwendung zum Öffnen einer bestimmten Datei, zum Anzeigen
    ''' von Suchergebnissen usw. gestartet wird.
    ''' </summary>
    ''' <param name="e">Details über Startanforderung und -prozess.</param>
    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)

        _graphColorBrush = Resources("GraphColorBrush")
        _secondaryGraphColorBrush = Resources("SecondaryGraphColorBrush")
        _coordinateSystemColorBrush = Resources("CoordinateSystemColorBrush")
        _areaColorBrush = Resources("AreaColorBrush")
        _errorColorBrush = Resources("ErrorColorBrush")
        _standardForegroundBrush = Resources("TextControlForeground")

        ' App-Initialisierung nicht wiederholen, wenn das Fenster bereits Inhalte enthält.
        ' Nur sicherstellen, dass das Fenster aktiv ist.

        If rootFrame Is Nothing Then
            ' Frame erstellen, der als Navigationskontext fungiert und zum Parameter der ersten Seite navigieren
            rootFrame = New Frame()

            AddHandler rootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Zustand von zuvor angehaltener Anwendung laden
            End If
            ' Den Frame im aktuellen Fenster platzieren
            Window.Current.Content = rootFrame
        End If

        If e.PrelaunchActivated = False Then
            If rootFrame.Content Is Nothing Then
                ' Wenn der Navigationsstapel nicht wiederhergestellt wird, zur ersten Seite navigieren
                ' und die neue Seite konfigurieren, indem die erforderlichen Informationen als Navigationsparameter
                ' übergeben werden
                rootFrame.Navigate(GetType(MainPage), e.Arguments)
            End If

            ' Sicherstellen, dass das aktuelle Fenster aktiv ist
            Window.Current.Activate()
        End If
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
    ''' </summary>
    ''' <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
    ''' <param name="e">Details über den Navigationsfehler</param>
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
    ''' ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
    ''' unbeschädigt bleiben.
    ''' </summary>
    ''' <param name="sender">Die Quelle der Anhalteanforderung.</param>
    ''' <param name="e">Details zur Anhalteanforderung.</param>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
        deferral.Complete()
    End Sub

End Class
