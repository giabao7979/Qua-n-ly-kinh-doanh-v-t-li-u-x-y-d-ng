use master;
--san pham
create table tbl_products(
	MaSP varchar(20) primary key,
	TenSP NVARCHAR(55),
	GiaSP bigint,
	SoLuong int,
	DonVi nvarchar(20),
	NgayNhap Date,
	NhaCC nvarchar(55),
	TrangThai nvarchar(20)
);
drop table tbl_orders;
drop table tbl_hoadon;
--hoa don
create table tbl_hoadon(
	MaHD varchar(20) PRIMARY KEY,
	TenKH nvarchar(55),
	Sdt varchar(20),
	DiaChi nvarchar(100),
	TrangThai varchar(20)
);
--thong ke
create table tbl_orders(
	MaSP varchar(20),
	MaHD varchar(20),
	foreign key (MaSP) references tbl_products(MaSP),
	foreign key (MaHD) references tbl_hoadon(MaHD)

)
