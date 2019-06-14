Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            Extract(String.Join(" ", e.CommandLine))
        End Sub
        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Extract(String.Join(" ", e.CommandLine))
        End Sub
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show(e.Exception.Message,
                            "Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Sub
        Private Sub Extract(commandArg As String)
            Dim fileName As String
            Dim folderName As String
            Dim folderFiles As String()
            Dim isRPT As Boolean = LCase(Path.GetExtension(commandArg)).Equals(".rpt")
            Dim directoryExists As Boolean = Directory.Exists(commandArg)
            Dim outputFileRef As String = String.Empty
            If isRPT Then
                fileName = Path.GetFullPath(commandArg)
                AppWorker.Extract(fileName, outputFileRef)
                If My.Settings.OOS Then
                    Try
                        Process.Start(outputFileRef)
                    Catch ex As Exception
                        PostToLog(Date.Now.ToString("f") + " - Error opening [" + outputFileRef + "]: " + ex.Message)
                    End Try
                End If
            ElseIf directoryExists Then
                folderName = Path.GetFullPath(commandArg)
                folderFiles = Directory.GetFiles(folderName, "*.rpt", SearchOption.TopDirectoryOnly)
                If folderFiles.Count = 0 Then
                    PostToLog(Date.Now.ToString("f") +
                              " - No RPT files found in directory: [" +
                              folderName +
                              "]")
                Else
                    For Each folderFile In folderFiles
                        AppWorker.Queue(New WaitCallback(Sub() AppWorker.Extract(Path.GetFullPath(folderFile))))
                    Next
                End If
            End If
        End Sub
    End Class
End Namespace
