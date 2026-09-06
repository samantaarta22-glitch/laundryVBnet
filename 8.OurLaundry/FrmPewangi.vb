Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmPewangi
    Dim R As Integer = 255
    Dim G As Integer = 0
    Dim B As Integer = 0
   

    Private Sub FrmPewangi_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TampilData()
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
    End Sub
    Private Function RoundedRect(ByVal rect As Rectangle, ByVal radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Integer = radius * 2
        Dim arc As New Rectangle(rect.Location, New Size(diameter, diameter))

        ' Kiri atas
        path.AddArc(arc, 180, 90)

        ' Kanan atas
        arc.X = rect.Right - diameter
        path.AddArc(arc, 270, 90)

        ' Kanan bawah
        arc.Y = rect.Bottom - diameter
        path.AddArc(arc, 0, 90)

        ' Kiri bawah
        arc.X = rect.Left
        path.AddArc(arc, 90, 90)

        path.CloseFigure()
        Return path
    End Function

    <DllImport("user32.dll")>
    Private Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.dll")>
    Private Shared Sub SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer)
    End Sub

    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private Const HTCAPTION As Integer = 2
    Private Sub FrmCustomer_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
    End Sub
    Private Sub Panel2_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Panel2.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        End If
    End Sub

    Private Sub TampilData()
        Try
            Call Koneksi()
            Dim sql As String = "SELECT * FROM pewangi ORDER BY id_pewangi ASC"
            Da = New MySqlDataAdapter(sql, Conn)
            Ds = New DataSet
            Da.Fill(Ds, "pewangi")
            DataGridView1.DataSource = Ds.Tables("pewangi")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .MultiSelect = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns("id_pewangi").HeaderText = "ID Pewangi"
                .Columns("nama_pewangi").HeaderText = "Nama Pewangi"
                .Columns("harga_per_pcs").HeaderText = "Harga / Pcs"

                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.Gold
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.Gold
                DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                DataGridView1.EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End With

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
                Color.Cyan,
            Color.Azure,
                Color.Lime,
            Color.Gold}
            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub

        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        Try
            txtID.Text = row.Cells("id_pewangi").Value.ToString()
            txtNama.Text = row.Cells("nama_pewangi").Value.ToString()
            txtKilo.Text = row.Cells("harga_per_pcs").Value.ToString()

        Catch ex As Exception
            MsgBox("Error ambil data: " & ex.Message)
        End Try
    End Sub
    Private Sub bersihkan()
        txtID.Clear()
        txtKilo.Clear()
        txtNama.Clear()
        txtID.Focus()
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        bersihkan()
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimpan.Click
        If txtID.Text = "" Or txtNama.Text = "" Or txtKilo.Text = "" Then
            MsgBox("Semua data harus diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' 🔍 Cek apakah ID sudah ada
            Dim cekSql As String = "SELECT COUNT(*) FROM pewangi WHERE id_pewangi=@id"
            Dim cmdCek As New MySqlCommand(cekSql, Conn)
            cmdCek.Parameters.AddWithValue("@id", txtID.Text)
            Dim jumlah As Integer = CInt(cmdCek.ExecuteScalar())

            Dim sql As String

            If jumlah > 0 Then
                ' 🔥 UPDATE
                sql = "UPDATE pewangi SET nama_pewangi=@nama, harga_per_pcs=@harga WHERE id_pewangi=@id"
            Else
                ' 🔥 INSERT
                sql = "INSERT INTO pewangi (id_pewangi, nama_pewangi, harga_per_pcs) VALUES (@id, @nama, @harga)"
            End If

            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)
            Cmd.Parameters.AddWithValue("@nama", txtNama.Text)
            Cmd.Parameters.AddWithValue("@harga", txtKilo.Text)

            Cmd.ExecuteNonQuery()

            If jumlah > 0 Then
                MsgBox("Data berhasil diupdate!", vbInformation)
            Else
                MsgBox("Data berhasil disimpan!", vbInformation)
            End If

            ' Refresh datagrid
            TampilData()

            ' Bersihkan input
            bersihkan()

        Catch ex As Exception
            MsgBox("Error simpan: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        If txtID.Text = "" Then
            MsgBox("Pilih data yang ingin dihapus!", vbExclamation)
            Exit Sub
        End If

        ' Konfirmasi dulu
        If MsgBox("Yakin ingin menghapus data pewangi ini?", vbYesNo + vbQuestion) = vbNo Then
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' 🔍 Cek apakah ID ini dipakai di tabel layanan
            Dim cekSql As String = "SELECT COUNT(*) FROM layanan WHERE pewangi=@id"
            Dim cmdCek As New MySqlCommand(cekSql, Conn)
            cmdCek.Parameters.AddWithValue("@id", txtID.Text)
            Dim jumlah As Integer = CInt(cmdCek.ExecuteScalar())

            If jumlah > 0 Then
                MsgBox("Tidak bisa menghapus! Pewangi ini masih digunakan pada data layanan.", vbCritical)
                Exit Sub
            End If

            ' 🔥 Jika aman → HAPUS
            Dim sql As String = "DELETE FROM pewangi WHERE id_pewangi=@id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)
            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil dihapus!", vbInformation)

            ' Refresh tabel
            TampilData()

            ' Bersihkan input
            bersihkan()
        Catch ex As MySqlException
            ' HANDLE FOREIGN KEY ERROR
            If ex.Number = 1451 Then
                MessageBox.Show("Data tidak bisa dihapus karena sedang digunakan di tabel lain, coba diperiksa lagi!",
                                "Gagal Menghapus", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            Else
                MessageBox.Show("Kesalahan MySQL: " & ex.Message)
            End If

        Catch ex As Exception
            MsgBox("Error hapus: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Gold
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub
End Class