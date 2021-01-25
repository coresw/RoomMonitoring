use master;
drop database if exists RoomMonitoring;
create database RoomMonitoring;
use RoomMonitoring;

drop table if exists Attendee;
drop table if exists AttendeeType;
drop table if exists Person;
drop table if exists Reservation;
drop table if exists ReservationStatus;
drop table if exists Room;

create table Room(
Id int primary key identity(1,1),
Name varchar(25),
EMail varchar(60) unique,
EndpointIP varchar(40),
);

create table ReservationStatus(
Id int primary key,
Name varchar(20) unique
);
insert into ReservationStatus(Id, Name) values (1, 'Unchecked'), (2, 'OK'), (3, 'Notified');

create table Reservation(
Id int primary key identity(1,1),
Token varchar(255) unique,
Created datetime,
Modified datetime,
TimeFrom datetime,
TimeTo datetime,
Name varchar(50),
Body varchar(256),
ReservationStatusId int foreign key references ReservationStatus(Id),
RoomId int foreign key references Room(Id)
);

create table Person(
Id int primary key identity(1,1),
EMail varchar(50) unique,
Name varchar(30)
);

create table AttendeeType(
Id int primary key,
Name varchar(20) unique
);
insert into AttendeeType(Id, Name) values (1, 'Organizer'), (2, 'Required'), (3, 'Optional');

create table Attendee(
Id int primary key identity(1,1),
PersonId int foreign key references Person(Id),
ReservationId int foreign key references Reservation(Id),
AttendeeTypeId int foreign key references AttendeeType(Id)
);
