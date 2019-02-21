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

insert into users values (1, "Simon", "Raass", null, null, "simon", "simon@gmail.com", "123456");
insert into users values (2, "Manuel", "Reismann", null, null, "manuel", "manuel@gmx.at", "hallo123", 0);
insert into users values (1, "admin", "admin", "2019-02-02" , 0, "admin", "admin@swp.at", sha1("Admin1!"), 0);

select * from users;

delete from users where id="1";

alter table users auto_increment = 1;

drop table users;