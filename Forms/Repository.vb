Imports CrystalDecisions.Enterprise
Imports System.Threading
Public Class RepositoryForm
    Private _titleSortState As New TitleSortState
    Private _kindSortState As New KindSortState
    Private _creationDateSortState As New CreationDateSortState
    Private _updateDateSortState As New UpdateDateSortState
    Private Sub RepositoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Lock()
        Text = "Connected to: " + AppSession.EnterpriseSession.CMSName + " (Loading...)"
        Unlock()
    End Sub
    Private Sub RepositoryForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Lock()
        RepositoryTree.Create(AppSession.EnterpriseSession.CMSName)
        Text = "Connected to: " + AppSession.EnterpriseSession.CMSName
        Unlock()
    End Sub
    Private Sub RepositoryTree_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles RepositoryTree.NodeMouseClick
        Dim infoObjects As InfoObjects
        If e.Button = MouseButtons.Left Then
            Lock()
            If RepositoryTree.IsRootNode(e.Node) Then
                infoObjects = AppSession.Query(AppSession.ListviewQuerySyntax("23"))
            Else
                infoObjects = AppSession.Query(AppSession.ListviewQuerySyntax(e.Node.Name))
            End If
            RepositoryList.Create(e.Node.Name, infoObjects)
            RepositoryTree.SelectedNode = e.Node
            Unlock()
        End If
    End Sub
    Private Sub RepositoryTree_MouseDown(sender As Object, e As MouseEventArgs) Handles RepositoryTree.MouseDown
        If RepositoryTree.SelectedNode Is Nothing Then Exit Sub
        RepositoryTree.SelectedNode = Nothing
    End Sub
    Private Sub RepositoryList_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RepositoryList.MouseDoubleClick
        Dim infoObjects As InfoObjects
        Dim selectedItem As ListViewItem
        Dim selectedItemID As String
        Dim correspondingNode As TreeNode
        If e.Button = MouseButtons.Left AndAlso
            RepositoryList.SelectedItems.Count = 1 AndAlso
            e.Clicks = 2 AndAlso
            RepositoryList.IsFolderItem(RepositoryList.SelectedItems.Item(0)) Then
            Lock()
            selectedItem = RepositoryList.SelectedItems(0)
            selectedItemID = RepositoryList.GetInfoObject(selectedItem).ID.ToString
            infoObjects = AppSession.Query(AppSession.ListviewQuerySyntax(selectedItemID))
            RepositoryList.Create(selectedItemID, infoObjects)
            RepositoryTree.Select()
            correspondingNode = RepositoryTree.Nodes.Find(selectedItemID, True).Single
            RepositoryTree.SelectedNode = correspondingNode
            correspondingNode.EnsureVisible()
            Unlock()
        End If
    End Sub
    Private Sub RepositoryList_MouseClick(sender As Object, e As MouseEventArgs) Handles RepositoryList.MouseClick
        If e.Button = MouseButtons.Right AndAlso
                RepositoryList.SelectedItems.Count > 0 AndAlso
                e.Clicks = 1 AndAlso
                RepositoryList.GetItemAt(e.X, e.Y) IsNot Nothing Then
            RepositoryContextMenuStrip.Show(RepositoryList, e.X, e.Y)
        End If
    End Sub
    Private Sub RepositoryList_MouseDown(sender As Object, e As MouseEventArgs) Handles RepositoryList.MouseDown
        If RepositoryList.SelectedItems.Count > 0 AndAlso
            RepositoryList.GetItemAt(e.X, e.Y) Is Nothing Then
            Lock()
            RepositoryList.SelectedItems.Clear()
            Unlock()
        End If
    End Sub
    Private Sub RepositoryList_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles RepositoryList.ColumnClick
        Lock()
        Select Case e.Column
            Case 0
                RepositoryList.ListViewItemSorter = New TitleSorter(_titleSortState.GetNextState)
            Case 1
                RepositoryList.ListViewItemSorter = New KindSorter(_kindSortState.GetNextState)
            Case 2
                RepositoryList.ListViewItemSorter = New CreationDateSorter(_creationDateSortState.GetNextState)
            Case 3
                RepositoryList.ListViewItemSorter = New UpdateDateSorter(_updateDateSortState.GetNextState)
        End Select
        Unlock()
    End Sub
    Private Sub RepositoryContextMenuStrip_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles RepositoryContextMenuStrip.ItemClicked
        Dim infoObjects As InfoObjects
        Dim outputFileRef As String = String.Empty
        If RepositorySplitContainer.ActiveControl Is RepositoryList Then
            If e.ClickedItem.Text = "Extract" Then
                For Each item In RepositoryList.SelectedItems
                    If RepositoryList.IsFolderItem(item) Then
                        infoObjects = AppSession.Query(AppSession.ReportsOnlyQuerySyntax(RepositoryList.GetInfoObject(item).ID.ToString))
                        For Each infoObject In infoObjects
                            AppWorker.Queue(New WaitCallback(Sub() AppWorker.Extract(infoObject, AppSession.EnterpriseSession)))
                        Next
                    Else
                        If RepositoryList.SelectedItems.Count > 1 Then
                            AppWorker.Queue(New WaitCallback(Sub() AppWorker.Extract(RepositoryList.GetInfoObject(item), AppSession.EnterpriseSession)))
                        Else
                            AppWorker.Extract(RepositoryList.GetInfoObject(item), AppSession.EnterpriseSession, outputFileRef)
                            If My.Settings.OOS Then
                                Try
                                    Process.Start(outputFileRef)
                                Catch ex As Exception
                                    PostToLog(Date.Now.ToString("f") + " - Error opening [" + outputFileRef + "]: " + ex.Message)
                                End Try
                            End If
                        End If
                    End If
                Next
            ElseIf e.ClickedItem.Text = "Download" Then
                For Each item In RepositoryList.SelectedItems
                    If RepositoryList.IsFolderItem(item) Then
                        infoObjects = AppSession.Query(AppSession.ReportsOnlyQuerySyntax(RepositoryList.GetInfoObject(item).ID.ToString))
                        For Each infoObject In infoObjects
                            AppWorker.Queue(New WaitCallback(Sub() AppWorker.Download(infoObject, AppSession.EnterpriseSession)))
                        Next
                    Else
                        If RepositoryList.SelectedItems.Count > 1 Then
                            AppWorker.Queue(New WaitCallback(Sub() AppWorker.Download(RepositoryList.GetInfoObject(item), AppSession.EnterpriseSession)))
                        Else
                            AppWorker.Download(RepositoryList.GetInfoObject(item), AppSession.EnterpriseSession, outputFileRef)
                            If My.Settings.OOS Then
                                Try
                                    Process.Start(outputFileRef)
                                Catch ex As Exception
                                    PostToLog(Date.Now.ToString("f") + " - Error opening [" + outputFileRef + "]: " + ex.Message)
                                End Try
                            End If
                        End If
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub Lock()
        Enabled = False
        Cursor = Cursors.WaitCursor
        UseWaitCursor = True
    End Sub
    Private Sub Unlock()
        UseWaitCursor = False
        Cursor = Cursors.Default
        Enabled = True
    End Sub
End Class