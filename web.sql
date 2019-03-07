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
    layout_color varchar(100),
    background_login varchar(100),
    
    constraint id_PK primary key(id)



)engine = InnoDB;


insert into users values (1, "Simon", "Raass", "2001-04-01" , 0, "Simon", "Simon@swp.at", sha2("123456789", 256), 0, "blue", "/Content/img/background_login_registration.jpg");

select * from users;

delete from users where id="2";

alter table users auto_increment = 1;

drop table users;