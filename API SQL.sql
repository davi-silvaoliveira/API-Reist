create database if not exists reist_2021;
-- default character set latin1
-- collate latin1_general_cs;
use reist_2021;


-- TABELAS --


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

create table funcionario(
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
    salario float(6,2) not null,
    cargo varchar(100) not null, 
    constraint fk_endereco_funcionario foreign key (endereco) references endereco(cep)
);

create table local(
	id_local int primary key auto_increment,
    sigla varchar(3) not null,	
    nome_local varchar(60) not null,
    tipo_local varchar(20) not null,
    numero_endereco int not null, -- 1 -> aeroporto | 2-> terminal
    endereco int(8) zerofill,
    constraint fk_endereco_local foreign key (endereco) references endereco(cep)
);

create table hotel(
	cnpj_hotel bigint(14) zerofill primary key,
    nome_hotel varchar(60) not null,
    descricao varchar(600) not null,
    endereco int(8) zerofill,
    constraint fk_endereco_hotel foreign key (endereco) references endereco(cep)
);

create table quarto(
	id int primary key auto_increment,
    nome varchar(128) not null,
    quantidade int not null,
    capacidade int not null,
    descricao varchar(200) not null,
    precoDiaria float(6,2) not null,
    cnpj_hotel bigint(14) zerofill,
    constraint fk_hotelquarto foreign key (cnpj_hotel) references hotel (cnpj_hotel) 
);

create table empresa(
	-- id_empresa int primary key auto_increment,
    cnpj_empresa bigint(14) zerofill primary key,
    nome_empresa varchar(50) not null,
    transporte int not null -- 1 -> aviao | 2-> onibus
);

create table passagem(
	id_passagem int primary key auto_increment,
    saida datetime not null,
    origem int not null,
    chegada datetime not null,
    destino int not null,
    assentos int not null,
    classe int not null, -- 1 -> economia | 2-> executiva
    preco float(6,2) not null,
    empresa bigint(14) zerofill not null,
    constraint fk_empresa_passagem foreign key (empresa) references empresa(cnpj_empresa),
    constraint fk_origem_passagem foreign key (origem) references local(id_local),
    constraint fk_destino_passagem foreign key (destino) references local(id_local)
);

create table viagem(
	id_viagem int primary key auto_increment,
    checkin datetime,
    checkout datetime,
    ida int,
    volta int,
    quantidade_quartos int,
    quantidade_hospedes int,
    quarto int,
    constraint fk_id_passagemIda foreign key (ida) references passagem(id_passagem),
    constraint fk_id_passagemVolta foreign key (volta) references passagem(id_passagem),
    constraint fk_id_quarto foreign key (quarto) references quarto(id)
);

create table cartao(
	id_cartao int primary key auto_increment,
    numero int not null,
    validade date not null,
    codSeguranca int not null,
    tipo char not null,
    bandeira varchar(70) not null
);

create table pacote(
	id_pacote int primary key auto_increment,
    descricao varchar(500) not null,
    desconto float,
    id_viagem int,
    constraint fk_viagem_pacote foreign key (id_viagem) references viagem(id_viagem)
);


create table pagamento_compra(
	-- id_pagamento int primary key auto_increment,
    id_compra int primary key auto_increment,
    cpf_cliente bigint(14) not null,
    id_pacote int,
    id_viagem int,
    cartao int not null,
    parcelas int not null,
    data_pagamento date not null,
    constraint fk_cliente_pagamento foreign key (cpf_cliente) references cliente(cpf),
    constraint fk_cartao_pagamento foreign key (cartao) references cartao(id_cartao),
    constraint fk_pacote_compra foreign key (id_pacote) references pacote(id_pacote),
    constraint fk_viagem_compra foreign key (id_viagem) references pacote(id_pacote)
);

alter table local modify numero_endereco int null;
alter table hotel add numero_endereco int after endereco;
alter table cartao modify numero bigint;
alter table viagem change quantidade_hospedes quantidade_pessoas int;

-- PROCEDURES --


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
create procedure verificar_endereco(in thisCep int(11), in thisLogradouro varchar(100), in thisBairro varchar(100), in thisCidade varchar(100),
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


/*-- tipo 0 -> viagem | tipo 1 -> pacote
delimiter $$
create procedure comprar_pacote(in thisCpf int(8), in tipo bool, in idPacoteViagem int, in thisCartao int, in thisParcelas int,
in thisData date)
begin

declare passageiros int;
declare ThisIdPassagem int;
declare ThisIdViagem int;

if (tipo = 1) then insert into pagamento_compra values(default, thisCpf, idPacoteViagem, null, thisCartao, thisParcelas, thisData);

set ThisIdPassagem = (select ida from viagem where id_viagem = (select id_viagem from pacote where id_pacote = idPacoteViagem));
set passageiros = (select assentos from passagem where id_passagem = (select ida from viagem where id_viagem = @@identity));
update passagem set assentos = assentos - passageiros where id_assentos = ThisIdPassagem;  

else
	insert into pagamento_compra values(default, thisCpf, null, idPacoteViagem, thisCartao, thisParcelas, thisData);

end if;

end $$
delimiter ;*/


-- VIEWS --


create view vw_listar_enderecos as
select en.cep, en.logradouro, b.nome_bairro as bairro, c.nome_cidade as cidade, e.UF_estado as estado from endereco as en 
inner join bairro as b on en.bairro = b.id_bairro inner join cidade as c on b.cidade = c.id_cidade inner join estado as e on c.UF = e.id_estado;

create view vw_buscar_passagem_ida as
select p.id_passagem as id_passagem, p.saida as saida, p.chegada as chegada, p.assentos, p.preco, p.classe, o.sigla as origem,
d.sigla as destino, end_origem.estado as ori_uf, end_origem.cidade as ori_city, end_destino.estado as des_uf, end_destino.cidade as des_city from passagem as p 
inner join local as o inner join 
local as d inner join vw_listar_enderecos as end_origem inner join vw_listar_enderecos as end_destino where o.id_local = p.origem and
d.id_local = p.destino and end_origem.cep = o.endereco and end_destino.cep = d.endereco;

create view vw_buscar_passagem_ida_volta as
select ida.id_passagem as id_ida, volta.id_passagem as id_volta, ida.saida as saida_ida, ida.chegada as chegada_ida, ida.assentos as assentos_ida, ida.preco as preco_ida, ida.classe, o.sigla as origem, 
volta.saida as saida_volta, volta.chegada as chegada_volta, volta.assentos as assentos_volta, volta.preco as preco_volta,
d.sigla as destino, end_origem.cidade as ori_city ,end_origem.estado as ori_uf, end_destino.cidade as des_city, end_destino.estado as des_uf from passagem as ida 
inner join passagem as volta inner join local as o inner join local as d inner join vw_listar_enderecos as end_origem inner join vw_listar_enderecos as end_destino 
where ida.saida < volta.saida and o.id_local = ida.origem and o.id_local = volta.destino and d.id_local = ida.destino and d.id_local = volta.origem and end_origem.cep = o.endereco and end_destino.cep = d.endereco
and ida.classe = volta.classe and ida.empresa = volta.empresa;

create view vw_buscar_hotel as
select cnpj_hotel, nome_hotel, descricao, numero_endereco, vw.cep, vw.logradouro, vw.bairro, vw.cidade, vw.estado
from hotel inner join vw_listar_enderecos as vw where hotel.endereco = vw.cep;

create view vw_listar_clientes as
select cpf, nome, email, senha, celular, sexo, numero_endereco, vw.cep, vw.logradouro, vw.bairro, vw.cidade, vw.estado from cliente 
inner join vw_listar_enderecos as vw where vw.cep = endereco;

-- create view vw_listar_clientes as


-- INSERTS --


insert into estado values(default, "SP"),(default, "BA"), (default, "RJ"), (default, "RS"), (default, "SC"), (default, "MG"), (default, "PA"), (default, "PE"), (default, "PR");
insert into cidade values(default, "São Paulo", 1), (default, "Guarulhos", 1), (default, "Campinas", 1), (default, "Belo Horizonte", 6), (default, "Porto Seguro", 2), 
(default, "Curitiba", 9), (default, "Rio de Janeiro", 3), (default, "Porto Alegre", 4), (default, "São José dos Pinhais", 9);
insert into bairro values(default, "Vila Leopoldina", 1), (default, "Lapa", 1), (default, "Guarulhos", 2), (default, "Cidade Alta", 6), 
(default, "Vila Congonhas", 1), (default, "Águas Belas", 9), (default, "Centro Histórico de São Paulo", 1);
insert into endereco values(05089000, "Guaipá", 1), (83010900, "Rocha Pombo", 6), (07190100, "Hélio Smidt", 3), (45810000, "Estrada do Aeroporto", 4),
(04626911, "Washington Luís", 5), (01219902, "Largo do Arouche", 7);

insert into local values(default, "GRU", "Aeroporto Internacional de Guarulhos", 1, null, 07190100),
(default, "CGH", "Aeroporto de São Paulo", 1, null, 04626911), (default, "BPS", "Aeroporto Internacional de Porto Seguro", 1, null, 45810000),
(default, "CWB", "Aeroporto Internacional Afonso Pena", 1, null, 83010900);

insert into empresa values(09296295000129, "Azul Linhas Aéreas", 1);
insert into empresa values(06164253000187, "Gol Linhas Aéreas", 1);

insert into passagem values(default, '2022-01-01 12:20:00', 1, '2022-01-01 13:00:00', 3, 50, 1, 100.00, 09296295000129);
insert into passagem values(default, '2022-01-01 12:20:00', 1, '2022-01-01 13:00:00', 3, 50, 2, 300.00, 09296295000129);

insert into passagem values(default, '2022-01-15 20:00:00', 3,'2022-01-15 23:00:00', 1, 50, 1, 80.00, 09296295000129);
insert into passagem values(default, '2022-01-15 20:00:00', 3,'2022-01-15 23:00:00', 1, 50, 2, 250.00, 09296295000129);

insert into passagem values(default, '2022-02-10 15:00:00', 4,'2022-02-10 16:00:00', 2, 50, 1, 120.00, 06164253000187);
insert into passagem values(default, '2022-02-10 15:00:00', 4,'2022-02-10 16:00:00', 2, 50, 2, 400.00, 06164253000187);

insert into passagem values(default, '2022-02-20 18:00:00', 2,'2022-02-20 19:00:00', 4, 50, 1, 120.00, 06164253000187);
insert into passagem values(default, '2022-02-20 18:00:00', 2,'2022-02-20 19:00:00', 4, 50, 2, 400.00, 06164253000187);
insert into passagem values(default, '2022-02-28 18:00:00', 2,'2022-02-28 19:00:00', 4, 20, 1, 400.00, 06164253000187);


insert into hotel values(56449952000303, "San Michel Hotel", "Este hotel econômico despretensioso com vista para o Largo do Arouche fica a 6 minutos a pé de uma estação de metrô, a 13 minutos do teatro municipal de São Paulo e a 15 minutos das
exposições linguísticas no Museu da Língua Portuguesa. As suítes e os quartos simples contam com Wi-Fi gratuito, TV com tela plana e frigobar.
As acomodações de categoria mais alta incluem banheiras de hidromassagem e/ou sacadas com vista para a praça. A estadia de crianças de até cinco anos (uma por quarto) acompanhadas de um adulto é gratuita.
Buffet de café da manhã incluso. O hotel também oferece também um bar", 01219902, 200);
insert into quarto values(default, "Standart", 20, 2, "Quarto confortável, com TV!", 90.00, 56449952000303), 
(default, "Standart Triplo", 10, 3, "Quarto confortável, com TV e dois banheiros!", 100.00, 56449952000303);

insert into viagem values(default, '2022-01-11 16:00:00', '2022-01-15 18:00:00', 1, 3, 2, 4, 1);

insert into pacote values(default, "Pacote de São Paulo para Paraná, com 2 quartos pra 4 pessoas", 20, 1);

-- insert into cartao values(default, 1234123412341234, '2023-12-01', 123, "C", "Visa");

-- insert into compra values();

call cadastrar_cliente(12345665400, "Davi Silva Oliveira", "davi", "123",
"11951982626", "M", str_to_date('4/7/2003', "%d/%m/%Y"),
02984010, "Charles Mion", "Jardim Pirituba", "São Paulo", "SP", "425");


-- SELECTIONS --


select * from bairro;
select * from cidade;
select * from estado;
select * from endereco;
select * from cliente;
select * from cliente where email = 'davi';
select * from hotel; -- 12345678998765
select * from passagem;
select * from local;
select * from quarto;
select * from pacote;
select * from viagem;
select * from cartao;
select * from pagamento_compra;
select * from local;
select * from empresa;
select date_format(saida, '%d/%m/%Y') as saida from passagem;

select * from vw_listar_enderecos;
select * from vw_listar_clientes;
select * from vw_buscar_hotel;
select * from vw_buscar_passagem_ida where id_passagem = 1;
select * from vw_buscar_passagem_ida_volta;
select * from vw_buscar_hotel where estado = "SC" and cidade = "São José dos Pinhais";
select *, date_format(saida, '%d/%m/%Y') as saida from vw_buscar_passagem_ida where date(saida) = date('2021-12-25') and ori_city = "Guarulhos" and des_city = "São José dos Pinhais";
select * from vw_buscar_passagem_ida where date(saida) = date('2022-01-15') and des_city = "Guarulhos" and ori_city = "São José dos Pinhais" and classe = 1;
select * from vw_buscar_passagem_ida_volta where date(saida_ida) = date('2022-01-01') and date(saida_volta) = date('2022-01-15') and ori_city = 'Guarulhos' and des_city = 'Curitiba';
select id_ida, id_volta from vw_buscar_passagem_ida_volta where date(saida_ida) = date('2022-01-01') and date(saida_volta) = date('2022-01-15') and ori_city = 'Guarulhos' and des_city = 'Curitiba';
select * from pacote as p inner join viagem as v inner join vw_buscar_passagem_ida_volta as pass /*inner join hotel as h*/ where p.id_viagem = v.id_viagem and
pass.id_ida = v.ida and pass.id_volta = v.volta;

select count(*) from estado where uf_estado = "kk";
select count(*) from endereco where cep = 02525111;
select count(*) from cidade where nome_cidade = "dd" and "kk";
select count(*) from endereco where cep = 02525188 and bairro = 12;


-- drop table quarto;
-- truncate table local;
-- drop procedure cadastrar_cliente;3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2
-- drop view vw_buscar_passagem_ida;
-- truncate table cliente;
-- drop database reist_2021;