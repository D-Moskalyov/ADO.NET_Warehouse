CREATE DATABASE Warehouse
--DROP DATABASE Warehouse
USE Warehouse
--USE northwind

CREATE TABLE Clients
(
	ClientID int IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(255),
	Lastname varchar(255),
	email varchar(255),	
)

ALTER TABLE Clients ADD CONSTRAINT CK_Clients_email
    CHECK (email like '%_@__%.__%');

CREATE TABLE Sections
(
	SectionID int IDENTITY(1,1) PRIMARY KEY,
	Topography decimal
)


CREATE TABLE Goods
(
	GoodID int IDENTITY(1,1) PRIMARY KEY,
	ClientID int,
	SectionID int,
	DateStart date,
	DateFinish date,
	Cost int,
	Fine int,
	Stat bit,
	Location decimal,
	Title varchar(255),
	Height int,
	Width int
)

ALTER TABLE Goods ADD 
CONSTRAINT FK_Goods_Clients FOREIGN KEY(ClientID)
	REFERENCES Clients(ClientID);

ALTER TABLE Goods ADD 
CONSTRAINT FK_Goods_Sections FOREIGN KEY(SectionID)
	REFERENCES Sections(SectionID);

insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);
insert into Sections (Topography) values (0);

insert into Clients(FirstName, LastName, email) values ('Дмитрий', 'Москалёв', 'moscalevd@gmail.com');

insert into Goods(ClientID, SectionID, DateStart, DateFinish, Cost, Fine, Stat, Location, Title, Height, Width)
values (1, 2, '2014-08-12', '2014-09-10', 150, 0, 1, 2, 'MyProd', 25, 25);

--delete from Goods where GoodID = 3071 or GoodID = 3072
--delete from Clients where ClientID = 1012
--update Sections set Topography = 117469191 where SectionID = 5 or SectionID = 15
--update Goods set Fine = 0

select * from Clients
select * from Goods 
select * from Sections