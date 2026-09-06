Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmTransaksi
    Private balloonTip As New ToolTip() With {
   .IsBalloon = True,
   .ToolTipIcon = ToolTipIcon.Info,
   .ToolTipTitle = "Information"}
    Private Sub txtPelanggan_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles txtPelanggan.MouseEnter
        balloonTip.Show("Jika kamu lupa dengan ID dan Nama, Tekan saja tombol f1.", txtPelanggan, 0, txtPelanggan.Height, 3000)
    End Sub
    Private Sub txtBarcode_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles txtPelanggan.MouseLeave
        balloonTip.Hide(txtPelanggan)
    End Sub
    Private Sub txtTransaksi_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTransaksi.MouseEnter
        balloonTip.Show("Diharapkan ID Transaksi memiliki kesamaan yang tepat dengan ID Layanan.", txtTransaksi, 0, txtTransaksi.Height, 3000)
    End Sub
    Private Sub txtTransaksi_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles txtTransaksi.MouseLeave
        balloonTip.Hide(txtTransaksi)
    End Sub

    Private Sub FrmTransaksi_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        TampilDataTransaksi()
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
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

    Private Sub TampilDataTransaksi()
        Try
            Call Koneksi()
            Dim sql As String = "SELECT * FROM transaksi ORDER BY id_transaksi ASC"
            Da = New MySqlDataAdapter(sql, Conn)
            Ds = New DataSet
            Da.Fill(Ds, "transaksi")
            DataGridView1.DataSource = Ds.Tables("transaksi")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .MultiSelect = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns("id_transaksi").HeaderText = "ID Transaksi"
                .Columns("no_nota").HeaderText = "No. Nota"
                .Columns("tgl_masuk").HeaderText = "Tanggal Masuk"
                .Columns("id_pelanggan").HeaderText = "ID Pelanggan"
                .Columns("tgl_selesai").HeaderText = "Tanggal Selesai"
                .Columns("status").HeaderText = "Status"
                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.YellowGreen
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.YellowGreen
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
                Color.SandyBrown,
            Color.SlateBlue,
                Color.Olive,
            Color.Azure}



            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
    Handles DataGridView1.CellClick

        If e.RowIndex < 0 Then Exit Sub
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
        If row.IsNewRow OrElse IsDBNull(row.Cells("id_transaksi").Value) Then
            Bersihkan()
            Exit Sub
        End If
        txtTransaksi.Enabled = False
        Try
            txtTransaksi.Text = row.Cells("id_transaksi").Value.ToString()
            txtNota.Text = row.Cells("no_nota").Value.ToString()
            dtpMasuk.Value = CDate(row.Cells("tgl_masuk").Value)
            txtPelanggan.Text = row.Cells("id_pelanggan").Value.ToString()
            dtpSelesai.Value = CDate(row.Cells("tgl_selesai").Value)
            cmbStatus.Text = row.Cells("status").Value.ToString()
        Catch ex As Exception
            MsgBox("Error CellClick: " & ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) _
    Handles DataGridView1.CellFormatting

        If DataGridView1.Columns(e.ColumnIndex).Name = "status" Then

            If e.Value IsNot Nothing Then
                Dim status As String = e.Value.ToString().ToLower()

                Select Case status
                    Case "diterima"
                        e.CellStyle.BackColor = Color.DeepSkyBlue
                        e.CellStyle.ForeColor = Color.Black

                    Case "diproses"
                        e.CellStyle.BackColor = Color.Yellow
                        e.CellStyle.ForeColor = Color.Black

                    Case "selesai"
                        e.CellStyle.BackColor = Color.Lime
                        e.CellStyle.ForeColor = Color.Black

                  
                End Select
            End If
        End If

    End Sub
    Private Sub btnSimpan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimpan.Click

        If txtTransaksi.Text = "" Or txtNota.Text = "" Or txtPelanggan.Text = "" Or cmbStatus.Text = "" Then
            MsgBox("Data belum lengkap!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            '==================================================
            ' MODE TAMBAH DATA (ID masih bisa diedit)
            '==================================================
            If txtTransaksi.Enabled = True Then

                'CEK ID SUDAH ADA ATAU BELUM
                Dim cek As New MySqlCommand("SELECT COUNT(*) FROM transaksi WHERE id_transaksi=@id", Conn)
                cek.Parameters.AddWithValue("@id", txtTransaksi.Text)
                Dim jumlahID As Integer = cek.ExecuteScalar()

                If jumlahID > 0 Then
                    MsgBox("ID Transaksi sudah ada, tidak boleh duplikat!", vbExclamation)
                    Exit Sub
                End If

                'INSERT BARU
                Dim sqlInsert As String =
                    "INSERT INTO transaksi (id_transaksi, no_nota, tgl_masuk, id_pelanggan, tgl_selesai, status) " &
                    "VALUES (@id, @nota, @masuk, @pelanggan, @selesai, @status)"

                Dim cmd As New MySqlCommand(sqlInsert, Conn)
                cmd.Parameters.AddWithValue("@id", txtTransaksi.Text)
                cmd.Parameters.AddWithValue("@nota", txtNota.Text)
                cmd.Parameters.AddWithValue("@masuk", dtpMasuk.Value)
                cmd.Parameters.AddWithValue("@pelanggan", txtPelanggan.Text)
                cmd.Parameters.AddWithValue("@selesai", dtpSelesai.Value)
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text)

                cmd.ExecuteNonQuery()

                MsgBox("Data transaksi berhasil disimpan!", vbInformation)

            Else
                '==================================================
                ' MODE EDIT (ID tidak boleh diedit)
                '==================================================

                Dim sqlUpdate As String =
                    "UPDATE transaksi SET no_nota=@nota, tgl_masuk=@masuk, id_pelanggan=@pelanggan, " &
                    "tgl_selesai=@selesai, status=@status WHERE id_transaksi=@id"

                Dim cmd As New MySqlCommand(sqlUpdate, Conn)
                cmd.Parameters.AddWithValue("@id", txtTransaksi.Text)
                cmd.Parameters.AddWithValue("@nota", txtNota.Text)
                cmd.Parameters.AddWithValue("@masuk", dtpMasuk.Value)
                cmd.Parameters.AddWithValue("@pelanggan", txtPelanggan.Text)
                cmd.Parameters.AddWithValue("@selesai", dtpSelesai.Value)
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text)

                cmd.ExecuteNonQuery()

                MsgBox("Data transaksi berhasil diperbarui!", vbInformation)
            End If

            'REFRESH & BERSIHKAN
            TampilDataTransaksi()
            Bersihkan()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, vbCritical)
        End Try

    End Sub


    Private Sub Bersihkan()
        txtTransaksi.Clear()
        txtNota.Clear()
        txtPelanggan.Clear()
        cmbStatus.SelectedIndex = -1
        dtpMasuk.Value = Date.Now
        dtpSelesai.Value = Date.Now
        txtTransaksi.Enabled = True   'biar bisa isi ID baru kalau perlu
        txtTransaksi.Focus()

    End Sub


    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        Bersihkan()
    End Sub

    Private Sub btnHapus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHapus.Click

        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Pilih minimal satu baris yang ingin dihapus!", vbExclamation)
            Exit Sub
        End If

        Dim jawab = MsgBox("Yakin ingin menghapus " & DataGridView1.SelectedRows.Count & " data?",
                           vbYesNo + vbQuestion, "Konfirmasi")
        If jawab = vbNo Then Exit Sub

        Try
            Call Koneksi()

            For Each row As DataGridViewRow In DataGridView1.SelectedRows

                If Not row.IsNewRow Then
                    Dim id As String = row.Cells("id_transaksi").Value.ToString()

                    Dim cmd As New MySqlCommand("DELETE FROM transaksi WHERE id_transaksi=@id", Conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.ExecuteNonQuery()
                End If

            Next

            MsgBox("Data yang dipilih berhasil dihapus!", vbInformation)

            TampilDataTransaksi()
            Bersihkan()
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
            MsgBox("Error Hapus: " & ex.Message, vbCritical)
        End Try

    End Sub

    Private Sub FrmTransaksi_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            FrmIdPelanggan.ShowDialog()
            FrmIdPelanggan.BringToFront()
        End If
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
   Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.SlateBlue
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub
End Class