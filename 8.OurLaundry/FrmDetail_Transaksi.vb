Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class FrmDetail_Transaksi
    Dim hargaKilo As Integer = 0
    Dim hargaItem As Integer = 0
    Dim idPewangi As String = ""
    Dim hargaPewangi As Integer = 0
    Dim hargaPerPcs As Integer = 0
    Dim jumlahPerPcs As Integer = 0
    Private balloonTip As New ToolTip() With {
  .IsBalloon = True,
  .ToolTipIcon = ToolTipIcon.Info,
  .ToolTipTitle = "Information"}
    Private Sub txtSubtotal_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSubtotal.MouseEnter
        balloonTip.Show("Total Harga Satuan + Total Harga Pewangi.", txtSubtotal, 0, txtSubtotal.Height, 3000)
    End Sub
    Private Sub txtSubtotal_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles txtSubtotal.MouseLeave
        balloonTip.Hide(txtSubtotal)
    End Sub

    Private Sub FrmDetail_Transaksi_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        TampilData()
        txtSatuan.ReadOnly = True
        txtPewangi.ReadOnly = True
        txtTotal.ReadOnly = True
        txtSubtotal.ReadOnly = True
        txtNamaPewangi.ReadOnly = True
        Label11.Cursor = Cursors.Hand
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
        btnUpdate.Cursor = Cursors.Hand
        Label11.Visible = False
        PictureBox1.Visible = False
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

    Private Sub Panel3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel3.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel3.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.DeepSkyBlue, Color.DarkSlateBlue, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = New Color() {
                Color.Crimson,
                Color.Orange,
                Color.Gold,
                Color.DeepPink}
            blend.Positions = New Single() {0.0F, 0.3F, 0.7F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub

    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub txtLayanan_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLayanan.TextChanged
        If txtLayanan.Text = "" Then
            hargaKilo = 0
            hargaItem = 0
            idPewangi = ""
            jumlahPerPcs = 0
            hargaPerPcs = 0
            txtPewangi.Text = ""
            txtTotal.Text = ""
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' 🔍 Ambil data layanan termasuk jumlah_per_pcs & id_penyangi
            Dim sql As String = "SELECT harga_perkilo, harga_peritem, pewangi, jumlah_per_pcs  FROM layanan WHERE id_layanan=@id"

            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtLayanan.Text)
            Rd = Cmd.ExecuteReader()

            If Rd.Read() Then
                hargaKilo = Rd("harga_perkilo")
                hargaItem = Rd("harga_peritem")
                idPewangi = Rd("pewangi").ToString()
                jumlahPerPcs = Val(Rd("jumlah_per_pcs"))

                txtPewangi.Text = idPewangi          ' otomatis isi ID pewangi
            Else
                hargaKilo = 0
                hargaItem = 0
                jumlahPerPcs = 0
                idPewangi = ""
                txtPewangi.Text = ""
            End If
            Rd.Close()

            ' 🔍 Ambil harga per pcs dari tabel pewangi
            Dim sqlPw As String = "SELECT harga_per_pcs FROM pewangi WHERE id_pewangi=@idp"
            Cmd = New MySqlCommand(sqlPw, Conn)
            Cmd.Parameters.AddWithValue("@idp", idPewangi)
            Rd = Cmd.ExecuteReader()

            If Rd.Read() Then
                hargaPerPcs = Rd("harga_per_pcs")
            Else
                hargaPerPcs = 0
            End If
            Rd.Close()

            ' 🧮 Hitung total langsung (jumlah_per_pcs × harga_per_pcs)
            Dim total = jumlahPerPcs * hargaPerPcs
            txtTotal.Text = total.ToString()

        Catch ex As Exception
            MsgBox("Error ambil layanan: " & ex.Message)
        End Try
    End Sub

    Private Sub cmbHarga_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbHarga.SelectedIndexChanged
        If cmbHarga.Text = "Kilo" Then
            txtSatuan.Text = hargaKilo.ToString()
        ElseIf cmbHarga.Text = "Item" Then
            txtSatuan.Text = hargaItem.ToString()
        End If
    End Sub

    Private Sub txtPewangi_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPewangi.TextChanged
        If txtPewangi.Text = "" Then
            hargaPerPcs = 0
            Exit Sub
        End If

        Try
            Call Koneksi()
            Dim sql As String = "SELECT harga_per_pcs, nama_pewangi FROM pewangi WHERE id_pewangi=@id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtPewangi.Text)
            Rd = Cmd.ExecuteReader()

            If Rd.Read() Then

                hargaPerPcs = CInt(Rd("harga_per_pcs"))

                ' kalau lu mau nampilin nama pewangi:
                txtNamaPewangi.Text = Rd("nama_pewangi").ToString()

            Else
                hargaPerPcs = 0
                txtNamaPewangi.Text = ""
            End If

            Rd.Close()

            ' 🔥 Setelah harga & jumlah ketemu → HITUNG TOTAL
            HitungTotalPewangi()

        Catch ex As Exception
            MsgBox("Error ambil data pewangi: " & ex.Message)
        End Try
    End Sub
    Private Sub HitungTotalPewangi()
        Dim total As Integer = jumlahPerPcs * hargaPerPcs
        txtTotal.Text = total.ToString()
    End Sub

    Private Sub txtJumlah_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtJumlah.TextChanged
        If txtJumlah.Text = "" Or txtSatuan.Text = "" Then
            txtSubtotal.Text = ""
            Exit Sub
        End If

        Dim jumlah As Integer
        Dim satuan As Integer
        Dim totalPewangi As Integer = 0

        ' Ambil total pewangi dari txtTotal
        Integer.TryParse(txtTotal.Text, totalPewangi)

        ' Convert ke angka
        If Integer.TryParse(txtJumlah.Text, jumlah) AndAlso Integer.TryParse(txtSatuan.Text, satuan) Then

            ' Hitung subtotal layanan
            Dim subtotalLayanan As Integer = jumlah * satuan

            ' 🔥 Subtotal akhir = subtotal layanan + total pewangi
            Dim subtotalAkhir As Integer = subtotalLayanan + totalPewangi

            txtSubtotal.Text = subtotalAkhir.ToString()

        Else
            txtSubtotal.Text = ""
        End If
    End Sub
    Private Sub txtJumlah_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtJumlah.KeyPress
        ' Hanya angka
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            Exit Sub
        End If

        ' Maksimal 4 digit
        If Char.IsDigit(e.KeyChar) AndAlso txtJumlah.TextLength >= 4 Then
            e.Handled = True
        End If
    End Sub

    Private Sub TampilData()
        Try
            Call Koneksi()
            Dim sql As String = "SELECT * FROM detail_transaksi ORDER BY id_detail ASC"

            Da = New MySqlDataAdapter(sql, Conn)
            Ds = New DataSet
            Da.Fill(Ds, "detail_transaksi")
            DataGridView1.DataSource = Ds.Tables("detail_transaksi")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .MultiSelect = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns("id_detail").HeaderText = "ID Detail"
                .Columns("id_transaksi").HeaderText = "ID Transaksi"
                .Columns("id_layanan").HeaderText = "ID Layanan"
                .Columns("jenis_harga").HeaderText = "Jenis Harga"
                .Columns("jumlah").HeaderText = "Jumlah"
                .Columns("harga_satuan").HeaderText = "Harga Satuan"
                .Columns("id_pewangi").HeaderText = "ID Pewangi"
                .Columns("total_pewangi").HeaderText = "Total Pewangi"
                .Columns("subtotal").HeaderText = "Subtotal"

                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.Orange
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.Orange
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
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
     Handles DataGridView1.CellClick

        ' ⛔ Kalau klik header, langsung keluar
        If e.RowIndex < 0 Then Exit Sub

        ' ✅ Kalau klik baris kosong paling bawah → panggil Bersih()
        If e.RowIndex = DataGridView1.NewRowIndex Then
            Bersih()
            Exit Sub
        End If

        ' 🔥 Kalau bukan baris kosong → isi kontrol seperti biasa
        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        txtDetail.Text = row.Cells("id_detail").Value.ToString()
        txtTransaksi.Text = row.Cells("id_transaksi").Value.ToString()
        txtLayanan.Text = row.Cells("id_layanan").Value.ToString()
        cmbHarga.Text = row.Cells("jenis_harga").Value.ToString()
        txtJumlah.Text = row.Cells("jumlah").Value.ToString()
        txtSatuan.Text = row.Cells("harga_satuan").Value.ToString()
        txtPewangi.Text = row.Cells("id_pewangi").Value.ToString()
        txtTotal.Text = row.Cells("total_pewangi").Value.ToString()
        txtSubtotal.Text = row.Cells("subtotal").Value.ToString()

    End Sub

    Private Sub Bersih()
        txtDetail.Clear()
        txtTransaksi.Clear()
        txtLayanan.Clear()
        cmbHarga.SelectedIndex = -1
        txtJumlah.Clear()
        txtSatuan.Clear()
        txtPewangi.Clear()
        txtTotal.Clear()
        txtSubtotal.Clear()
        txtNamaPewangi.Clear()

        ' Jika mau cursor kembali fokus ke ID
        txtDetail.Focus()
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        Bersih()
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        ' Validasi input
        If txtDetail.Text = "" Then
            MsgBox("ID Detail wajib diisi!", vbExclamation)
            Exit Sub
        End If

        If txtTransaksi.Text = "" Or txtLayanan.Text = "" Or cmbHarga.Text = "" Or txtJumlah.Text = "" Then
            MsgBox("Semua field wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' =====================================================
            ' 🔍 CEK APAKAH ID_DETAIL SUDAH ADA
            ' =====================================================
            Dim sqlCek As String = "SELECT COUNT(*) FROM detail_transaksi WHERE id_detail=@id"
            Cmd = New MySqlCommand(sqlCek, Conn)
            Cmd.Parameters.AddWithValue("@id", txtDetail.Text)

            Dim jumlahData As Integer = CInt(Cmd.ExecuteScalar())

            If jumlahData > 0 Then
                MsgBox("ID Detail sudah digunakan, tidak boleh duplikat!", vbCritical)
                Exit Sub
            End If

            ' =====================================================
            ' 💾 SIMPAN DATA (INSERT)
            ' =====================================================
            Dim sqlInsert As String =
                "INSERT INTO detail_transaksi (id_detail, id_transaksi, id_layanan, jenis_harga, jumlah, harga_satuan, id_pewangi, total_pewangi, subtotal) VALUES (@detail, @transaksi, @layanan, @harga, @jumlah, @satuan, @pewangi, @totalPewangi, @subtotal)"

            Cmd = New MySqlCommand(sqlInsert, Conn)
            Cmd.Parameters.AddWithValue("@detail", txtDetail.Text)
            Cmd.Parameters.AddWithValue("@transaksi", txtTransaksi.Text)
            Cmd.Parameters.AddWithValue("@layanan", txtLayanan.Text)
            Cmd.Parameters.AddWithValue("@harga", cmbHarga.Text)
            Cmd.Parameters.AddWithValue("@jumlah", txtJumlah.Text)
            Cmd.Parameters.AddWithValue("@satuan", txtSatuan.Text)
            Cmd.Parameters.AddWithValue("@pewangi", txtPewangi.Text)
            Cmd.Parameters.AddWithValue("@totalPewangi", txtTotal.Text)
            Cmd.Parameters.AddWithValue("@subtotal", txtSubtotal.Text)

            Cmd.ExecuteNonQuery()

            MsgBox("Data detail transaksi berhasil disimpan!", vbInformation)

            TampilData()
            Bersih()

        Catch ex As Exception
            MsgBox("Error simpan data: " & ex.Message, vbCritical)
        End Try
    End Sub

   Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ' Validasi input
        If txtDetail.Text = "" Then
            MsgBox("ID Detail wajib dipilih untuk update!", vbExclamation)
            Exit Sub
        End If

        ' ===============================
        ' 🔒 CEK APAKAH USER MENGUBAH ID_DETAIL
        ' ===============================
        ' Ambil ID detail asli dari baris yang dipilih di DataGridView
        Dim idAsli As String = DataGridView1.CurrentRow.Cells("id_detail").Value.ToString()

        If txtDetail.Text <> idAsli Then
            MsgBox("ID Detail tidak bisa di edit!", vbCritical)
            txtDetail.Text = idAsli
            Exit Sub
        End If

        If txtTransaksi.Text = "" Or txtLayanan.Text = "" Or cmbHarga.Text = "" Or txtJumlah.Text = "" Then
            MsgBox("Semua field wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            ' =====================================================
            ' ✏ UPDATE DATA (kecuali id_detail)
            ' =====================================================
            Dim sqlUpdate As String =
                "UPDATE detail_transaksi SET id_transaksi=@transaksi, id_layanan=@layanan, jenis_harga=@harga, jumlah=@jumlah, harga_satuan=@satuan, id_pewangi=@pewangi, total_pewangi=@totalPewangi,  subtotal=@subtotal WHERE id_detail=@detail"

            Cmd = New MySqlCommand(sqlUpdate, Conn)
            Cmd.Parameters.AddWithValue("@detail", idAsli)
            Cmd.Parameters.AddWithValue("@transaksi", txtTransaksi.Text)
            Cmd.Parameters.AddWithValue("@layanan", txtLayanan.Text)
            Cmd.Parameters.AddWithValue("@harga", cmbHarga.Text)
            Cmd.Parameters.AddWithValue("@jumlah", txtJumlah.Text)
            Cmd.Parameters.AddWithValue("@satuan", txtSatuan.Text)
            Cmd.Parameters.AddWithValue("@pewangi", txtPewangi.Text)
            Cmd.Parameters.AddWithValue("@totalPewangi", txtTotal.Text)
            Cmd.Parameters.AddWithValue("@subtotal", txtSubtotal.Text)

            Cmd.ExecuteNonQuery()

            MsgBox("Data detail transaksi berhasil diupdate!", vbInformation)

            TampilData()
            Bersih()

        Catch ex As Exception
            MsgBox("Error update data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        If txtDetail.Text = "" Then
            MsgBox("Pilih data dulu dari tabel sebelum menghapus!", vbExclamation)
            Exit Sub
        End If

        ' Konfirmasi hapus
        Dim tanya = MsgBox("Yakin ingin menghapus data ini?", vbYesNo + vbQuestion, "Konfirmasi")
        If tanya = vbNo Then Exit Sub

        Try
            Call Koneksi()

            Dim sqlDelete As String = "DELETE FROM detail_transaksi WHERE id_detail=@detail"
            Cmd = New MySqlCommand(sqlDelete, Conn)
            Cmd.Parameters.AddWithValue("@detail", txtDetail.Text)

            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil dihapus!", vbInformation)

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
            MsgBox("Error hapus data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub Label11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label11.Click
        Dim f As New FrmHelp
        f.ShowDialog()

    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
 Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter, btnUpdate.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Orange
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave, btnUpdate.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.Orange
    End Sub
End Class