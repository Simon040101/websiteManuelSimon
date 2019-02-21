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
    passwrd varchar(100) not null,
    isAdmin tinyint not null default 0,
    
    constraint id_PK primary key(id)



)engine = InnoDB;


insert into users values (2, "Christoph", "Zallinger", "2019-02-02" , 0, "Christoph", "Chrissi@swp.at", sha1("123456789"), 3);

select * from users;

delete from users where id="4";

alter table users auto_increment = 1;

drop table users;