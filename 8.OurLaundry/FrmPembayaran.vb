Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop


Public Class FrmPembayaran
    Private balloonTip As New ToolTip() With {
 .IsBalloon = True,
 .ToolTipIcon = ToolTipIcon.Info,
 .ToolTipTitle = "Information"}
    Private Sub txtTransaksi_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTransaksi.MouseEnter
        balloonTip.Show("Jika Anda Lupa dengan ID Detail, tekan saja tombol F1.", txtTransaksi, 0, txtTransaksi.Height, 3000)
    End Sub
    Private Sub txtTransaksi_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles txtTransaksi.MouseLeave
        balloonTip.Hide(txtTransaksi)
    End Sub
    Private Sub btnExcel_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.MouseEnter
        balloonTip.Show("Data ini akan dimasukkan ke Excel Worksheet sebagai laporan.", btnExcel, 0, btnExcel.Height, 3000)
    End Sub
    Private Sub btnExcel_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.MouseLeave
        balloonTip.Hide(btnExcel)
    End Sub
    Private Sub FrmPemnayaran_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim radius As Integer = 20 ' Semakin besar, semakin melengkung
        Me.Region = New Region(RoundedRect(New Rectangle(0, 0, Me.Width, Me.Height), radius))
        TampilData()
        TampilKaryawan()
        Panel1.BackColor = Color.Honeydew
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        PictureBox5.Visible = False
        PictureBox6.Visible = False
        PictureBox7.Visible = False
        PictureBox8.Visible = False
        PictureBox9.Visible = False
        PictureBox10.Visible = False
        Label8.Visible = False
        Label9.Visible = False
        Label10.Visible = False
        Label12.Visible = False
        Label13.Visible = False
        Panel1.BackColor = Color.Black
        Panel4.BackColor = Color.White
        btnSimpan.Cursor = Cursors.Hand
        btnHapus.Cursor = Cursors.Hand
        btnBatal.Cursor = Cursors.Hand
        btnTutup.Cursor = Cursors.Hand
        btnUpdate.Cursor = Cursors.Hand
        btnExcel.Cursor = Cursors.Hand
        Me.KeyPreview = True
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

    Private Sub Panel3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Panel3.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel3.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.Black, Color.White, Drawing2D.LinearGradientMode.ForwardDiagonal)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = {
                Color.Cyan,
                Color.Yellow,
                Color.Lime,
                Color.Cyan,
                Color.Azure,
                Color.Lime,
                Color.Cyan,
                Color.Cyan
            }
            blend.Positions = {0.0F, 0.12F, 0.25F, 0.4F, 0.55F, 0.7F, 0.85F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)

        End Using
    End Sub
    Private Sub FrmPembayaran_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            Dim frm As New FrmDT
            frm.ShowDialog()
        End If
    End Sub


    Private Sub btnTutup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTutup.Click
        Me.Close()
    End Sub

    Private Sub TampilData()
        Try
            Call Koneksi()
            Dim sql As String = "SELECT * FROM pembayaran ORDER BY id_detail ASC"

            Da = New MySqlDataAdapter(sql, Conn)
            Ds = New DataSet
            Da.Fill(Ds, "pembayaran")
            DataGridView1.DataSource = Ds.Tables("pembayaran")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            With DataGridView1
                .MultiSelect = True
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns("id_bayar").HeaderText = "ID Bayar"
                .Columns("id_detail").HeaderText = "ID Detail"
                .Columns("id_karyawan").HeaderText = "ID Karyawan"
                .Columns("total").HeaderText = "Total Seluruh"
                .Columns("metode_pembayaran").HeaderText = "Metode Pembayaran"
                Dim headerStyle As New DataGridViewCellStyle()
                headerStyle.Font = New Font("Palatino Linotype", 8, FontStyle.Bold)  ' ukuran diperkecil
                headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                DataGridView1.Columns("metode_pembayaran").HeaderCell.Style = headerStyle
                .Columns("status").HeaderText = "Status Pembayaran"
                .Columns("tgl_bayar").HeaderText = "Tanggal Bayar"

                .AlternatingRowsDefaultCellStyle.BackColor = Color.Honeydew
                .DefaultCellStyle.ForeColor = Color.Black
                .DefaultCellStyle.SelectionBackColor = Color.Black
                .DefaultCellStyle.SelectionForeColor = Color.Cyan
                .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.ForeColor = Color.Cyan
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

    Private Sub CariData()
        Try
            Call Koneksi()

            Dim keyword As String = txtCari.Text.Trim()

            Dim sql As String = "SELECT * FROM pembayaran WHERE id_bayar LIKE @key OR id_detail LIKE @key OR id_karyawan LIKE @key OR metode_pembayaran LIKE @key OR status LIKE @key OR tgl_bayar LIKE @key ORDER BY id_detail ASC"

        Cmd = New MySqlCommand(sql, Conn)
        Cmd.Parameters.AddWithValue("@key", "%" & keyword & "%")

        Da = New MySqlDataAdapter(Cmd)
        Ds = New DataSet
        Da.Fill(Ds, "pembayaran")
        DataGridView1.DataSource = Ds.Tables("pembayaran")

    Catch ex As Exception
        MsgBox("Error pencarian: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub txtCari_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCari.TextChanged
        If txtCari.Text = "" Then
            TampilData()
        Else
            CariData()
        End If
    End Sub

    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        If DataGridView1.Columns(e.ColumnIndex).Name = "status" Then
            If e.Value IsNot Nothing Then
                Dim status As String = e.Value.ToString().ToLower()

                Select Case status
                    Case "lunas"
                        e.CellStyle.BackColor = Color.Lime
                        e.CellStyle.ForeColor = Color.Black

                    Case "hutang"
                        e.CellStyle.BackColor = Color.Red
                        e.CellStyle.ForeColor = Color.Black


                End Select
            End If
        End If
    End Sub
    Private Sub TampilKaryawan()
        Call Koneksi()
        Dim sql As String = "SELECT id_karyawan, nama FROM karyawan "
        Da = New MySqlDataAdapter(sql, Conn)
        Ds = New DataSet
        Da.Fill(Ds, "karyawan")

        cmbKaryawan.DataSource = Ds.Tables("karyawan")
        cmbKaryawan.DisplayMember = "nama"
        cmbKaryawan.ValueMember = "id_karyawan"

        cmbKaryawan.SelectedIndex = -1 'biar nggak auto pilih
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Exit Sub
        If e.RowIndex = DataGridView1.NewRowIndex Then
            BersihkanForm()
            Exit Sub
        End If

        Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

        Try
            txtIdBayar.Text = row.Cells("id_bayar").Value.ToString()
            txtTransaksi.Text = row.Cells("id_detail").Value.ToString()
            cmbKaryawan.SelectedValue = row.Cells("id_karyawan").Value.ToString()
            txtTotal.Text = row.Cells("total").Value.ToString()
            cmbMetode.Text = row.Cells("metode_pembayaran").Value.ToString()
            cmbStatus.Text = row.Cells("status").Value.ToString()

            ' untuk dateTimePicker
            If Not IsDBNull(row.Cells("tgl_bayar").Value) Then
                dtpBayar.Value = CDate(row.Cells("tgl_bayar").Value)
            End If

        Catch ex As Exception
            MsgBox("Error CellClick: " & ex.Message)
        End Try
    End Sub

    Private Sub txtTransaksi_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTransaksi.TextChanged
        If txtTransaksi.Text = "" Then
            txtTotal.Text = ""
            Exit Sub
        End If

        Try
            Call Koneksi()
            Dim sql As String = "SELECT subtotal FROM detail_transaksi WHERE id_detail = @id"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id", txtTransaksi.Text)

            Dim result = Cmd.ExecuteScalar()

            If result IsNot Nothing Then
                txtTotal.Text = result.ToString()
            Else
                txtTotal.Text = ""
            End If

        Catch ex As Exception
            MsgBox("Error ambil subtotal: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSimpan.Click
        If txtIdBayar.Text = "" Or txtTransaksi.Text = "" Or cmbKaryawan.Text = "" Or txtTotal.Text = "" Or cmbMetode.Text = "" Or cmbStatus.Text = "" Then
            MsgBox("Data belum lengkap!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            '=============================
            ' CEK APAKAH ID_BAYAR SUDAH ADA
            '=============================
            Dim cekSql As String = "SELECT COUNT(*) FROM pembayaran WHERE id_bayar = @id_bayar"
            Dim cekCmd As New MySqlCommand(cekSql, Conn)
            cekCmd.Parameters.AddWithValue("@id_bayar", txtIdBayar.Text)

            Dim jumlah As Integer = CInt(cekCmd.ExecuteScalar())

            If jumlah > 0 Then
                MsgBox("ID bayar tidak boleh duplikat!", vbCritical)
                Exit Sub
            End If
            '=============================


            Dim sql As String = "INSERT INTO pembayaran(id_bayar, id_detail, id_karyawan, total, metode_pembayaran, status, tgl_bayar) VALUES (@id_bayar, @id_detail, @id_karyawan, @total, @metode, @status, @tgl)"

            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id_bayar", txtIdBayar.Text)
            Cmd.Parameters.AddWithValue("@id_detail", txtTransaksi.Text)
            Cmd.Parameters.AddWithValue("@id_karyawan", cmbKaryawan.SelectedValue)
            Cmd.Parameters.AddWithValue("@total", txtTotal.Text)
            Cmd.Parameters.AddWithValue("@metode", cmbMetode.Text)
            Cmd.Parameters.AddWithValue("@status", cmbStatus.Text)
            Cmd.Parameters.AddWithValue("@tgl", dtpBayar.Value.ToString("yyyy-MM-dd HH:mm:ss"))

            Cmd.ExecuteNonQuery()

            MsgBox("Data pembayaran berhasil disimpan!", vbInformation)

            Call TampilData()
            Call BersihkanForm()

        Catch ex As Exception
            MsgBox("Error Simpan: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Sub BersihkanForm()
        txtIdBayar.Clear()
        txtTransaksi.Clear()
        cmbKaryawan.SelectedIndex = -1
        txtTotal.Clear()
        cmbMetode.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
        dtpBayar.Value = Date.Now
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        If txtIdBayar.Text = "" Or txtTransaksi.Text = "" Or cmbKaryawan.Text = "" Or txtTotal.Text = "" Or cmbMetode.Text = "" Or cmbStatus.Text = "" Then
            MsgBox("Data belum lengkap!", vbExclamation)
            Exit Sub
        End If

        Try
            Call Koneksi()

            '=============================
            ' CEK APAKAH ID_BAYAR ADA
            '=============================
            Dim cekSql As String = "SELECT COUNT(*) FROM pembayaran WHERE id_bayar = @id_bayar"
            Dim cekCmd As New MySqlCommand(cekSql, Conn)
            cekCmd.Parameters.AddWithValue("@id_bayar", txtIdBayar.Text)

            Dim jumlah As Integer = CInt(cekCmd.ExecuteScalar())

            If jumlah = 0 Then
                MsgBox("ID bayar tidak ditemukan, update dibatalkan!", vbCritical)
                Exit Sub
            End If
            '=============================


            Dim sql As String = "UPDATE pembayaran SET id_detail = @id_detail, id_karyawan = @id_karyawan, total = @total, metode_pembayaran = @metode, `status` = @status, tgl_bayar = @tgl WHERE id_bayar = @id_bayar"


            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id_bayar", txtIdBayar.Text)
            Cmd.Parameters.AddWithValue("@id_detail", txtTransaksi.Text)
            Cmd.Parameters.AddWithValue("@id_karyawan", cmbKaryawan.SelectedValue)
            Cmd.Parameters.AddWithValue("@total", txtTotal.Text)
            Cmd.Parameters.AddWithValue("@metode", cmbMetode.Text)
            Cmd.Parameters.AddWithValue("@status", cmbStatus.Text)
            Cmd.Parameters.AddWithValue("@tgl", dtpBayar.Value.ToString("yyyy-MM-dd HH:mm:ss"))

            Cmd.ExecuteNonQuery()

            MsgBox("Data pembayaran berhasil diperbarui!", vbInformation)

            Call TampilData()
            Call BersihkanForm()

        Catch ex As Exception
            MsgBox("Error Update: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub btnBatal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        BersihkanForm()
    End Sub

    Private Sub cmbMetode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMetode.SelectedIndexChanged
        If cmbMetode.Text = "Transfer" Then

            PictureBox1.Visible = True
            PictureBox2.Visible = True
            PictureBox3.Visible = True
            PictureBox4.Visible = True
            PictureBox5.Visible = True
            PictureBox6.Visible = True
            PictureBox7.Visible = True
            PictureBox8.Visible = True
            PictureBox9.Visible = True
            PictureBox10.Visible = True
            PictureBox11.Visible = False
            Label8.Visible = True
            Label9.Visible = True
            Label10.Visible = True
            Label12.Visible = True
            Label13.Visible = True
            Label14.Visible = False
            Label15.Visible = False

        Else
            ' Jika metode Cash → sembunyikan semua
            PictureBox1.Visible = False
            PictureBox2.Visible = False
            PictureBox3.Visible = False
            PictureBox4.Visible = False
            PictureBox5.Visible = False
            PictureBox6.Visible = False
            PictureBox7.Visible = False
            PictureBox8.Visible = False
            PictureBox9.Visible = False
            PictureBox10.Visible = False
            PictureBox11.Visible = True
            Label8.Visible = False
            Label9.Visible = False
            Label10.Visible = False
            Label12.Visible = False
            Label13.Visible = False
            Label14.Visible = True
            Label15.Visible = True
        End If
    End Sub

    Private Sub DrawCustomBorder(ByVal pb As PictureBox, ByVal e As PaintEventArgs, ByVal warna As Color, ByVal tebal As Integer)
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = New Rectangle(0, 0, pb.Width - 1, pb.Height - 1)
        Using p As New Pen(warna, tebal)
            g.DrawRectangle(p, rect)
        End Using
    End Sub
    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox1.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox2_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox2.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox3_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox3.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox4_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox4.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox5_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox5.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox6_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox6.Paint
        DrawCustomBorder(PictureBox1, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox7_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox7.Paint
        DrawCustomBorder(PictureBox7, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox8_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox8.Paint
        DrawCustomBorder(PictureBox8, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox9_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox9.Paint
        DrawCustomBorder(PictureBox9, e, Color.Black, 3)
    End Sub
    Private Sub PictureBox10_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox10.Paint
        DrawCustomBorder(PictureBox10, e, Color.Black, 3)
    End Sub

    Private Sub btnHapus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHapus.Click
        ' Cek dulu apakah id bayar kosong
        If txtIdBayar.Text = "" Then
            MsgBox("Pilih data yang ingin dihapus!", vbExclamation)
            Exit Sub
        End If

        ' Konfirmasi hapus
        Dim tanya As DialogResult = MsgBox("Yakin ingin menghapus data ini?", vbQuestion + vbYesNo, "Konfirmasi Hapus")

        If tanya = vbNo Then Exit Sub

        Try
            Call Koneksi()

            Dim sql As String = "DELETE FROM pembayaran WHERE id_bayar = @id_bayar"
            Cmd = New MySqlCommand(sql, Conn)
            Cmd.Parameters.AddWithValue("@id_bayar", txtIdBayar.Text)

            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil dihapus!", vbInformation)

            ' Refresh tabel
            TampilData()

            ' Bersihkan form
            BersihkanForm()

        Catch ex As Exception
            MsgBox("Error Hapus: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub Button_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
 Handles btnSimpan.MouseEnter, btnHapus.MouseEnter, btnBatal.MouseEnter, btnTutup.MouseEnter, btnUpdate.MouseEnter, btnExcel.MouseEnter
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Cyan
        btn.ForeColor = Color.Black
    End Sub

    Private Sub Button_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnSimpan.MouseLeave, btnHapus.MouseLeave, btnBatal.MouseLeave, btnTutup.MouseLeave, btnUpdate.MouseLeave, btnExcel.MouseLeave
        Dim btn As Button = CType(sender, Button)
        btn.BackColor = Color.Black
        btn.ForeColor = Color.White
    End Sub

    Private Sub Panel4_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel4.Paint
        Dim g As Graphics = e.Graphics
        Dim rect As Rectangle = Panel1.ClientRectangle
        Using brush As New Drawing2D.LinearGradientBrush(rect, Color.White, Color.Azure, Drawing2D.LinearGradientMode.Vertical)
            Dim blend As New Drawing2D.ColorBlend()
            blend.Colors = {Color.Cyan, Color.White, Color.Azure, Color.DarkCyan}
            blend.Positions = {0.0F, 0.3F, 0.6F, 1.0F}
            brush.InterpolationColors = blend
            g.FillRectangle(brush, rect)
        End Using
    End Sub


    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcel.Click
        Try
            Dim xlApp As New Excel.Application
            Dim xlWorkBook As Excel.Workbook = xlApp.Workbooks.Add()
            Dim xlWorkSheet As Excel.Worksheet = CType(xlWorkBook.Worksheets(1), Excel.Worksheet)

            ' ========== STYLE DASAR ==========
            xlApp.Visible = True
            xlWorkSheet.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            xlWorkSheet.Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter

            ' ========== JUDUL BESAR ==========
            Dim colCount As Integer = DataGridView1.Columns.Count
            Dim lastColLetter As String = Chr(64 + colCount)

            xlWorkSheet.Range("A1").Value = "Laporan Pembayaran OUR-LAUNDRY"
            xlWorkSheet.Range("A1:" & lastColLetter & "1").Merge()
            With xlWorkSheet.Range("A1")
                .Font.Name = "Segoe UI Semibold"
                .Font.Size = 22
                .Font.Bold = True
                .Font.Color = RGB(255, 255, 0)        ' TULISAN KUNING
                .Interior.Color = RGB(0, 0, 0)        ' BACKGROUND HITAM
                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            End With

            ' ========== FREEZE HEADER ==========
            xlWorkSheet.Application.ActiveWindow.SplitRow = 3
            xlWorkSheet.Application.ActiveWindow.FreezePanes = True

            ' ========== HEADER (Mulai Baris 2) ==========
            For i As Integer = 0 To DataGridView1.Columns.Count - 1
                xlWorkSheet.Cells(3, i + 1) = DataGridView1.Columns(i).HeaderText
            Next

            ' Styling Header
            Dim headerRange As Excel.Range = xlWorkSheet.Range(xlWorkSheet.Cells(3, 1), xlWorkSheet.Cells(3, colCount))
            headerRange.Interior.Color = RGB(0, 0, 0)
            headerRange.Font.Color = RGB(255, 255, 0)
            headerRange.Font.Bold = True
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

            ' ========== ISI DATA (Mulai Baris 3) ==========
            For row As Integer = 0 To DataGridView1.Rows.Count - 1
                For col As Integer = 0 To DataGridView1.Columns.Count - 1
                    xlWorkSheet.Cells(row + 4, col + 1) = DataGridView1.Rows(row).Cells(col).Value
                Next
            Next

            ' Hitung batas data
            Dim lastRow As Integer = DataGridView1.Rows.Count + 3

            ' ========== ZEBRA ROW ==========
            For r As Integer = 4 To lastRow
                If r Mod 2 = 0 Then
                    xlWorkSheet.Range("A" & r & ":" & lastColLetter & r).Interior.Color = RGB(242, 242, 242)
                End If
            Next

            ' ========== BORDER TIPIS DALAM ==========
            Dim fullRange As Excel.Range = xlWorkSheet.Range("A3", xlWorkSheet.Cells(lastRow, colCount))
            fullRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            fullRange.Borders.Weight = Excel.XlBorderWeight.xlThin

            ' ========== BORDER TEBAL LUAR ==========
            fullRange.Borders(Excel.XlBordersIndex.xlEdgeLeft).Weight = Excel.XlBorderWeight.xlMedium
            fullRange.Borders(Excel.XlBordersIndex.xlEdgeTop).Weight = Excel.XlBorderWeight.xlMedium
            fullRange.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlMedium
            fullRange.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlMedium

            ' Autofit kolom
            fullRange.Columns.AutoFit()

            Dim totalRow As Integer = lastRow + 1

            ' Label TOTAL
            xlWorkSheet.Range("A" & totalRow).Value = "Total Laba (BRUTO)"
            xlWorkSheet.Range("A" & totalRow & ":C" & totalRow).Merge()
            xlWorkSheet.Range("A" & totalRow).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight
            xlWorkSheet.Range("A" & totalRow).Font.Bold = True

            ' Formula SUM untuk kolom D
            xlWorkSheet.Range("D" & totalRow).Formula = "=SUM(D4:D" & lastRow & ")"
            xlWorkSheet.Range("D" & totalRow).Font.Bold = True

            ' Tema hitam-kuning biar selaras header
            xlWorkSheet.Range("A" & totalRow & ":D" & totalRow).Interior.Color = RGB(0, 0, 0)        ' Hitam
            xlWorkSheet.Range("A" & totalRow & ":D" & totalRow).Font.Color = RGB(0, 255, 0)         ' Hijau uang

            xlWorkSheet.Range("A" & totalRow & ":D" & totalRow).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter


            MessageBox.Show("Export Excel Berhasil & Styling Lengkap!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Gagal Export: " & ex.Message)
        End Try

    End Sub

End Class