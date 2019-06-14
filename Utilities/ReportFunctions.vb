Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ctrl = CrystalDecisions.ReportAppServer.Controllers
Imports ddm = CrystalDecisions.ReportAppServer.DataDefModel
Imports eng = CrystalDecisions.CrystalReports.Engine
Imports rdm = CrystalDecisions.ReportAppServer.ReportDefModel
Module ReportFunctions
    Const E_APPENDAGE As String = " - EXTRACTION"
    Property ExclusiveUsage As New Mutex
    Class Worker
        Sub Queue(callBack As WaitCallback)
            ThreadPool.QueueUserWorkItem(callBack)
        End Sub
        Sub Extract(fileName As String, Optional ByRef outputFileRef As String = Nothing)
            Dim writer As New Writer
            Dim textWriter As TextWriter
            Dim outputFile As String = If(My.Settings.ED = "Default", DesktopDirectory, My.Settings.ED) + "\" + Path.GetFileNameWithoutExtension(fileName) + E_APPENDAGE + My.Settings.PE
            If outputFileRef IsNot Nothing Then outputFileRef = outputFile
            Try
                textWriter = TextWriter.Synchronized(New StreamWriter(File.Open(outputFile, FileMode.Create)))
                Using reportDocument As New eng.ReportDocument
                    reportDocument.Load(fileName)
                    writer.Write(textWriter, reportDocument)
                End Using
                ExclusiveUsage.WaitOne()
                PostToLog(Date.Now.ToString("f") + " - [" + fileName + "] extracted. Output located at: " + My.Settings.ED)
                ExclusiveUsage.ReleaseMutex()
            Catch ex As Exception
                ExclusiveUsage.WaitOne()
                PostToLog(Date.Now.ToString("f") + " - Error extracting [" + fileName + "]: " + ex.Message)
                ExclusiveUsage.ReleaseMutex()
            Finally
                If textWriter IsNot Nothing Then
                    textWriter.Dispose()
                End If
            End Try
        End Sub
    End Class
    Class Writer
        Sub Write(textWriter As TextWriter, reportDocument As eng.ReportDocument)
            Dim subreportClientDocuments As IEnumerable(Of ctrl.SubreportClientDocument)
            WriteToFile(textWriter, New ReportDocument.ReportAttributes(reportDocument))
            If My.Settings.SR = "Include" Then
                subreportClientDocuments = From name As String
                                               In reportDocument.ReportClientDocument.SubreportController.GetSubreportNames
                                           Select reportDocument.ReportClientDocument.SubreportController.GetSubreport(name)
                For Each subreportClientDocument In subreportClientDocuments
                    WriteToFile(textWriter, New ReportDocument.SubreportAttributes(subreportClientDocument))
                Next
            End If
        End Sub
        Sub WriteToFile(textWriter As TextWriter, attributes As ReportDocument.Attributes)
            With textWriter
                .WriteLine("/*")
                .WriteLine(attributes.Title)
                .WriteLine(PrintLines(1, "*"))
                .WriteLine(PrintLines(1, "*"))
                .WriteLine("*/")
                If My.Settings.T <> "None" Then
                    .WriteLine(PrintLines(5, " "))
                    .WriteLine("/*")
                    .WriteLine(My.Settings.T + " Table(s):")
                    .WriteLine(PrintLines(1, "+"))
                    .WriteLine(PrintLines(1, "+"))
                    .WriteLine("*/")
                    If attributes.Tables.Count = 0 Then
                        .WriteLine("/*")
                        .WriteLine("No table(s) found.")
                        .WriteLine("*/")
                    Else
                        For Each table In attributes.Tables
                            .WriteLine(PrintLines(5, " "))
                            .WriteLine(PrintLines(1, "-"))
                            .WriteLine("/*" + table(0) + "*/")
                            .WriteLine(PrintLines(1, "-"))
                            .WriteLine(table(1))
                            If My.Settings.TF <> "None" Then
                                .WriteLine(PrintLines(5, " "))
                                .WriteLine("/*")
                                .WriteLine(My.Settings.TF + " Table Fields(s):")
                                .WriteLine(PrintLines(1, "+"))
                                .WriteLine(PrintLines(1, "+"))
                                If CType(table(2), IEnumerable(Of String())).Count = 0 Then
                                    .WriteLine("No table fields(s) found.")
                                Else
                                    For Each tablefield As String() In table(2)
                                        .WriteLine(String.Join(": ", tablefield))
                                    Next
                                End If
                                .WriteLine("*/")
                            End If
                        Next
                    End If
                End If
                If My.Settings.P <> "None" Then
                    .WriteLine(PrintLines(5, " "))
                    .WriteLine("/*")
                    .WriteLine(My.Settings.P + " Parameter(s):")
                    .WriteLine(PrintLines(1, "+"))
                    .WriteLine(PrintLines(1, "+"))
                    If attributes.Parameters.Count = 0 Then
                        .WriteLine("No parameter(s) found.")
                    Else
                        For Each parameter In attributes.Parameters
                            .WriteLine(String.Join(": ", parameter))
                        Next
                    End If
                    .WriteLine("*/")
                End If
                If My.Settings.F <> "None" Then
                    .WriteLine(PrintLines(5, " "))
                    .WriteLine("/*")
                    .WriteLine(My.Settings.F + " Formula(s):")
                    .WriteLine(PrintLines(1, "+"))
                    .WriteLine(PrintLines(1, "+"))
                    If attributes.Formulas.Count = 0 Then
                        .WriteLine("No formula(s) found.")
                    Else
                        For Each formula In attributes.Formulas
                            .WriteLine(PrintLines(2, " "))
                            .WriteLine(formula(0) + ":")
                            .WriteLine(PrintLines(1, "-"))
                            .WriteLine(formula(1))
                        Next
                    End If
                    .WriteLine("*/")
                End If
                If My.Settings.RL = "Include" Then
                    .WriteLine(PrintLines(5, " "))
                    .WriteLine("/*")
                    .WriteLine(PrintLines(1, " "))
                    WriteToFileReportLayout(textWriter, New ReportDocument.ReportLayout(attributes.DataDefinition, attributes.ReportDefinition))
                    .WriteLine("*/")
                End If
                .WriteLine(PrintLines(2, " "))
            End With
        End Sub
        Sub WriteToFileReportLayout(textWriter As TextWriter, reportLayoutAttributes As ReportDocument.ReportLayout)
            Dim numbersAndLetters As New ReportDocument.ReportLayout.NumbersAndLetters
            Dim areaSpacer As String = Space("Area: ".Length)
            Dim areaSpacerIndent As String = areaSpacer + Space(5)
            Dim sectionSpacer As String = areaSpacer + Space("Section: ".Length)
            Dim sectionSpacerIndent As String = sectionSpacer + Space(5)
            Dim reportObjectSpacer As String = sectionSpacer + Space("Object: ".Length)
            Dim reportObjectSpacerIndent As String = reportObjectSpacer + Space(5)
            With textWriter
                .WriteLine("Report Layout")
                .WriteLine(PrintLines(2, "+"))
                .WriteLine(PrintLines(1, " "))
                For Each area In reportLayoutAttributes.Areas
                    If area.IsGroupHeader Then
                        numbersAndLetters.IncrementNumber()
                        .WriteLine("Area: " + area.Name + " #" + numbersAndLetters.GetNumber.ToString + " (" + reportLayoutAttributes.GetGroupAreaName(numbersAndLetters.GetNumber - 1) + ")")
                    ElseIf area.IsGroupFooter Then
                        .WriteLine("Area: " + area.Name + " #" + numbersAndLetters.GetNumber.ToString + " (" + reportLayoutAttributes.GetGroupAreaName(numbersAndLetters.GetNumber - 1) + ")")
                        numbersAndLetters.DecrementNumber()
                    Else
                        .WriteLine("Area: " + area.Name)
                    End If
                    If area.EnabledAttributes.Count > 0 Or area.AreaFormulas.Count > 0 Then
                        .WriteLine(PrintLines(1, " "))
                        .WriteLine(areaSpacer + "{")
                        If area.EnabledAttributes.Count > 0 Then
                            .WriteLine(areaSpacerIndent + "Enabled Attributes:")
                            .WriteLine(areaSpacerIndent + "-------------------")
                            For Each enabledAttribute As String In area.EnabledAttributes
                                .WriteLine(areaSpacerIndent + enabledAttribute)
                            Next
                        End If
                        If area.EnabledAttributes.Count > 0 AndAlso area.AreaFormulas.Count > 0 Then .WriteLine(PrintLines(1, " "))
                        If area.AreaFormulas.Count > 0 Then
                            .WriteLine(areaSpacerIndent + "Conditional Formulas:")
                            .WriteLine(areaSpacerIndent + "---------------------")
                            For Each formula In area.AreaFormulas
                                .WriteLine(areaSpacerIndent + String.Join(": ", formula))
                            Next
                        End If
                        .WriteLine(areaSpacer + "}")
                    End If
                    .WriteLine(PrintLines(1, " "))
                    For Each section In area.Sections
                        If area.ContainsOneSection Then
                            .WriteLine(sectionSpacer + "Section: " + section.Name)
                        Else
                            .WriteLine(sectionSpacer + "Section: " + section.Name + " " + numbersAndLetters.GetLetter)
                            numbersAndLetters.IncrementLetterCounter()
                        End If
                        If section.EnabledAttributes.Count > 0 Or section.SectionFormulas.Count > 0 Then
                            .WriteLine(PrintLines(1, " "))
                            .WriteLine(sectionSpacer + "{")
                            If section.EnabledAttributes.Count > 0 Then
                                .WriteLine(sectionSpacerIndent + "Enabled Attributes:")
                                .WriteLine(sectionSpacerIndent + "-------------------")
                                For Each enabledAttribute As String In section.EnabledAttributes
                                    .WriteLine(sectionSpacerIndent + enabledAttribute)
                                Next
                            End If
                            If section.EnabledAttributes.Count > 0 AndAlso section.SectionFormulas.Count > 0 Then .WriteLine(PrintLines(1, " "))
                            If section.SectionFormulas.Count > 0 Then
                                .WriteLine(sectionSpacerIndent + "Conditional Formulas:")
                                .WriteLine(sectionSpacerIndent + "---------------------")
                                For Each formula In section.SectionFormulas
                                    .WriteLine(sectionSpacerIndent + String.Join(": ", formula))
                                Next
                            End If
                            .WriteLine(sectionSpacer + "}")
                        End If
                        .WriteLine(PrintLines(1, " "))
                        For Each reportObject In section.ReportObjects
                            .WriteLine(reportObjectSpacer + "Object: " + reportObject.Name)
                            If reportObject.IsField AndAlso reportObject.GetFieldFormulas(reportObject.Current).Count > 0 Then
                                .WriteLine(PrintLines(1, " "))
                                .WriteLine(reportObjectSpacer + "Field Formulas:")
                                .WriteLine(reportObjectSpacer + "---------------")
                                For Each fieldFormula As String() In reportObject.GetFieldFormulas(reportObject.Current)
                                    .WriteLine(reportObjectSpacer + String.Join(" - ", fieldFormula))
                                Next
                            ElseIf reportObject.IsPicture AndAlso
                                    Not String.IsNullOrEmpty(reportObject.GetGraphicLocation(reportObject.Current)) Then
                                .WriteLine(PrintLines(1, " "))
                                .WriteLine(reportObjectSpacer + "Graphic Location:")
                                .WriteLine(reportObjectSpacer + "---------------")
                                .WriteLine(reportObjectSpacer + reportObject.GetGraphicLocation(reportObject.Current))
                            End If
                            If reportObject.EnabledAttributes.Count > 0 Or reportObject.ReportObjectFormulas.Count > 0 Then
                                .WriteLine(PrintLines(1, " "))
                                .WriteLine(reportObjectSpacer + "{")
                                If reportObject.EnabledAttributes.Count > 0 Then
                                    .WriteLine(reportObjectSpacerIndent + "Enabled Attributes:")
                                    .WriteLine(reportObjectSpacerIndent + "-------------------")
                                    For Each enabledAttribute As String In reportObject.EnabledAttributes
                                        .WriteLine(reportObjectSpacerIndent + enabledAttribute)
                                    Next
                                End If
                                If reportObject.EnabledAttributes.Count > 0 AndAlso reportObject.ReportObjectFormulas.Count > 0 Then .WriteLine(PrintLines(1, " "))
                                If reportObject.ReportObjectFormulas.Count > 0 Then
                                    .WriteLine(reportObjectSpacerIndent + "Conditional Formulas:")
                                    .WriteLine(reportObjectSpacerIndent + "---------------------")
                                    For Each formula In reportObject.ReportObjectFormulas
                                        .WriteLine(reportObjectSpacerIndent + String.Join(": ", formula))
                                    Next
                                End If
                                .WriteLine(reportObjectSpacer + "}")
                            End If
                            .WriteLine(PrintLines(1, " "))
                        Next
                    Next
                    numbersAndLetters.ResetLetterCounter()
                Next
            End With
        End Sub
        Function PrintLines(counter As Integer, text As String)
            Dim numberOfLines As Integer
            Dim stringBuilder As New StringBuilder
            For numberOfLines = 0 To counter - 2
                stringBuilder.Append(text, 100)
                stringBuilder.AppendLine()
            Next
            stringBuilder.Append(text, 100)
            Return stringBuilder.ToString()
        End Function
    End Class
    Class ReportDocument
        Shared Function MakeOneLine(str As String) As String
            Return Replace(Replace(Replace(Trim(str), vbCrLf, " "), vbCr, " "), vbLf, " ")
        End Function
        Shared Function GetEnumName(Of T)(enumValue As T)
            Return [Enum].GetName(GetType(T), enumValue)
        End Function
        Shared Function GetProperName(str As String) As String
            Dim undesireableText As String() = {
            "crFieldValueType",
            "crAreaSectionKind",
            "crReportObjectKind",
            "crSectionAreaConditionFormulaType",
            "crObjectFormatConditionFormulaType",
            "crCommonFieldFormatConditionFormulaType",
            "crDateFieldFormatConditionFormulaType",
            "crDateTimeFieldFormatConditionFormulaType",
            "crNumericFieldFormatConditionFormulaType",
            "crTimeFieldFormatConditionFormulaType",
            "crBooleanFieldFormatConditionFormulaType",
            "crFontColorConditionFormulaType",
            "crSortDirection",
            "Enable",
            "Field",
            "Order"
            }
            Return Regex.Replace(Regex.Replace(str, String.Join("|", undesireableText), ""), "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ")
        End Function
        MustInherit Class Attributes
            Property Title As String
            Property ReportDefinition As rdm.ReportDefinition
            Property DataDefinition As ddm.DataDefinition
            Property Tables As IEnumerable(Of Object())
            Property Parameters As IEnumerable(Of String())
            Property Formulas As IEnumerable(Of String())
            Function GetFieldByteSize(i As Integer)
                Return "[" + Int((i - 2) / 2).ToString + "]"
            End Function
        End Class
        Class ReportAttributes
            Inherits Attributes
            Sub New(r As eng.ReportDocument)
                Title = "Report: " + r.SummaryInfo.ReportTitle
                ReportDefinition = r.ReportClientDocument.ReportDefinition
                DataDefinition = r.ReportClientDocument.DataDefinition
                Tables = From t As ddm.Table
                         In r.ReportClientDocument.Database.Tables
                         Where Tests.Table(t, My.Settings.T)
                         Select {
                            t.Name,
                            t.CommandText,
                            (From tf As ddm.Field
                             In t.DataFields
                             Where Tests.TableField(tf, My.Settings.TF)
                             Select CType({tf.Name, GetProperName(GetEnumName(tf.Type)) + " " + GetFieldByteSize(tf.Length)}, String()))
                         }
                Parameters = From p As ddm.ParameterField
                             In DataDefinition.ParameterFields
                             Where Tests.Parameter(p, My.Settings.P)
                             Select {p.Name, GetProperName(GetEnumName(p.Type))}
                             Distinct
                Formulas = From f As ddm.FormulaField
                           In DataDefinition.FormulaFields
                           Where Tests.Formula(f, My.Settings.F)
                           Select {f.Name, f.Text}
            End Sub
        End Class
        Class SubreportAttributes
            Inherits Attributes
            Sub New(s As ctrl.SubreportClientDocument)
                Title = "Subreport: " + s.Name
                ReportDefinition = s.Document.ReportDefinition
                DataDefinition = s.Document.DataDefinition
                Tables = From t As ddm.Table
                         In s.Document.Database.Tables
                         Where Tests.Table(t, My.Settings.T)
                         Select {
                            t.Name,
                            t.CommandText,
                            (From tf As ddm.Field
                             In t.DataFields
                             Where Tests.TableField(tf, My.Settings.TF)
                             Select CType({tf.Name, GetProperName(GetEnumName(tf.Type)) + " " + GetFieldByteSize(tf.Length)}, String()))
                         }
                Parameters = From p As ddm.ParameterField
                             In DataDefinition.ParameterFields
                             Where Tests.Parameter(p, My.Settings.P)
                             Select {p.Name, GetProperName(GetEnumName(p.Type))}
                             Distinct
                Formulas = From f As ddm.FormulaField
                           In DataDefinition.FormulaFields
                           Where Tests.Formula(f, My.Settings.F)
                           Select {f.Name, f.Text}
            End Sub
        End Class
        Class ReportLayout
            Property Areas As New List(Of Area)
            Property Groups As ddm.Groups
            Property Sorts As ddm.Sorts
            Sub New(dd As ddm.DataDefinition, rd As rdm.ReportDefinition)
                SetAreas(rd.Areas)
                Groups = dd.Groups
                Sorts = dd.Sorts
            End Sub
            Sub SetAreas(rdmAreas As rdm.Areas)
                Dim reportFooterAndPageFooter As List(Of Area)
                For Each rdmArea As rdm.Area In rdmAreas
                    Areas.Add(New Area(rdmArea))
                Next
                reportFooterAndPageFooter = Areas.FindAll(Function(a) a.IsReportFooter Or a.IsPageFooter)
                Areas = Areas.Except(Areas.FindAll(Function(a) a.IsReportFooter Or a.IsPageFooter).AsEnumerable).ToList
                If reportFooterAndPageFooter.Any(Function(a) a.IsReportFooter) Then
                    Areas.Add(reportFooterAndPageFooter.Find(Function(a) a.IsReportFooter))
                End If
                If reportFooterAndPageFooter.Any(Function(a) a.IsPageFooter) Then
                    Areas.Add(reportFooterAndPageFooter.Find(Function(a) a.IsPageFooter))
                End If
            End Sub
            Function GetGroupAreaName(i As Integer)
                Return Groups(i).ConditionField.LongName
            End Function
            Class Area
                Property Current As rdm.Area
                Property Sections As New List(Of Section)
                ReadOnly Property Name As String
                    Get
                        Return GetProperName(GetEnumName(Current.Kind))
                    End Get
                End Property
                ReadOnly Property EnabledAttributes As IEnumerable(Of String) = From p
                                                                                In GetType(rdm.ISCRAreaFormat).GetProperties
                                                                                Where p.GetValue(Current.Format, Nothing).ToString = "True"
                                                                                Select GetProperName(p.Name)
                ReadOnly Property AreaFormulas As IEnumerable(Of String()) = From f
                                                                             In [Enum].GetValues(GetType(rdm.CrSectionAreaFormatConditionFormulaTypeEnum))
                                                                             Where Not String.IsNullOrWhiteSpace(Current.Format.ConditionFormulas.Formula(f).Text)
                                                                             Select {GetProperName([Enum].GetName(GetType(rdm.CrSectionAreaFormatConditionFormulaTypeEnum), f)), MakeOneLine(Current.Format.ConditionFormulas.Formula(f).Text)}
                ReadOnly Property IsGroupHeader As Boolean
                    Get
                        Return Current.Kind = rdm.CrAreaSectionKindEnum.crAreaSectionKindGroupHeader
                    End Get
                End Property
                ReadOnly Property IsGroupFooter As Boolean
                    Get
                        Return Current.Kind = rdm.CrAreaSectionKindEnum.crAreaSectionKindGroupFooter
                    End Get
                End Property
                ReadOnly Property IsPageFooter As Boolean
                    Get
                        Return Current.Kind = rdm.CrAreaSectionKindEnum.crAreaSectionKindPageFooter
                    End Get
                End Property
                ReadOnly Property IsReportFooter As Boolean
                    Get
                        Return Current.Kind = rdm.CrAreaSectionKindEnum.crAreaSectionKindReportFooter
                    End Get
                End Property
                ReadOnly Property ContainsOneSection As Boolean
                    Get
                        Return Sections.Count = 1
                    End Get
                End Property
                Sub New(a As rdm.Area)
                    Current = a
                    SetSections(Current.Sections)
                End Sub
                Sub SetSections(rdmSections As rdm.Sections)
                    For Each rdmSection As rdm.Section In rdmSections
                        Sections.Add(New Section(rdmSection))
                    Next
                End Sub
                Class Section
                    Property Current As rdm.Section
                    Property ReportObjects As New List(Of ReportObject)
                    ReadOnly Property Name As String
                        Get
                            Return GetProperName(GetEnumName(Current.Kind))
                        End Get
                    End Property
                    ReadOnly Property EnabledAttributes As IEnumerable(Of String) = From p
                                                                                    In GetType(rdm.ISCRSectionFormat).GetProperties
                                                                                    Where p.GetValue(Current.Format, Nothing).ToString = "True"
                                                                                    Select GetProperName(p.Name)
                    ReadOnly Property SectionFormulas As IEnumerable(Of String()) = From f
                                                                                    In [Enum].GetValues(GetType(rdm.CrSectionAreaFormatConditionFormulaTypeEnum))
                                                                                    Where Not String.IsNullOrWhiteSpace(Current.Format.ConditionFormulas.Formula(f).Text)
                                                                                    Select {GetProperName([Enum].GetName(GetType(rdm.CrSectionAreaFormatConditionFormulaTypeEnum), f)), MakeOneLine(Current.Format.ConditionFormulas.Formula(f).Text)}
                    Sub New(s As rdm.Section)
                        Current = s
                        SetReportObjects(s.ReportObjects)
                    End Sub
                    Sub SetReportObjects(rdmReportObjects As rdm.ReportObjects)
                        For Each rdmReportObject As rdm.ReportObject In rdmReportObjects
                            ReportObjects.Add(New ReportObject(rdmReportObject))
                        Next
                        ReportObjects.OrderBy(Function(r) r.TopLeftness)
                    End Sub
                    Class ReportObject
                        Property Current As rdm.ReportObject
                        ReadOnly Property Name As String
                            Get
                                If IsText Then
                                    Return MakeOneLine(CType(Current, rdm.TextObject).Text)
                                ElseIf IsFieldHeading Then
                                    Return MakeOneLine(CType(Current, rdm.FieldHeadingObject).Text)
                                ElseIf IsField Then
                                    Return MakeOneLine(CType(Current, rdm.FieldObject).DataSource)
                                ElseIf IsSubreport Then
                                    Return MakeOneLine(CType(Current, rdm.SubreportObject).SubreportName)
                                Else
                                    Return Current.Name
                                End If
                            End Get
                        End Property
                        ReadOnly Property EnabledAttributes As IEnumerable(Of String) = From p
                                                                                        In GetType(rdm.ISCRObjectFormat).GetProperties
                                                                                        Where p.GetValue(Current.Format, Nothing).ToString = "True"
                                                                                        Select GetProperName(p.Name)
                        ReadOnly Property ReportObjectFormulas As IEnumerable(Of String()) = From f
                                                                                             In [Enum].GetValues(GetType(rdm.CrObjectFormatConditionFormulaTypeEnum))
                                                                                             Where Not String.IsNullOrWhiteSpace(Current.Format.ConditionFormulas.Formula(f).Text)
                                                                                             Select {GetProperName([Enum].GetName(GetType(rdm.CrObjectFormatConditionFormulaTypeEnum), f)), MakeOneLine(Current.Format.ConditionFormulas.Formula(f).Text)}
                        ReadOnly Property IsField As Boolean
                            Get
                                Return Current.Kind = rdm.CrReportObjectKindEnum.crReportObjectKindField
                            End Get
                        End Property
                        ReadOnly Property IsFieldHeading As Boolean
                            Get
                                Return Current.Kind = rdm.CrReportObjectKindEnum.crReportObjectKindFieldHeading
                            End Get
                        End Property
                        ReadOnly Property IsText As Boolean
                            Get
                                Return Current.Kind = rdm.CrReportObjectKindEnum.crReportObjectKindText
                            End Get
                        End Property
                        ReadOnly Property IsSubreport As Boolean
                            Get
                                Return Current.Kind = rdm.CrReportObjectKindEnum.crReportObjectKindSubreport
                            End Get
                        End Property
                        ReadOnly Property IsPicture As Boolean
                            Get
                                Return Current.Kind = rdm.CrReportObjectKindEnum.crReportObjectKindPicture
                            End Get
                        End Property
                        ReadOnly Property TopLeftness As Integer
                            Get
                                Return Current.Left + Current.Top
                            End Get
                        End Property
                        Sub New(r As rdm.ReportObject)
                            Current = r
                        End Sub
                        Function GetFieldFormulas(fieldObject As rdm.FieldObject)
                            Dim fieldFormulas As New List(Of String())
                            With fieldObject.FieldFormat.CommonFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrCommonFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrCommonFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FieldFormat.DateFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrDateFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrDateFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FieldFormat.NumericFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrNumericFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrNumericFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FieldFormat.DateTimeFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrDateTimeFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrDateTimeFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FieldFormat.TimeFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrTimeFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrTimeFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FieldFormat.BooleanFormat.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrBooleanFieldFormatConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({GetProperName(GetEnumName(CType(enumValue, rdm.CrBooleanFieldFormatConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            With fieldObject.FontColor.ConditionFormulas
                                For Each enumValue In GetType(rdm.CrFontColorConditionFormulaTypeEnum).GetEnumValues
                                    If Not String.IsNullOrWhiteSpace(.Formula(enumValue).Text) Then
                                        fieldFormulas.Add({"Font " + GetProperName(GetEnumName(CType(enumValue, rdm.CrFontColorConditionFormulaTypeEnum))), MakeOneLine(.Formula(enumValue).Text)})
                                    End If
                                Next
                            End With
                            Return fieldFormulas
                        End Function
                        Function GetGraphicLocation(pictureObject As rdm.PictureObject)
                            Return If(String.IsNullOrWhiteSpace(pictureObject.GraphicLocationFormula.Text), "", pictureObject.GraphicLocationFormula.Text)
                        End Function
                    End Class
                End Class
            End Class
            Class NumbersAndLetters
                Private _letters As Array = "abcdefghijklmnopqrstuvwxyz".ToCharArray
                Private _letterCounter As Integer = 0
                Private _numberCounter As Integer = 0
                Sub IncrementNumber()
                    _numberCounter = _numberCounter + 1
                End Sub
                Sub DecrementNumber()
                    _numberCounter = _numberCounter - 1
                End Sub
                Sub IncrementLetterCounter()
                    _letterCounter = _letterCounter + 1
                End Sub
                Sub ResetLetterCounter()
                    _letterCounter = 0
                End Sub
                Function GetNumber()
                    Return _numberCounter
                End Function
                Function GetLetter()
                    Return _letters(_letterCounter)
                End Function
            End Class
        End Class
        Class Tests
            Shared Function Table(t As ddm.Table, setting As String)
                Select Case setting
                    Case "Command"
                        Return t.ClassName = "CrystalReports.CommandTable"
                    Case "All"
                        Return 1 = 1
                    Case "None"
                        Return 1 = 0
                    Case Else
                        Return Nothing
                End Select
            End Function
            Shared Function Parameter(p As ddm.ParameterField, setting As String)
                Select Case setting
                    Case "Used"
                        Return Not {0, 2}.Contains(p.Usage)
                    Case "All"
                        Return 1 = 1
                    Case "None"
                        Return 1 = 0
                    Case Else
                        Return Nothing
                End Select
            End Function
            Shared Function TableField(tf As ddm.Field, setting As String)
                Select Case setting
                    Case "Used"
                        Return tf.UseCount > 0
                    Case "All"
                        Return 1 = 1
                    Case "None"
                        Return 1 = 0
                    Case Else
                        Return Nothing
                End Select
            End Function
            Shared Function Formula(f As ddm.FormulaField, setting As String)
                Select Case setting
                    Case "Used"
                        Return f.UseCount > 0
                    Case "All"
                        Return 1 = 1
                    Case "None"
                        Return 1 = 0
                    Case Else
                        Return Nothing
                End Select
            End Function
        End Class
    End Class
End Module
