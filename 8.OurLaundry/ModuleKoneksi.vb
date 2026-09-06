Imports MySql.Data.MySqlClient

Module ModuleKoneksi
    Public Conn As MySqlConnection
    Public Cmd As MySqlCommand
    Public Da As MySqlDataAdapter
    Public Ds As DataSet
    Public Rd As MySqlDataReader
    Public LokasiDB As String
    Public cSql As String

    Sub Koneksi()
        LokasiDB = "server=localhost;userid=root;password=;database=laundryku"
        Conn = New MySqlConnection(LokasiDB)
        Try
            If Conn.State = ConnectionState.Closed Then
                Conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi Gagal: " & ex.Message)
        End Try
    End Sub
End Module
