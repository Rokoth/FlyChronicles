ALTER TABLE users.user_info 
ADD CONSTRAINT fk_user_info_user_id 
FOREIGN KEY (user_id) 
REFERENCES users.user (id);

ALTER TABLE users.user_info 
ADD CONSTRAINT fk_user_info_user_info_type_id 
FOREIGN KEY (user_info_type_id) 
REFERENCES users.user_info_type (id);

ALTER TABLE game.person 
ADD CONSTRAINT fk_person_user_id 
FOREIGN KEY (user_id) 
REFERENCES users.user (id);

ALTER TABLE game.person_info 
ADD CONSTRAINT fk_person_info_person_id 
FOREIGN KEY (person_id) 
REFERENCES game.person (id);

ALTER TABLE game.person_info 
ADD CONSTRAINT fk_person_info_person_info_type_id 
FOREIGN KEY (person_info_type_id) 
REFERENCES game.person_info_type (id);

ALTER TABLE game.ship 
ADD CONSTRAINT fk_ship_server_id 
FOREIGN KEY (server_id) 
REFERENCES game."server" (id);

ALTER TABLE game.ship_level 
ADD CONSTRAINT fk_ship_level_ship_id 
FOREIGN KEY (ship_id) 
REFERENCES game.ship (id);

/*

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
); */









