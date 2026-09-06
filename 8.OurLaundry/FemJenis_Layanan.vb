Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FemJenis_Layanan

    Private Sub FemJenis_Layanan_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        TampilData()
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

    Sub TampilData()
        Try
            Koneksi()
            Da = New MySqlDataAdapter("SELECT * FROM jenis_layanan", Conn)
            Ds = New DataSet
            Da.Fill(Ds, "jenis_layanan")
            DataGridView1.DataSource = Ds.Tables("jenis_layanan")

            ' Opsional: Auto resize biar cakep
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .Columns("id_jenis").HeaderText = "ID Jenis"
                .Columns("jenis_layanan_cuci").HeaderText = "Nama Jenis"
                .Columns("harga_perkilo").HeaderText = "Harga/Kilo"
                .Columns("harga_peritem").HeaderText = "Harga/Item"
                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.Aquamarine
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.Aquamarine
                DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                DataGridView1.EnableHeadersVisualStyles = False
                .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
            End With

        Catch ex As Exception
            MessageBox.Show("Gagal menampilkan data: " & ex.Message)
        End Try
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
     Color.MediumSlateBlue,
     Color.MediumOrchid,
     Color.DeepSkyBlue,
     Color.Aquamarine
 }


            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim i As Integer = e.RowIndex
            If i < 0 Then Exit Sub
            txtID.Text = DataGridView1.Item("id_jenis", i).Value.ToString
            txtNama.Text = DataGridView1.Item("jenis_layanan_cuci", i).Value.ToString
            txtKilo.Text = DataGridView1.Item("harga_perkilo", i).Value.ToString
            txtItem.Text = DataGridView1.Item("harga_peritem", i).Value.ToString
        Catch ex As Exception
            ' Jangan tampilkan pesan apa pun agar user tidak terganggu
        End Try
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Aquamarine
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub

    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        ' Pastikan textbox id terisi
        If txtID.Text.Trim() = "" Then
            MessageBox.Show("Pilih data yang akan dihapus terlebih dahulu.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Konfirmasi penghapusan
        Dim tanya = MessageBox.Show("Yakin mau hapus data ini ?", "Konfirmasi",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If tanya = DialogResult.No Then Exit Sub

        Try
            Koneksi()
            Dim sql As String = "DELETE FROM jenis_layanan WHERE id_jenis = @id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)

            Cmd.ExecuteNonQuery()

            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TampilData()
            Bersih()

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
            MessageBox.Show("Terjadi kesalahan: " & ex.Message)

        End Try
    End Sub

    Private Sub Bersih()
        txtID.Clear()
        txtNama.Clear()
        txtKilo.Clear()
        txtItem.Clear()
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimpan.Click
        ' Validasi input kosong
        If txtID.Text.Trim() = "" Or txtNama.Text.Trim() = "" Then
            MessageBox.Show("ID dan Nama Layanan tidak boleh kosong bang!", "Peringatan",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Koneksi()

            ' CEK APAKAH ID SUDAH ADA
            Dim cekIdSql As String = "SELECT COUNT(*) FROM jenis_layanan WHERE id_jenis = @id"
            Cmd = New MySqlCommand(cekIdSql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtID.Text)
            Dim idAda As Integer = Cmd.ExecuteScalar()

            ' Jika id sudah ada → MODE EDIT
            If idAda > 0 Then
                Dim tanya = MessageBox.Show("ID sudah ada. Mau edit data ini bang?", "Konfirmasi Edit",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If tanya = DialogResult.No Then Exit Sub

                ' MODE EDIT
                Dim updateSql As String =
                    "UPDATE jenis_layanan SET jenis_layanan_cuci=@nama, harga_perkilo=@kilo, harga_peritem=@item WHERE id_jenis=@id"

                Cmd = New MySqlCommand(updateSql, Conn)
                Cmd.Parameters.AddWithValue("@id", txtID.Text)
                Cmd.Parameters.AddWithValue("@nama", txtNama.Text)
                Cmd.Parameters.AddWithValue("@kilo", txtKilo.Text)
                Cmd.Parameters.AddWithValue("@item", txtItem.Text)

                Cmd.ExecuteNonQuery()

                MessageBox.Show("Data berhasil diperbarui!", "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                ' MODE TAMBAH DATA BARU
                Dim insertSql As String =
                    "INSERT INTO jenis_layanan (id_jenis, jenis_layanan_cuci, harga_perkilo, harga_peritem) VALUES (@id, @nama, @kilo, @item)"

                Cmd = New MySqlCommand(insertSql, Conn)
                Cmd.Parameters.AddWithValue("@id", txtID.Text)
                Cmd.Parameters.AddWithValue("@nama", txtNama.Text)
                Cmd.Parameters.AddWithValue("@kilo", txtKilo.Text)
                Cmd.Parameters.AddWithValue("@item", txtItem.Text)

                Cmd.ExecuteNonQuery()

                MessageBox.Show("Data baru berhasil disimpan!", "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' Refresh DGV + bersihkan form
            TampilData()
            Bersih()

        Catch ex As MySqlException
            ' KHUSUS kalau ada duplikat UNIQUE KEY
            If ex.Number = 1062 Then
                MessageBox.Show("Nama layanan sudah ada bang! (Duplikat UNIQUE)", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("Kesalahan MySQL: " & ex.Message)
            End If

        Catch ex As Exception
            MessageBox.Show("Kesalahan: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        Bersih()
    End Sub
End Class