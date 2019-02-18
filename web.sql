create database XOVO collate utf8_general_ci;

use XOVO;

create table users(

	id int not null auto_increment,
    firstname varchar(100),
    lastname varchar(100) not null,
    birthdate date null, 
    gender tinyint null,
    username varchar(100) null, 
    email varchar(100) not null unique, 
    passwrd varchar(40) not null,
    
    constraint id_PK primary key(id)



)engine = InnoDB;

insert into users values (1, "Simon", "Raass", null, null, "simon", "simon@gmail.com", "123456");

select * from users;

delete from users;