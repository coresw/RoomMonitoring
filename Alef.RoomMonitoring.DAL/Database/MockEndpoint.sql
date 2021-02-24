use RoomMonitoring;

drop table if exists MockEndpoint;
create table MockEndpoint(
Id int primary key identity(1, 1),
EndpointIp varchar(40),
PeopleCount int
);
