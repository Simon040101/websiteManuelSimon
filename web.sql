
drop table if exists UserCommentFeed;
drop table if exists UsersLikeFeed;
drop table if exists feed;
drop table if exists users;



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
    profilpic varchar(100),
    
    constraint id_PK primary key(id)
    
)engine = InnoDB;

create table Feed(
	feed_id int not null auto_increment,
    username varchar(100),
	user_id int not null,
	creationDateTime datetime,
	imagePath varchar(300),
	content text,

	constraint feed_id_PK primary key(feed_id),
	constraint user_id_FK foreign key(user_id) references users(id) 
)engine = InnoDB;

create table UsersLikeFeed(
	uid int,
    fid int,
    
    Constraint FK_user_like foreign key (uid) references users(id) on delete cascade on update cascade,
    Constraint FK_feed_like foreign key (fid) references feed(feed_id) on delete cascade on update cascade,
    Primary key (uid, fid)
)engine = InnoDB;

create table UserCommentFeed(
	username varchar(100), 
    fid int,
    content varchar(100),
    comment_id int not null auto_increment,
    
    Constraint FK_feed_comment foreign key (fid) references feed(feed_id) on delete cascade on update cascade,
    primary key (comment_id	)
)engine = InnoDB;

insert into users values (1, "Simon", "Raass", "2001-04-01" , 0, "Simon", "Simon@swp.at", sha2("123456789", 256), 0, "blue", "/Content/img/background_login_registration.jpg", "/Content/img/placeholder.png");