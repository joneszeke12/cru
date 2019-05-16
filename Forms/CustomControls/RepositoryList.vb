Imports CrystalDecisions.Enterprise
Imports System.Windows.Forms
Class RepositoryList
    Sub Create(repositoryNodeID As String, infoObjects As InfoObjects)
        BeginUpdate()
        Items.Clear()
        Tag = repositoryNodeID
        For Each infoObject As InfoObject In infoObjects
            If infoObject.Kind = "Folder" Then
                Items.Add(New ListViewItem(New String() {infoObject.Title, "Folder",
                                           infoObject.Properties("SI_CREATION_TIME").Value,
                                           infoObject.Properties("SI_UPDATE_TS").Value}, 1) With
                                           {.Tag = {New Dictionary(Of String, Boolean) From {{"IsFolder", True}}, infoObject}})
            Else
                Items.Add(New ListViewItem(New String() {infoObject.Title, "CR File",
                                           infoObject.Properties("SI_CREATION_TIME").Value,
                                           infoObject.Properties("SI_UPDATE_TS").Value}, 2) With
                                           {.Tag = {New Dictionary(Of String, Boolean) From {{"IsFolder", False}}, infoObject}})
            End If
        Next
        EndUpdate()
    End Sub
    Function IsFolderItem(listViewItem As ListViewItem) As Boolean
        Return CType(listViewItem.Tag(0), Dictionary(Of String, Boolean)).Item("IsFolder")
    End Function
    Function GetInfoObject(listViewItem As ListViewItem) As InfoObject
        Return CType(listViewItem.Tag(1), InfoObject)
    End Function
End Class
Class TitleSorter
    Implements IComparer
    Private _sortOrder As Integer
    Sub New(i As Integer)
        _sortOrder = i
    End Sub
    Function GetTitle(listViewItem As ListViewItem) As String
        Return listViewItem.SubItems(0).Text
    End Function
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return String.Compare(GetTitle(CType(x, ListViewItem)), GetTitle(CType(y, ListViewItem))) * _sortOrder
    End Function
End Class
Class KindSorter
    Implements IComparer
    Private _sortOrder As Integer
    Sub New(i As Integer)
        _sortOrder = i
    End Sub
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return String.Compare(GetKind(CType(x, ListViewItem)), GetKind(CType(y, ListViewItem))) * _sortOrder
    End Function
    Function GetKind(listViewItem As ListViewItem) As String
        Return listViewItem.SubItems(1).Text
    End Function
End Class
Class CreationDateSorter
    Implements IComparer
    Private _sortOrder As Integer
    Sub New(i As Integer)
        _sortOrder = i
    End Sub
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return Date.Compare(GetCreationDate(CType(x, ListViewItem)), GetCreationDate(CType(y, ListViewItem))) * _sortOrder
    End Function
    Function GetCreationDate(listViewItem As ListViewItem) As String
        Return Date.Parse(listViewItem.SubItems(2).Text)
    End Function
End Class
Class UpdateDateSorter
    Implements IComparer
    Private _sortOrder As Integer
    Sub New(i As Integer)
        _sortOrder = i
    End Sub
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Return Date.Compare(GetUpdateDate(CType(x, ListViewItem)), GetUpdateDate(CType(y, ListViewItem))) * _sortOrder
    End Function
    Function GetUpdateDate(listViewItem As ListViewItem) As String
        Return Date.Parse(listViewItem.SubItems(3).Text)
    End Function
End Class
MustInherit Class SortState
    Private _currentState As Integer = -1
    Function GetNextState() As Integer
        _currentState *= -1
        Return _currentState
    End Function
End Class
Class TitleSortState
    Inherits SortState
End Class
Class KindSortState
    Inherits SortState
End Class
Class CreationDateSortState
    Inherits SortState
End Class
Class UpdateDateSortState
    Inherits SortState
End Class
