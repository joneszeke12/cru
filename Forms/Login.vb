Public Class LoginForm
    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With My.Settings
            ServerComboBox.DataSource = { .RecentServer1, .RecentServer2, .RecentServer3}
        End With
    End Sub
    Private Sub LoginForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ServerComboBox.ResetText()
        UsernameTextBox.ResetText()
        PasswordTextBox.ResetText()
        AuthenticationComboBox.ResetText()
    End Sub
    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click
        If String.IsNullOrWhiteSpace(ServerComboBox.Text) OrElse
            String.IsNullOrWhiteSpace(UsernameTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(PasswordTextBox.Text) OrElse
            AuthenticationComboBox.SelectedIndex = -1 Then
            MessageBox.Show(New Form With {.TopMost = True},
                            "All fields are required.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Else
            Login({UsernameTextBox.Text,
                   PasswordTextBox.Text,
                   ServerComboBox.Text,
                   AuthenticationComboBox.Text})
        End If
    End Sub
    Private Sub CancelButton1_Click(sender As Object, e As EventArgs) Handles CancelButton1.Click
        Close()
    End Sub
    Private Sub Login(loginInfo As String())
        Try
            Lock()
            AppSession.Login(loginInfo)
            MessageBox.Show(New Form With {.TopMost = True},
                "Login successful.",
                "Result")
            AddToRecentServers(Trim(ServerComboBox.Text))
            RepositoryForm.Show(MainForm)
            Close()
        Catch ex As Exception
            MessageBox.Show(New Form With {.TopMost = True},
                            ex.Message,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            PasswordTextBox.ResetText()
            Unlock()
        End Try
    End Sub
    Private Sub AddToRecentServers(serverName As String)
        With My.Settings
            If .RecentServer1 <> serverName And .RecentServer2 <> serverName Then
                .RecentServer3 = .RecentServer2
                .RecentServer2 = .RecentServer1
                .RecentServer1 = serverName
            ElseIf .RecentServer1 <> serverName And .RecentServer2 = serverName And .RecentServer3 <> serverName Then
                .RecentServer2 = .RecentServer1
                .RecentServer1 = serverName
            Else
                .RecentServer1 = serverName
            End If
        End With
    End Sub
    Private Sub Lock()
        ServerComboBox.Enabled = False
        UsernameTextBox.Enabled = False
        PasswordTextBox.Enabled = False
        AuthenticationComboBox.Enabled = False
        LoginButton.Enabled = False
        CancelButton1.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        UseWaitCursor = True
    End Sub
    Private Sub Unlock()
        ServerComboBox.Enabled = True
        UsernameTextBox.Enabled = True
        PasswordTextBox.Enabled = True
        AuthenticationComboBox.Enabled = True
        LoginButton.Enabled = True
        CancelButton1.Enabled = True
        UseWaitCursor = False
        Cursor.Current = Cursors.Default
    End Sub
End Class
