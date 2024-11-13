create extension "uuid-ossp";
create schema if not exists users;
create schema if not exists game;
create schema if not exists "admin";

create table if not exists users."user" (
      id              uuid            not null default uuid_generate_v4()
    , "name"          varchar(50)     not null
    , "login"         varchar(50)     not null
    , "password"      varchar(1000)   not null
    , constraint 
        pk_user_id    primary key(id)
);

create table if not exists users.user_info_type(
      id                      uuid            not null default uuid_generate_v4()
    , "name"                  varchar(100)    not null
    , code                    varchar(100)    not null
    , constraint 
        pk_user_info_type_id  primary key(id)
);

create table if not exists users.user_info (
      id                 uuid            not null default uuid_generate_v4()
    , user_id            uuid            not null 
    , user_info_type_id  uuid            not null
    , "value"            varchar         null
    , constraint 
        pk_user_info_id  primary key(id)
);

create table if not exists game.person (
      id              uuid            not null default uuid_generate_v4()
    , "name"          varchar(50)     not null
    , user_id         uuid            not null    
    , constraint 
        pk_person_id  primary key(id)
);

create table if not exists game.person_info_type(
      id                        uuid            not null default uuid_generate_v4()
    , "name"                    varchar(100)    not null
    , code                      varchar(100)    not null
    , constraint 
        pk_person_info_type_id  primary key(id)
);

create table if not exists game.person_info (
      id                   uuid            not null default uuid_generate_v4()
    , person_id            uuid            not null 
    , person_info_type_id  uuid            not null
    , "value"              varchar         null
    , constraint 
        pk_user_info_id    primary key(id)
);

create table if not exists game."server" (
      id              uuid            not null default uuid_generate_v4()
    , "name"          varchar(50)     not null
    , "ip"            varchar(50)     not null 
    , machine_name    varchar(100)    not null
    , constraint 
        pk_server_id  primary key(id)
);

create table if not exists game.ship (
      id            uuid            not null default uuid_generate_v4()
    , "name"        varchar(50)     not null    
    , code          varchar(100)    not null
    , server_id     uuid            not null
    , constraint 
        pk_ship_id  primary key(id)
);

create table if not exists game.ship_level (
      id                  uuid            not null default uuid_generate_v4()
    , ship_id             uuid            not null
    , number              int             not null    
    , code                varchar(100)    not null
    , constraint 
        pk_ship_level_id  primary key(id)
);

create table if not exists game.ship_level_map (
      id                      uuid            not null default uuid_generate_v4()
    , x                       int             not null 
    , y                       int             not null
    , ship_level_id           uuid            not null
    , terrain_id              uuid            not null
    , constraint 
        pk_ship_level_map_id  primary key(id)
);

create table if not exists game.terrain_type(
      id                    uuid            not null default uuid_generate_v4()
    , "name"                varchar(100)    not null
    , code                  varchar(100)    not null
    , default_image         varchar(255)    null
    , constraint 
        pk_terrain_type_id  primary key(id)
);

create table if not exists game.terrain(
      id                    uuid            not null default uuid_generate_v4()    
    , terrain_type_id       uuid            not null
    , "image"               varchar(255)    null
    , constraint 
        pk_terrain_id       primary key(id)
);









