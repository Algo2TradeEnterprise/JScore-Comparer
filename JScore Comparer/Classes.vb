Public Class ScoreDetails
    Public Property EmpNo As String
    Public Property Fields As Dictionary(Of String, Field)
End Class

Public Class Field
    Public Property File1Value As String
    Public Property File2Value As String
    Public Property Changes As Difference
End Class

Public Class Difference
    Public Property Added As String
    Public Property Removed As String
    Public Property Updated As String
End Class