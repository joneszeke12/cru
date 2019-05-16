Public Class LogForm
    Private Declare Function HideCaret Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean
    Private Sub LogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Location = New Point(MainForm.Location.X / 2, MainForm.Location.Y + 250)
    End Sub
    Private Sub LogTextBox_GotFocus(sender As Object, e As EventArgs) Handles LogTextBox.GotFocus
        HideCaret(LogTextBox.Handle)
    End Sub
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim param As CreateParams = MyBase.CreateParams
            param.ClassStyle = param.ClassStyle Or &H200
            Return param
        End Get
    End Property
End Class