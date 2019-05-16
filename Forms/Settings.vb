Public Class SettingsForm
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyPropertyGrid.SelectedObject = New Options
    End Sub
    Private Sub DefaultButton_Click(sender As Object, e As EventArgs) Handles DefaultButton.Click
        With My.Settings
            If .CM = "On" Then
                .CM = "Off"
            End If
            .Reset()
        End With
        MyPropertyGrid.Refresh()
    End Sub
End Class