create database if not exists reist_2021;
-- default character set latin1
-- collate latin1_general_cs;
use reist_2021;

create table estado(
	id_estado int primary key auto_increment,
    UF_estado varchar(2) not null unique
);

create table cidade(
	id_cidade int primary key auto_increment,
    nome_cidade varchar(100) not null,
    UF int not null,
    constraint fk_uf_cidade foreign key (UF) references estado(id_estado)
);

create table bairro(
	id_bairro int primary key auto_increment,
    nome_bairro varchar(100) not null,
    cidade int not null,
    constraint fk_cidade_bairro foreign key (cidade) references cidade(id_cidade)
);

create table endereco(
	cep int(8) zerofill primary key,
    logradouro varchar(100) not null,
    bairro int not null,
    constraint fk_bairro_endereco foreign key (bairro) references bairro(id_bairro)
);

create table cliente(
	cpf bigint(11) primary key,
	nome varchar(60) not null,
    email varchar(60) not null unique,
    senha varchar(128) not null,
    celular bigint(11) not null,
    sexo char not null,
    numero_endereco int not null,
    data_nascimento date not null,
    endereco int(8) zerofill,
    constraint fk_endereco_cliente foreign key (endereco) references endereco(cep)
);

/*create table funcionario(
	cpf bigint(11) primary key,
	nome varchar(60) not null,
    email varchar(60) not null unique,
    senha varchar(128) not null,
    celular bigint(11) not null,
    sexo char not null,
    numero_endereco int not null,
    data_nascimento date not null,
    endereco int(8) zerofill,
    acesso int not null,
    constraint fk_endereco_funcionario foreign key (endereco) references endereco(cep)
);*/

create table local(
	id_local int primary key auto_increment,
    nome_local varchar(60) not null,
    tipo_local varchar(20) not null,
    numero_endereco int not null,
    endereco int(8) zerofill,
    constraint fk_endereco_local foreign key (endereco) references endereco(cep)
);

create table hotel(
	cnpj_hotel bigint(14) primary key,
    nome_hotel varchar(60) not null,
    descricao varchar(250) not null,
    endereco int(8) zerofill,
    constraint fk_endereco_hotel foreign key (endereco) references endereco(cep)
);

delimiter $$
create procedure cadastrar_cliente(in thisCpf bigint(11), in thisNome varchar(60), in thisEmail varchar(60), in thisSenha varchar(128),
in thisCelular bigint(11),in thisSexo char, in thisData_nascimento date, in thisCep int(8) zerofill, in thisLogradouro varchar(100),
in thisBairro varchar(100), in thisCidade varchar(100), in thisUf varchar(2), in thisNumero_endereco int)
begin

declare x int;
set x = (select count(*) from endereco where cep = thisCep);

if (x = 0) then call verificar_endereco(thisCep, thisLogradouro, thisBairro, thisCidade, thisUf); end if;

insert into cliente values(thisCpf, thisNome, thisEmail, thisSenha, thisCelular, thisSexo, thisNumero_endereco, thisData_nascimento, thisCep);

end $$
delimiter ;


delimiter $$
create procedure verificar_endereco(in thisCep int(8) zerofill, in thisLogradouro varchar(100), in thisBairro varchar(100), in thisCidade varchar(100),
in thisUf varchar(2))
begin

declare x int;
declare id_state int;
declare id_city int;
declare id_district int;

set x = (select count(*) from estado where UF_estado = thisUf);
if (x = 0) then insert into estado values(default, thisUf); end if;
set id_state = (select id_estado from estado where UF_estado = thisUf);

set x = (select count(*) from cidade where nome_cidade = thisCidade and uf = id_state);
if (x = 0) then insert into cidade values(default, thisCidade, id_state); end if;
set id_city = (select id_cidade from cidade where nome_cidade = thisCidade and UF = id_state);

set x = (select count(*) from bairro where nome_bairro = thisBairro and cidade = id_city);
if (x = 0) then insert into bairro values(default, thisBairro, id_city); end if;
set id_district = (select id_bairro from bairro where nome_bairro = thisBairro and cidade = id_city);

set x = (select count(*) from endereco where cep = thisCep and bairro = id_district);
if (x = 0) then insert into endereco values(thisCep, thisLogradouro, id_district); end if;

end $$
delimiter ;

create view vw_listar_enderecos as
select en.cep, en.logradouro, b.nome_bairro as bairro, c.nome_cidade as cidade, e.UF_estado as estado from endereco as en 
inner join bairro as b on en.bairro = b.id_bairro inner join cidade as c on b.cidade = c.id_cidade inner join estado as e on c.UF = e.id_estado;

select cpf, nome, email, senha, celular, sexo, numero_endereco, vw.cep, vw.logradouro, vw.bairro, vw.cidade, vw.estado from cliente 
inner join vw_listar_enderecos as vw where vw.cep = endereco;


/* --- */


insert into estado values(default, "SP");
insert into cidade values(default, "São Paulo", 1);
insert into bairro values(default, "Vila Leopoldina", 1);
insert into endereco values(05089000, "Guaipá", 1);
insert into endereco values(08950000, "Romano", 1);
insert into cliente values("12312312301", "Davi Silva Oliveira", "davi@gmail.com",
"FA3C1CDEE866E8B57B644E55AA85AD1F001EA14471DA9D41CDD3195E5613F4B8B6FFF905E7F1AFB3954A3E182E92C52497E41DECF5718B51A09BFADF52E77F20",
"11951982768", "M",
"678", str_to_date('4/7/2003', "%d/%m/%Y"), 05089000);
insert into local values(default, "Aero Basilides", "Aeroporto", 0, 05089000);

call cadastrar_cliente(12345665477, "Guilherme", "Guilhermessss.com", "senha123", "11951982626", "M", str_to_date('4/7/2003', "%d/%m/%Y"),
02525198, "Rua de testas", "Bairrinho legal", "OK SP", "OU", "456");
call verificar_endereco(02525170, "Rua de testas", "Bairrinho legal", "OK SP", "OU");

select * from bairro;
select * from cidade;
select * from estado;
select * from endereco;
select * from cliente;
select * from vw_listar_enderecos;
select count(*) from estado where uf_estado = "kk";
select count(*) from endereco where cep = 02525111;
(select count(*) from cidade where nome_cidade = "dd" and "kk");
select count(*) from endereco where cep = 02525188 and bairro = 12;

-- drop table hotel;
-- drop procedure cadastrar_cliente;
-- drop view listar_enderecos;
-- truncate table cliente;
-- drop database reist_2021;