Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmPelayanan
    Private Sub FrmPelayanan_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TampilData2()
        tampildatalayanancmb()
        TampilDataLayanan()
        IsiComboPewangi()
        Panel2.Cursor = Cursors.NoMove2D
        txtHargaKilo.ReadOnly = True
        txtHargaItem.ReadOnly = True
        txtHargaPewangi.ReadOnly = True
        cmbLayanan.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPewangi.DropDownStyle = ComboBoxStyle.DropDownList
        DesainButton(btnUpdate)
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
        btnUpdate.Cursor = Cursors.Hand
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
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

    Private Sub cmbLayanan_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbLayanan.SelectedIndexChanged
        Call Koneksi()

        Dim layananDipilih As String = cmbLayanan.Text

        Cmd = New MySqlCommand("SELECT harga_perkilo, harga_peritem FROM jenis_layanan WHERE jenis_layanan_cuci=@layanan", Conn)
        Cmd.Parameters.AddWithValue("@layanan", layananDipilih)

        Rd = Cmd.ExecuteReader

        If Rd.Read() Then
            txtHargaKilo.Text = Rd("harga_perkilo").ToString()
            txtHargaItem.Text = Rd("harga_peritem").ToString()
        End If

        Rd.Close()
    End Sub
    Private Sub tampildatalayanancmb()
        Call Koneksi()

        Cmd = New MySqlCommand("SELECT * FROM jenis_layanan", Conn)
        Rd = Cmd.ExecuteReader

        cmbLayanan.Items.Clear()

        While Rd.Read
            cmbLayanan.Items.Add(Rd("jenis_layanan_cuci").ToString())
        End While
        cmbLayanan.Text = "-- Pilih Layanan --"
        Rd.Close()
    End Sub

    Sub TampilDataLayanan()
        Call Koneksi()

        Da = New MySqlDataAdapter("SELECT * FROM layanan", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "layanan")
        DataGridView1.DataSource = Ds.Tables("layanan")
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        With DataGridView1
            .Columns("id_layanan").HeaderText = "ID Layanan"
            .Columns("nama_layanan").HeaderText = "Nama Layanan"
            .Columns("harga_perkilo").HeaderText = "Harga/Kilo"
            .Columns("harga_peritem").HeaderText = "Harga/Item"
            .Columns("pewangi").HeaderText = "Pewangi"
            .Columns("jumlah_per_pcs").HeaderText = "Jumlah Pewangi/Pcs"
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.Black
            .DefaultCellStyle.SelectionForeColor = Color.LightSteelBlue
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.LightSteelBlue
            DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("Palatino Linotype", 10, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        End With
    End Sub

    Sub TampilData2(Optional ByVal keyword As String = "")
        Try
            Call Koneksi()

            Dim sql As String = " SELECT id_layanan, nama_layanan, harga_perkilo, harga_peritem, pewangi, jumlah_per_pcs FROM layanan"

        If keyword <> "" Then
            sql &= " WHERE id_layanan LIKE @key OR nama_layanan LIKE @key"
                End If

        Da = New MySqlDataAdapter(sql, Conn)

        If keyword <> "" Then
            Da.SelectCommand.Parameters.AddWithValue("@key", "%" & keyword & "%")
                End If

        Ds = New DataSet
        Da.Fill(Ds, "layanan")
        DataGridView1.DataSource = Ds.Tables("layanan")

    Catch ex As Exception
        MsgBox("Gagal menampilkan data: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub txtCari_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCari.TextChanged
        TampilData2(txtCari.Text)
    End Sub

    Sub IsiComboPewangi()
        Call Koneksi()

        Da = New MySqlDataAdapter("SELECT * FROM pewangi", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "pewangi")

        cmbPewangi.DataSource = Ds.Tables("pewangi")
        cmbPewangi.DisplayMember = "nama_pewangi"
        cmbPewangi.ValueMember = "id_pewangi"
        cmbPewangi.SelectedIndex = -1
        cmbPewangi.Text = "-- Pilih Pewangi --"

    End Sub
    Private Sub cmbPewangi_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbPewangi.SelectedIndexChanged
        If cmbPewangi.SelectedIndex = -1 Then Exit Sub 'kalau belum pilih apa-apa jangan error

        Call Koneksi()

        Cmd = New MySqlCommand("SELECT harga_per_pcs FROM pewangi WHERE id_pewangi=@id", Conn)
        Cmd.Parameters.AddWithValue("@id", cmbPewangi.SelectedValue.ToString())

        Rd = Cmd.ExecuteReader
        If Rd.Read() Then
            txtHargaPewangi.Text = Rd("harga_per_pcs").ToString()
        End If
        Rd.Close()
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Sub Bersihkan()
        ' Textbox
        txtID.Clear()
        txtHargaKilo.Clear()
        txtHargaItem.Clear()
        txtJumlahPewangi.Clear()
        txtHargaPewangi.Clear()

        ' ComboBox
        cmbLayanan.SelectedIndex = -1
        cmbLayanan.Text = "-- Pilih Layanan --"

        cmbPewangi.SelectedIndex = -1
        cmbPewangi.Text = "-- Pilih Pewangi --"

        ' Fokus ke input pertama
        cmbLayanan.Focus()
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        Bersihkan()
        txtID.Focus()
    End Sub

    Private Sub btnHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHapus.Click
        If txtId.Text = "" Then
            MsgBox("Pilih data yang mau dihapus!", vbExclamation)
            Exit Sub
        End If

        If MsgBox("Yakin hapus data ini?", vbQuestion + vbYesNo, "Konfirmasi") = vbNo Then
            Exit Sub
        End If

        Try
            Call Koneksi() 'module koneksi lu

            Dim sql As String = "DELETE FROM layanan WHERE id_layanan=@id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtId.Text)

            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil dihapus!", vbInformation)

            Bersihkan()

            'Refresh DGV
            TampilDataLayanan()
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
            MsgBox("Gagal menghapus data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        If DataGridView1.Rows(e.RowIndex).IsNewRow Then
            Bersihkan()
            Exit Sub
        End If

        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        txtID.Text = row.Cells("id_layanan").Value.ToString()
        cmbLayanan.Text = row.Cells("nama_layanan").Value.ToString()
        txtHargaKilo.Text = row.Cells("harga_perkilo").Value.ToString()
        txtHargaItem.Text = row.Cells("harga_peritem").Value.ToString()

        ' INI YANG PENTING ‼ 
        cmbPewangi.SelectedValue = row.Cells("pewangi").Value.ToString()

        txtJumlahPewangi.Text = row.Cells("jumlah_per_pcs").Value.ToString()
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimpan.Click

        If txtID.Text = "" Or cmbLayanan.Text = "" Or txtHargaKilo.Text = "" Or txtHargaItem.Text = "" Or cmbPewangi.Text = "" Or txtJumlahPewangi.Text = "" Then
            MsgBox("Data belum lengkap", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            '==============================
            ' CEK ID SUDAH ADA ATAU BELUM
            '==============================
            Dim cek As New MySqlCommand("SELECT COUNT(*) FROM layanan WHERE id_layanan=@id", Conn)
            cek.Parameters.AddWithValue("@id", txtID.Text)

            Dim jumlahID As Integer = cek.ExecuteScalar()

            If jumlahID > 0 Then
                MsgBox("ID sudah ada! Tidak boleh duplikat!", vbCritical)
                Exit Sub
            End If

            '==============================
            ' INSERT BARU
            '==============================
            Dim sql As String =
                "INSERT INTO layanan (id_layanan, nama_layanan, harga_perkilo, harga_peritem, pewangi, jumlah_per_pcs) " &
                "VALUES (@id, @layanan, @hargakilo, @hargaitem, @pewangi, @jumlah)"

            Dim cmd As New MySqlCommand(sql, Conn)
            cmd.Parameters.AddWithValue("@id", txtID.Text)
            cmd.Parameters.AddWithValue("@layanan", cmbLayanan.Text)
            cmd.Parameters.AddWithValue("@hargakilo", txtHargaKilo.Text)
            cmd.Parameters.AddWithValue("@hargaitem", txtHargaItem.Text)
            cmd.Parameters.AddWithValue("@pewangi", cmbPewangi.SelectedValue)
            cmd.Parameters.AddWithValue("@jumlah", txtJumlahPewangi.Text)

            cmd.ExecuteNonQuery()

            MsgBox("Data berhasil disimpan", vbInformation)

            TampilDataLayanan()
            Bersihkan()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        End Try

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        If txtID.Text = "" Then
            MsgBox("Pilih data dulu dari tabel!", vbExclamation)
            Exit Sub
        End If

        If cmbLayanan.Text = "" Or txtHargaKilo.Text = "" Or txtHargaItem.Text = "" Or cmbPewangi.Text = "" Or txtJumlahPewangi.Text = "" Then
            MsgBox("Data belum lengkap", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()
            Dim cek As New MySqlCommand("SELECT COUNT(*) FROM layanan WHERE id_layanan=@id", Conn)
            cek.Parameters.AddWithValue("@id", txtID.Text)

            Dim jumlahID As Integer = cek.ExecuteScalar()

            If jumlahID = 0 Then
                MsgBox("ID tidak ditemukan, tidak bisa update!", vbCritical)
                Exit Sub
            End If
            Dim sql As String =
                "UPDATE layanan SET " &
                "nama_layanan=@layanan, " &
                "harga_perkilo=@hargakilo, " &
                "harga_peritem=@hargaitem, " &
                "pewangi=@pewangi, " &
                "jumlah_per_pcs=@jumlah " &
                "WHERE id_layanan=@id"

            Dim cmd As New MySqlCommand(sql, Conn)
            cmd.Parameters.AddWithValue("@id", txtID.Text)
            cmd.Parameters.AddWithValue("@layanan", cmbLayanan.Text)
            cmd.Parameters.AddWithValue("@hargakilo", txtHargaKilo.Text)
            cmd.Parameters.AddWithValue("@hargaitem", txtHargaItem.Text)
            cmd.Parameters.AddWithValue("@pewangi", cmbPewangi.SelectedValue)
            cmd.Parameters.AddWithValue("@jumlah", txtJumlahPewangi.Text)

            cmd.ExecuteNonQuery()

            MsgBox("Data berhasil diperbarui!", vbInformation)

            TampilDataLayanan()
            Bersihkan()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        End Try

    End Sub

    Private Sub DesainButton(ByVal btn As Button)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = Color.FromArgb(30, 144, 255) 'DodgerBlue
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI", 8, FontStyle.Bold)

        btn.TextAlign = ContentAlignment.MiddleRight
        btn.ImageAlign = ContentAlignment.MiddleLeft

        btn.Padding = New Padding(10, 0, 10, 0) 'biar teks ada jarak
    End Sub

    Private Sub Panel1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel1.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
                Color.LightYellow,
            Color.SlateBlue,
                Color.DarkBlue,
            Color.Ivory}
            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
  Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.LightYellow
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub
End Class
