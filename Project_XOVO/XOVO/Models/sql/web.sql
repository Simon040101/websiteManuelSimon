use xovo;
select * from users;

drop table users;
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
insert into users values (2, "Manuel", "Reismann", "2000-01-31" , 0, "Manuel", "Manuel@swp.at", sha2("123456789", 256), 1, "red", "/Content/img/background_login_registration.jpg");
select * from users;

create table Feed(
feed_id int not null auto_increment,
user_id int not null,
creationDateTime datetime,
imagePath varchar(300),
content text,

constraint feed_id_PK primary key(feed_id),
constraint user_id_FK foreign key(user_id) references users(id)
)engine = InnoDB;

insert into feed values(null, 1, "2019-03-12 09:11:00", "/Content/img/background_login_registration.jpg", "erster Feed");
insert into feed values(NULL, 1, "2019-03-13 17:01:00", "/Content/img/background_login_registration.jpg", "zweiter Feed");
insert into feed values(null, 1, "2019-03-14 12:38:00", "/Content/img/background_login_registration.jpg", "dritter Feed");