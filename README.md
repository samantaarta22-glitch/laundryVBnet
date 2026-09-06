# 🧺 OurLaundry - Aplikasi Manajemen Laundry

Aplikasi desktop berbasis VB.NET untuk membantu pengelolaan bisnis laundry, mulai dari pencatatan pesanan, perhitungan harga, hingga cetak struk.

## ✨ Fitur Utama
- ✅ Input data pelanggan dan detail pesanan
- ✅ Perhitungan total harga otomatis (berdasarkan berat/jenis layanan)
- ✅ Riwayat transaksi
- ✅ Pembayaran via QR Code

## 🛠️ Tech Stack
- **Bahasa Pemrograman:** Visual Basic .NET (VB.NET)
- **IDE:** Microsoft Visual Studio 2010
- **Database:** (Sebutkan database yang dipakai, misal: MySQL / SQL Server / SQLite)

## 🚀 Cara Menjalankan Project
1. Clone repository ini: `git clone https://github.com/samantaarta22-glitch/laundryVBnet.git`
2. Buka file `.sln` menggunakan Microsoft Visual Studio.
3. Restore NuGet packages (jika ada) dan pastikan koneksi database sudah sesuai.
4. Build dan Run project.

## 📸 Screenshot Aplikasi

Berikut adalah tampilan antarmuka aplikasi OurLaundry:

### Halaman Utama
<img width="1365" height="709" alt="image" src="https://github.com/user-attachments/assets/3e85cc2c-7933-488d-9e7e-6ec63123eb4a" />
Deskripsi: Tampilan utama aplikasi yang berfungsi sebagai navigasi pusat (Main Menu). Dirancang dengan antarmuka yang bersih dan intuitif untuk memudahkan pengguna mengakses berbagai modul seperti data master, transaksi, dan laporan dengan cepat.


### Form Customer
<img width="1366" height="705" alt="image" src="https://github.com/user-attachments/assets/d0b97093-86a1-491b-baa0-231f7f4dcaaf" />
Deskripsi: Form untuk pengelolaan data pelanggan (CRUD - Create, Read, Update, Delete). Fitur ini memungkinkan admin untuk menyimpan riwayat pelanggan secara terdatabase, sehingga memudahkan proses pencarian data dan personalisasi layanan pada transaksi berikutnya.


### Form Pelayanan
<img width="1366" height="703" alt="image" src="https://github.com/user-attachments/assets/6f471ae9-65c7-493b-aa61-657512d1c07a" />
Deskripsi: Modul konfigurasi jenis layanan laundry. Admin dapat mendefinisikan berbagai paket layanan seperti Cuci Basah - Cuci Kering - Cuci Setrika - Express hingga Premium Laundry dengan pengaturan harga yang fleksibel sesuai berat atau satuan.


### Form Transaksi
<img width="1366" height="707" alt="image" src="https://github.com/user-attachments/assets/17c2c23e-8178-4c27-bd48-7e1709e52473" />
Deskripsi: Antarmuka untuk input transaksi baru. Form ini mengintegrasikan data pelanggan dengan jenis layanan yang dipilih, menghitung estimasi berat, dan menentukan tanggal pengambilan. Sistem dirancang untuk meminimalisir kesalahan input data (human error).


### Form Detail Transaksi
<img width="1366" height="705" alt="image" src="https://github.com/user-attachments/assets/cdf75a12-583f-42ec-b4a5-d1ff0db5c13a" />
Deskripsi: Tampilan rincian pesanan yang masuk. Form ini berfungsi untuk memonitor status pengerjaan laundry (apakah sedang dicuci, disetrika, atau siap ambil) serta rekapitulasi total biaya per item secara detail sebelum ke tahap pembayaran.


### Form Pembayaran 
<img width="1366" height="708" alt="image" src="https://github.com/user-attachments/assets/71f1babb-7243-485e-b3e4-44f4f8fb8566" />
Deskripsi: Modul kasir untuk proses checkout. Sistem akan menghitung total harga secara otomatis, menghitung kembalian, dan mencatat status pembayaran (Lunas/Belum Lunas). Mendukung pencatatan pembayaran yang akurat untuk laporan keuangan harian.

