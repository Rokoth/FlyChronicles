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

create table if not exists users.h_user_info_type(
      h_id            serial,
      id              uuid            not null
    , "name"          varchar(100)    not null
    , code            varchar(100)    not null
    , is_deleted      bool            not null
    , userid          uuid            not null
    , change_date     timestamp       not null default now()
    , constraint 
        pk_h_user_info_type_id  primary key(h_id)
);

create table if not exists users.user_info (
      id                 uuid            not null default uuid_generate_v4()
    , user_id            uuid            not null 
    , user_info_type_id  uuid            not null
    , "value"            varchar         null
    , constraint 
        pk_user_info_id  primary key(id)
);

------------------- person

create table if not exists game.person (
      id              uuid            not null default uuid_generate_v4()
    , "name"          varchar(50)     not null
    , user_id         uuid            not null    
    , constraint 
        pk_person_id  primary key(id)
);

create table if not exists game.h_person (
      h_id            serial,
      id              uuid            not null 
    , "name"          varchar(50)     not null
    , user_id         uuid            not null    
    , is_deleted      bool            not null
    , userid          uuid            not null
    , change_date     timestamp       not null default now()
    , constraint 
        pk_hperson_id  primary key(h_id)
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

--------------------------- inventory
create table if not exists game.inventory (
      id                    uuid            not null default uuid_generate_v4()
    , "name"                varchar(50)     not null
    , inventory_type_id     uuid            not null    
    , constraint 
        pk_inventory_id     primary key(id)
);

create table if not exists game.h_inventory (
      h_id                  serial
    , id                    uuid            not null default uuid_generate_v4()
    , "name"                varchar(50)     not null
    , inventory_type_id     uuid            not null
    , is_deleted            bool            not null
    , userid                uuid            not null
    , change_date           timestamp       not null default now()
    , constraint 
        pk_inventory_id     primary key(id)
);

-- todo game.inventory_info_type, inventory_info, inventory_type

--------------------------- characteristic


create table if not exists game.h_characteristic (
      h_id                      serial
    , id                        uuid            not null default uuid_generate_v4()
    , "name"                    varchar(50)     not null
    , characteristic_type_id    uuid            not null
    , min_value                 decimal         null
    , max_value                 decimal         null
    , enable_values             text            null
    , is_deleted                bool            not null
    , userid                    uuid            not null
    , change_date               timestamp       not null default now()
    , constraint 
        pk_characteristic_id    primary key(id)
);


-------------------------- server

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









