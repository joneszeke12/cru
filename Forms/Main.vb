Imports System.IO
Imports System.Threading
Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LogForm.Show()
    End Sub
    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Select Case WindowState
            Case FormWindowState.Minimized
                ShowInTaskbar = False
                With MFNotifyIcon
                    .Icon = Icon
                    .Visible = True
                    .ShowBalloonTip(3000, "CR Utility",
                                    "Program is currently running in background.",
                                    ToolTipIcon.None)
                End With
            Case FormWindowState.Normal
                ShowInTaskbar = True
                For Each ownedForm In OwnedForms
                    ownedForm.Visible = True
                Next
        End Select
    End Sub
    Private Sub FolderButton_Click(sender As Object, e As EventArgs) Handles FolderButton.Click
        Dim folderName As String
        Dim folderFiles As String()
        Dim dialogResult As DialogResult
        Dim folderBrowserDialog As New FolderBrowserDialog() With {
                .Description = "Select a folder that contains '.rpt' files.",
                .RootFolder = Environment.SpecialFolder.Desktop
            }
        dialogResult = folderBrowserDialog.ShowDialog(Me)
        If dialogResult = DialogResult.Cancel Then
            Exit Sub
        Else
            folderName = folderBrowserDialog.SelectedPath
            folderFiles = Directory.GetFiles(folderName, "*.rpt", SearchOption.TopDirectoryOnly)
            If folderFiles.Count = 0 Then
                AppTextBox.AppendText(Date.Now.ToString("f") +
                                      " - No RPT files found in directory: [" +
                                      folderName +
                                      "]" + vbCrLf)
            Else
                For Each folderFile In folderFiles
                    AppWorker.Queue(New WaitCallback(Sub() AppWorker.Extract(Path.GetFullPath(folderFile))))
                Next
            End If
        End If
    End Sub
    Private Sub FileButton_Click(sender As Object, e As EventArgs) Handles FileButton.Click
        Dim fileNames As String()
        Dim dialogResult As DialogResult
        Dim openFileDialog As New OpenFileDialog With {
                .Filter = "RPT Files|*.rpt",
                .InitialDirectory = Environment.SpecialFolder.Desktop,
                .Multiselect = True,
                .Title = "Select '.rpt' file(s)."
        }
        Dim outputFileRef As String = String.Empty
        dialogResult = openFileDialog.ShowDialog()
        If dialogResult = DialogResult.Cancel Then
            Exit Sub
        Else
            fileNames = openFileDialog.FileNames
            For Each fileName In fileNames
                If fileNames.Count > 1 Then
                    AppWorker.Queue(New WaitCallback(Sub() AppWorker.Extract(fileName)))
                Else
                    AppWorker.Extract(fileName, outputFileRef)
                    If My.Settings.OOS Then
                        Try
                            Process.Start(outputFileRef)
                        Catch ex As Exception
                            PostToLog(Date.Now.ToString("f") + " - Error opening [" + outputFileRef + "]: " + ex.Message)
                        End Try
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub SettingsButton_Click(sender As Object, e As EventArgs) Handles SettingsButton.Click
        If Not Application.OpenForms.OfType(Of SettingsForm).Any Then
            SettingsForm.Show(Me)
        End If
    End Sub
    Private Sub MFNotifyIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles MFNotifyIcon.MouseClick
        If e.Button = MouseButtons.Left Then
            WindowState = FormWindowState.Normal
            MFNotifyIcon.Visible = False
        ElseIf e.Button = MouseButtons.Right Then
            MFNotifyIcon.ContextMenuStrip.Show()
        End If
    End Sub
    Private Sub MFContextMenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MFContextMenuStrip.ItemClicked
        If e.ClickedItem.Text = "Show" Then
            WindowState = FormWindowState.Normal
            MFNotifyIcon.Visible = False
        ElseIf e.ClickedItem.Text = "Exit" Then
            Application.Exit()
        End If
    End Sub
End Class