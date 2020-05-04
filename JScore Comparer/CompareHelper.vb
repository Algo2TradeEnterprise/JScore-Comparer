Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class CompareHelper
    Implements IDisposable

#Region "Events/Event handlers"
    Public Event DocumentDownloadComplete()
    Public Event DocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
    Public Event Heartbeat(ByVal msg As String)
    Public Event HeartbeatSub(ByVal msg As String)
    Public Event WaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
    'The below functions are needed to allow the derived classes to raise the above two events
    Protected Overridable Sub OnDocumentDownloadComplete()
        RaiseEvent DocumentDownloadComplete()
    End Sub
    Protected Overridable Sub OnDocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
        RaiseEvent DocumentRetryStatus(currentTry, totalTries)
    End Sub
    Protected Overridable Sub OnHeartbeat(ByVal msg As String)
        RaiseEvent Heartbeat(msg)
    End Sub
    Protected Overridable Sub OnHeartbeatSub(ByVal msg As String)
        RaiseEvent HeartbeatSub(msg)
    End Sub
    Protected Overridable Sub OnWaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
        RaiseEvent WaitingFor(elapsedSecs, totalSecs, msg)
    End Sub
#End Region

    Private ReadOnly _cts As CancellationTokenSource
    Private ReadOnly _file1Path As String
    Private ReadOnly _file2Path As String
    Private ReadOnly _fileSchema As Dictionary(Of String, String)

    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal file1Path As String, ByVal file2Path As String)
        _cts = canceller
        _file1Path = file1Path
        _file2Path = file2Path

        _fileSchema = New Dictionary(Of String, String) From
                          {{"Emp No", "EMP_CODE"},
                           {"Field1", "PROJECT_ACQUIRED_SKILL"},
                           {"Field2", "CERTIFIED_SKILL"},
                           {"Field3", "TRAINING_SKILLS"}}
    End Sub

    Public Async Function StartCompareAsync() As Task
        Await Task.Delay(1).ConfigureAwait(False)
        If Not File.Exists(_file1Path) Then Throw New ApplicationException("File 1 is not available at given path")
        If Not File.Exists(_file1Path) Then Throw New ApplicationException("File 2 is not available at given path")

        Dim empScoreDetails As Dictionary(Of String, ScoreDetails) = Nothing
        ReadRequiedDataFromFile(_file1Path, 1, empScoreDetails)
        ReadRequiedDataFromFile(_file2Path, 2, empScoreDetails)
        'Start Comparison
        If empScoreDetails IsNot Nothing AndAlso empScoreDetails.Count > 0 Then
            OnHeartbeat("Comparing score")
            Dim counter As Integer = 0
            For Each runningEmp In empScoreDetails
                _cts.Token.ThrowIfCancellationRequested()
                counter += 1
                OnHeartbeatSub(String.Format("Comparing scores # {0}/{1}", counter, empScoreDetails.Count))
                CompareAllScores(runningEmp.Value)
            Next
            OnHeartbeatSub("")

            _cts.Token.ThrowIfCancellationRequested()
            OnHeartbeat("Writing data to memory")
            Dim columnCount As Integer = (_fileSchema.Count - 1) * 3
            Dim output(empScoreDetails.Count + 1, columnCount)
            'Writing Columns
            Dim clmCtr As Integer = 0
            output(1, clmCtr) = _fileSchema("Emp No")
            For fieldCount As Integer = 1 To _fileSchema.Count - 1
                _cts.Token.ThrowIfCancellationRequested()
                clmCtr += 1
                output(0, clmCtr) = _fileSchema.ElementAt(fieldCount).Value
                output(1, clmCtr) = "Added"
                clmCtr += 1
                output(1, clmCtr) = "Removed"
                If _fileSchema.ElementAt(fieldCount).Key = "Field1" Then
                    clmCtr += 1
                    output(1, clmCtr) = "Updated"
                End If
            Next
            'Writing Data
            Dim rowCtr As Integer = 1
            For Each runningEmp In empScoreDetails
                _cts.Token.ThrowIfCancellationRequested()
                rowCtr += 1
                OnHeartbeatSub(String.Format("Writing data to memory # {0}/{1}", rowCtr - 1, empScoreDetails.Count))
                Dim columnCtr As Integer = 0

                output(rowCtr, columnCtr) = runningEmp.Value.EmpNo
                If runningEmp.Value.Fields IsNot Nothing AndAlso runningEmp.Value.Fields.Count > 0 Then
                    For Each runningField In runningEmp.Value.Fields
                        If runningField.Value.Changes IsNot Nothing Then
                            columnCtr += 1
                            output(rowCtr, columnCtr) = runningField.Value.Changes.Added
                            columnCtr += 1
                            output(rowCtr, columnCtr) = runningField.Value.Changes.Removed
                            If runningField.Key.ToUpper = "FIELD1" Then
                                columnCtr += 1
                                output(rowCtr, columnCtr) = runningField.Value.Changes.Updated
                            End If
                        Else
                            columnCtr += 1
                            columnCtr += 1
                            If runningField.Key.ToUpper = "FIELD1" Then
                                columnCtr += 1
                            End If
                        End If
                    Next
                End If
            Next
            OnHeartbeatSub("")

            _cts.Token.ThrowIfCancellationRequested()
            OnHeartbeat("Opening Excel to write data")
            Dim outputFilename As String = Path.Combine(Path.GetDirectoryName(_file2Path), String.Format("Output {0}.xlsx", Now.ToString("HH_mm_ss")))
            Using xlHlpr As New ExcelHelper(outputFilename, ExcelHelper.ExcelOpenStatus.OpenAfreshForWrite, ExcelHelper.ExcelSaveType.XLS_XLSX, _cts)
                AddHandler xlHlpr.Heartbeat, AddressOf OnHeartbeat
                AddHandler xlHlpr.WaitingFor, AddressOf OnWaitingFor

                OnHeartbeat("Writing data to excel")
                Dim range As String = xlHlpr.GetNamedRange(1, output.GetLength(0) - 1, 1, output.GetLength(1) - 1)
                _cts.Token.ThrowIfCancellationRequested()
                xlHlpr.WriteArrayToExcel(output, range)
                OnHeartbeat(String.Format("Output file available at:{0}", Path.GetDirectoryName(outputFilename)))

                RemoveHandler xlHlpr.Heartbeat, AddressOf OnHeartbeat
                RemoveHandler xlHlpr.WaitingFor, AddressOf OnWaitingFor
            End Using
        End If
    End Function

    Private Sub CompareAllScores(ByRef scoreData As ScoreDetails)
        If scoreData IsNot Nothing AndAlso scoreData.Fields IsNot Nothing AndAlso scoreData.Fields.Count > 0 Then
            For Each runningField In scoreData.Fields
                _cts.Token.ThrowIfCancellationRequested()
                CompareFieldScore(runningField.Value, runningField.Key)
            Next
        End If
    End Sub

    Private Sub CompareFieldScore(ByRef fieldData As Field, ByVal fieldName As String)
        If fieldData IsNot Nothing AndAlso (fieldData.File1Value IsNot Nothing OrElse fieldData.File2Value IsNot Nothing) Then
            Dim seperator As String() = {"),"}
            Dim file1Data() As String = Nothing
            If fieldData.File1Value IsNot Nothing Then
                file1Data = fieldData.File1Value.Split(seperator, StringSplitOptions.RemoveEmptyEntries)
            End If
            _cts.Token.ThrowIfCancellationRequested()
            Dim file2Data() As String = Nothing
            If fieldData.File2Value IsNot Nothing Then
                file2Data = fieldData.File2Value.Split(seperator, StringSplitOptions.RemoveEmptyEntries)
            End If
            If file1Data IsNot Nothing AndAlso file1Data.Count > 0 Then
                For data As Integer = 0 To file1Data.Count - 2
                    file1Data(data) = String.Format("{0})", file1Data(data))
                Next
            End If
            If file2Data IsNot Nothing AndAlso file2Data.Count > 0 Then
                For data As Integer = 0 To file2Data.Count - 2
                    file2Data(data) = String.Format("{0})", file2Data(data))
                Next
            End If
            'Removed Skills
            _cts.Token.ThrowIfCancellationRequested()
            Dim removedSkills As List(Of String) = Nothing
            If file1Data IsNot Nothing AndAlso file1Data.Count > 0 AndAlso file2Data IsNot Nothing AndAlso file2Data.Count > 0 Then
                Dim indexNumber As Integer = 0
                For i As Integer = 0 To file1Data.Count - 1
                    _cts.Token.ThrowIfCancellationRequested()
                    If Not IsSkillNameExists(file1Data(i), file2Data, indexNumber) Then
                        If removedSkills Is Nothing Then removedSkills = New List(Of String)
                        removedSkills.Add(file1Data(i))
                    Else
                        If fieldName.ToUpper <> "FIELD1" Then
                            If Not file2Data.Contains(file1Data(i)) Then
                                If removedSkills Is Nothing Then removedSkills = New List(Of String)
                                removedSkills.Add(file1Data(i))
                            End If
                        End If
                    End If
                Next
            ElseIf file1Data IsNot Nothing AndAlso file1Data.Count > 0 AndAlso (file2Data Is Nothing OrElse file2Data.Count = 0) Then
                For i As Integer = 0 To file1Data.Count - 1
                    _cts.Token.ThrowIfCancellationRequested()
                    If removedSkills Is Nothing Then removedSkills = New List(Of String)
                    removedSkills.Add(file1Data(i))
                Next
            End If
            'Added Skills
            _cts.Token.ThrowIfCancellationRequested()
            Dim addedSkills As List(Of String) = Nothing
            If file1Data IsNot Nothing AndAlso file1Data.Count > 0 AndAlso file2Data IsNot Nothing AndAlso file2Data.Count > 0 Then
                Dim indexNumber As Integer = 0
                For i As Integer = 0 To file2Data.Count - 1
                    _cts.Token.ThrowIfCancellationRequested()
                    If Not IsSkillNameExists(file2Data(i), file1Data, indexNumber) Then
                        If addedSkills Is Nothing Then addedSkills = New List(Of String)
                        addedSkills.Add(file2Data(i))
                    Else
                        If fieldName.ToUpper <> "FIELD1" Then
                            If Not file1Data.Contains(file2Data(i)) Then
                                If addedSkills Is Nothing Then addedSkills = New List(Of String)
                                addedSkills.Add(file2Data(i))
                            End If
                        End If
                    End If
                Next
            ElseIf file2Data IsNot Nothing AndAlso file2Data.Count > 0 AndAlso (file1Data Is Nothing OrElse file1Data.Count = 0) Then
                For i As Integer = 0 To file2Data.Count - 1
                    _cts.Token.ThrowIfCancellationRequested()
                    If addedSkills Is Nothing Then addedSkills = New List(Of String)
                    addedSkills.Add(file2Data(i))
                Next
            End If
            'Updated Skills
            _cts.Token.ThrowIfCancellationRequested()
            Dim updatedSkills As List(Of String) = Nothing
            If file1Data IsNot Nothing AndAlso file1Data.Count > 0 AndAlso file2Data IsNot Nothing AndAlso file2Data.Count > 0 Then
                If fieldName.ToUpper = "FIELD1" Then
                    For i As Integer = 0 To file2Data.Count - 1
                        _cts.Token.ThrowIfCancellationRequested()
                        Dim indexNumber As Integer = 0
                        If IsSkillNameExists(file2Data(i), file1Data, indexNumber) Then
                            If Not file2Data(i).Trim.ToUpper = file1Data(indexNumber).Trim.ToUpper AndAlso Not file1Data.Contains(file2Data(i)) Then
                                Dim scoreUpdate As String = GetScoreUpdate(file1Data(indexNumber), file2Data(i))
                                If scoreUpdate IsNot Nothing Then
                                    If updatedSkills Is Nothing Then updatedSkills = New List(Of String)
                                    updatedSkills.Add(scoreUpdate)
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            fieldData.Changes = New Difference With {
                .Added = GetSkillString(addedSkills),
                .Removed = GetSkillString(removedSkills),
                .Updated = GetSkillString(updatedSkills)
            }
        End If
    End Sub

    Private Function GetSkillString(ByVal skillList As List(Of String)) As String
        Dim ret As String = Nothing
        If skillList IsNot Nothing AndAlso skillList.Count > 0 Then
            Dim skill As String = Nothing
            For Each runningSkill In skillList
                _cts.Token.ThrowIfCancellationRequested()
                skill = String.Format("{0},{1}", skill, runningSkill)
            Next
            ret = skill.Substring(1)
        End If
        Return ret
    End Function

    Private Function GetScoreUpdate(ByVal skillScore1 As String, ByVal skillScore2 As String) As String
        Dim ret As String = Nothing
        Dim score1 As String = GetScore(skillScore1)
        Dim score2 As String = GetScore(skillScore2)
        _cts.Token.ThrowIfCancellationRequested()
        If score1 IsNot Nothing AndAlso score2 IsNot Nothing Then
            If IsNumeric(score1) AndAlso IsNumeric(score2) Then
                If Val(score2) > Val(score1) Then
                    ret = String.Format("(+){0}", skillScore2)
                ElseIf Val(score2) < Val(score1) Then
                    ret = String.Format("(-){0}", skillScore2)
                End If
            Else
                Dim subScore1 As String = score1(1)
                Dim subScore2 As String = score2(1)
                If Val(subScore2) > Val(subScore1) Then
                    ret = String.Format("(+){0}", skillScore2)
                ElseIf Val(subScore2) < Val(subScore1) Then
                    ret = String.Format("(-){0}", skillScore2)
                End If
            End If
        End If
        Return ret
    End Function

    Private Function GetScore(ByVal skillScore As String) As String
        Dim ret As String = ""
        If skillScore IsNot Nothing AndAlso skillScore.Contains("(") AndAlso skillScore.Contains(")") Then
            Dim firstIndex As Integer = skillScore.IndexOf("(")
            Dim secondIndex As Integer = skillScore.IndexOf(")")
            ret = skillScore.Substring(firstIndex + 1, secondIndex - (firstIndex + 1))
        End If
        Return ret
    End Function

    Private Function IsSkillNameExists(ByVal skillNameWithScore As String, ByVal skillDataArray() As String, ByRef indexNumber As Integer) As Boolean
        Dim ret As Boolean = False
        If skillDataArray IsNot Nothing AndAlso skillDataArray.Count > 0 Then
            Dim skillName As String = skillNameWithScore.Substring(0, skillNameWithScore.IndexOf("("))
            For i As Integer = 0 To skillDataArray.Count - 1
                _cts.Token.ThrowIfCancellationRequested()
                Dim runningSkillName As String = skillDataArray(i).Substring(0, skillDataArray(i).IndexOf("("))
                If runningSkillName.Trim.ToUpper = skillName.Trim.ToUpper Then
                    ret = True
                    indexNumber = i
                    Exit For
                End If
            Next
        End If
        Return ret
    End Function

    Private Sub ReadRequiedDataFromFile(ByVal fileName As String, ByVal fileNumber As Integer, ByRef empScoreDetails As Dictionary(Of String, ScoreDetails))
        OnHeartbeat(String.Format("Opening File{0}", fileNumber))
        Using xlHlpr As New ExcelHelper(fileName, ExcelHelper.ExcelOpenStatus.OpenExistingForReadWrite, ExcelHelper.ExcelSaveType.XLS_XLSX, _cts)
            AddHandler xlHlpr.Heartbeat, AddressOf OnHeartbeat
            AddHandler xlHlpr.WaitingFor, AddressOf OnWaitingFor

            Dim allSheets As List(Of String) = xlHlpr.GetExcelSheetsName()
            If allSheets IsNot Nothing AndAlso allSheets.Count > 0 Then
                Dim dataSheet As String = Nothing
                For Each runningSheet In allSheets
                    _cts.Token.ThrowIfCancellationRequested()
                    If runningSheet.Contains("BFSI") Then
                        dataSheet = runningSheet
                        Exit For
                    End If
                Next
                If dataSheet IsNot Nothing Then
                    xlHlpr.SetActiveSheet(dataSheet)
                    OnHeartbeat(String.Format("Checking schema of File{0}", fileNumber))
                    xlHlpr.CheckExcelSchema(_fileSchema.Values.ToArray)
                    xlHlpr.UnFilterSheet(dataSheet)

                    OnHeartbeat(String.Format("Reading data from File{0}", fileNumber))
                    Dim scoreData As Object(,) = xlHlpr.GetExcelInMemory()
                    If scoreData IsNot Nothing Then
                        OnHeartbeatSub(String.Format("Seraching required column numbers from File{0}", fileNumber))
                        Dim empColumnNumber As Integer = GetColumnOf2DArray(scoreData, 1, _fileSchema("Emp No"))
                        Dim dataColumnNumbers As Dictionary(Of String, Integer) = Nothing
                        For Each runningColumn In _fileSchema
                            _cts.Token.ThrowIfCancellationRequested()
                            If Not runningColumn.Key = "Emp No" Then
                                Dim column As Integer = GetColumnOf2DArray(scoreData, 1, runningColumn.Value)

                                If dataColumnNumbers Is Nothing Then dataColumnNumbers = New Dictionary(Of String, Integer)
                                dataColumnNumbers.Add(runningColumn.Key, column)
                            End If
                        Next
                        If dataColumnNumbers IsNot Nothing AndAlso dataColumnNumbers.Count > 0 Then
                            If empScoreDetails Is Nothing Then empScoreDetails = New Dictionary(Of String, ScoreDetails)
                            For rowCounter As Integer = 2 To scoreData.GetLength(0) - 1
                                _cts.Token.ThrowIfCancellationRequested()
                                OnHeartbeatSub(String.Format("Reading required columns data # {0}/{1}", rowCounter - 1, scoreData.GetLength(0) - 2))
                                Dim empID As String = scoreData(rowCounter, empColumnNumber)
                                If empID IsNot Nothing AndAlso empID <> "" Then
                                    Dim fields As Dictionary(Of String, Field) = Nothing
                                    If empScoreDetails.ContainsKey(empID) Then
                                        fields = empScoreDetails(empID).Fields
                                    Else
                                        fields = New Dictionary(Of String, Field)
                                        empScoreDetails.Add(empID, New ScoreDetails With {.EmpNo = empID, .Fields = fields})
                                    End If
                                    For Each runningDataColumn In dataColumnNumbers
                                        _cts.Token.ThrowIfCancellationRequested()
                                        Dim runningField As Field = Nothing
                                        If fields IsNot Nothing AndAlso fields.Count > 0 AndAlso fields.ContainsKey(runningDataColumn.Key) Then
                                            runningField = fields(runningDataColumn.Key)
                                        Else
                                            runningField = New Field
                                            fields.Add(runningDataColumn.Key, runningField)
                                        End If
                                        Dim fieldValue As String = scoreData(rowCounter, runningDataColumn.Value)
                                        If fileNumber = 1 Then
                                            runningField.File1Value = fieldValue
                                        ElseIf fileNumber = 2 Then
                                            runningField.File2Value = fieldValue
                                        End If
                                    Next
                                End If
                            Next
                            OnHeartbeatSub("")
                        End If
                    End If
                Else
                    Throw New ApplicationException(String.Format("BFSI sheet not found on File{0}", fileNumber))
                End If
            End If

            RemoveHandler xlHlpr.Heartbeat, AddressOf OnHeartbeat
            RemoveHandler xlHlpr.WaitingFor, AddressOf OnWaitingFor
        End Using
    End Sub

    Private Function GetColumnOf2DArray(ByVal array As Object(,), ByVal rowNumber As Integer, ByVal searchData As String) As Integer
        Dim ret As Integer = Integer.MinValue
        If array IsNot Nothing AndAlso searchData IsNot Nothing Then
            For column As Integer = 1 To array.GetLength(1)
                _cts.Token.ThrowIfCancellationRequested()
                If array(rowNumber, column) IsNot Nothing AndAlso
                    array(rowNumber, column).ToString.ToUpper = searchData.ToUpper Then
                    ret = column
                    If ret <> Integer.MinValue Then Exit For
                End If
            Next
        End If
        Return ret
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
