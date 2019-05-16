Imports CrystalDecisions.Enterprise
Imports System.Windows.Forms
Public Class RepositoryTree
    Sub Create(cmsName As String)
        Dim infoObjects As InfoObjects
        BeginUpdate()
        With Nodes.Add("0", cmsName, 0, 0)
            .Tag = {New Dictionary(Of String, Boolean) From {{"IsRoot", True}, {"IsFolder", False}}, Nothing}
            infoObjects = AppSession.Query(AppSession.TreeQuerySyntax("23"))
            For Each infoObject In infoObjects
                AddRepositoryNodes(infoObject, .Nodes)
            Next
        End With
        Sort()
        EndUpdate()
    End Sub
    Sub AddRepositoryNodes(infoObject As InfoObject, treeNodes As TreeNodeCollection)
        Dim infoObjects As InfoObjects
        infoObjects = AppSession.Query(AppSession.TreeQuerySyntax(infoObject.ID.ToString))
        With treeNodes.Add(infoObject.ID.ToString, infoObject.Title, 1, 1)
            .Tag = {New Dictionary(Of String, Boolean) From {{"IsRoot", False}, {"IsFolder", True}}, infoObject}
            For Each infoObject In infoObjects
                AddRepositoryNodes(infoObject, .Nodes)
            Next
        End With
    End Sub
    Function IsRootNode(node As TreeNode) As Boolean
        Return CType(node.Tag(0), Dictionary(Of String, Boolean)).Item("IsRoot")
    End Function
    Function IsFolderNode(node As TreeNode) As Boolean
        Return CType(node.Tag(0), Dictionary(Of String, Boolean)).Item("IsFolder")
    End Function
    Function GetInfoObject(node As TreeNode) As InfoObject
        Return CType(node.Tag(1), InfoObject)
    End Function
End Class