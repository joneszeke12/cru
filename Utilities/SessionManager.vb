Imports CrystalDecisions.Enterprise
Module SessionManager
    Class Session
        Private _sessionMgr As New SessionMgr
        Private _infoStore As InfoStore
        Private _token As String
        Property EnterpriseSession As EnterpriseSession
        Function TreeQuerySyntax(objectID As String)
            Return "select *
                    from ci_infoobjects
                    where si_parentid = " + objectID + " " +
                   "and si_name != 'REPORT CONVERSION TOOL' 
                    and si_name != 'ADMINISTRATION TOOLS' 
                    and si_name != 'AUDITOR' 
                    and si_kind = 'Folder'"
        End Function
        Function ListviewQuerySyntax(objectID As String)
            Return "select *
                                                 from ci_infoobjects
                                                 where si_parentid = " + objectID + " " +
                                                "and (si_kind = 'Folder' or si_kind = 'CrystalReport')"
        End Function
        Function ReportsOnlyQuerySyntax(objectID As String)
            Return "select *
                                                 from ci_infoobjects
                                                 where si_parentid = " + objectID + " " +
                                                "and si_kind = 'CrystalReport'"
        End Function
        Sub Login(info As String())
            EnterpriseSession = _sessionMgr.Logon(Trim(info(0)), Trim(info(1)), Trim(info(2)), Trim(info(3)))
            _infoStore = EnterpriseSession.GetService("InfoStore")
            _token = EnterpriseSession.LogonTokenMgr.DefaultToken
        End Sub
        Sub LoginWithToken()
            EnterpriseSession = _sessionMgr.LogonWithToken(_token)
        End Sub
        Function Query(str As String)
            Return _infoStore.Query(str)
        End Function
    End Class
End Module
