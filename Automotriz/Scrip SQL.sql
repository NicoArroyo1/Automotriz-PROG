create database AutomotrizDB_PROG
go
use AutomotrizDB_PROG
go

create table Tipos_empleado(
cod_tipo_empleado int identity(1,1),
descripcion varchar(20),
constraint pk_tipos_empleados primary key (cod_tipo_empleado)
)

create table Empleados(
legajo int identity(1,1),
nombre varchar(50),
cod_tipo_empleado int,
constraint pk_empleados primary key (legajo),
constraint fk_tipos_empleados foreign key (cod_tipo_empleado)
	references Tipos_empleado (cod_tipo_empleado)
)

create table Modelos(
cod_modelo int identity(1,1),
modelo varchar(20),
constraint pk_modelos primary key (cod_modelo)
)

CREATE TABLE Tipos_vehiculo(
cod_tipo_vehiculo int IDENTITY(1,1) NOT NULL,
descripcion varchar(20) NULL,
CONSTRAINT pk_tipos_vehiculos PRIMARY KEY (cod_tipo_vehiculo)
)

create table Automoviles(
patente int identity(1000, 1),
cod_modelo int,
cod_tipo_vehiculo int,
color varchar(15),
precio money,
fec_fabricacion date,
constraint pk_automoviles primary key (patente),
constraint fk_modelos foreign key (cod_modelo)
	references Modelos (cod_modelo),
constraint fk_tipos_vehiculo foreign key (cod_tipo_vehiculo)
	references Tipos_vehiculo (cod_tipo_vehiculo)
)

create table Autopartes(
nro_serie int identity(1,1),
autoparte varchar(50),
precio money,
fec_fabricacion date,
constraint pk_autopartes primary key (nro_serie)
)

create table Autoplanes(
cod_plan int identity(1,1),
nom_plan varchar(30),
cant_coutas int,
tasa_interes int,
constraint pk_planes primary key (cod_plan)
)

create table Tipos_cliente(
cod_tipo_cliente int identity(1,1),
descripcion varchar(25),
constraint pk_tipos_clientes primary key (cod_tipo_cliente)
)

create table Tipos_producto(
cod_tipo_producto int identity(1,1),
descripcion varchar(100),
constraint pk_tipos_productos primary key (cod_tipo_producto)
)

create table Facturas(
cod_factura int identity(1,1),
cod_empleado int,
fecha date,
nom_cliente varchar(50),
cuit bigint,
cod_plan int,
cod_tipo_cliente int,
constraint pk_facturas primary key (cod_factura),
constraint fk_tipos_clientes foreign key (cod_tipo_cliente)
	references Tipos_cliente (cod_tipo_cliente),
constraint fk_planes foreign key (cod_plan)
	references Autoplanes (cod_plan),
constraint fk_empleados_factura foreign key (cod_empleado)
	references Empleados (legajo)
)

create table Detalles_factura(
cod_det_factura int identity(1,1),
cod_factura int,
patente int,
nro_serie int,
cantidad int,
precio money,
constraint pk_detalles_facturas primary key (cod_det_factura),
constraint fk_facturas foreign key (cod_factura)
	references Facturas (cod_factura),
constraint fk_patente foreign key (patente)
	references Automoviles (patente),
constraint fk_nro_serie foreign key (nro_serie)
	references Autopartes (nro_serie)
)


--------------------------- INSERT TIPOS_CLIENTES ---------------------------------------------
insert into Tipos_cliente (descripcion)
				   values ('Consumidor Final'),
			              ('Responsable Inscripto'),
			              ('Monotributista')

-------------------------- INSERT TIPOS_EMPLEADO ----------------------------------------------
insert into Tipos_empleado (descripcion)
                    values ('Vendedor'),
						   ('Técnico')

-------------------------- INSERT EMPLEADOS ---------------------------------------------------
insert into Empleados(nombre            ,cod_tipo_empleado)
               values('Leonardo Fuentes', 1),
                     ('Nicolas Garcia'  , 1),
				     ('Agustina Ojeda'  , 1),
                     ('Joaquin Moya'    , 2),
                     ('Sofia Villanueva', 2),
					 ('Diego Carrizo'   , 2),
					 ('Fernando Rosso'  , 2)

-------------------------- INSERT TIPOS_PRODUCTOS --------------------------------------------
INSERT INTO Tipos_producto (descripcion)
					VALUES ('Autopartes'),
						   ('Automoviles')

-------------------------- INSERT MODELOS ----------------------------------------------------
INSERT INTO Modelos (modelo)
			 VALUES ('Argo'),
					('Cronos'),
					('Uno'),
					('Mobi'),
					('Fiorino'),
					('Ducato'),
					('Toro')

-------------------------- INSERT AUTOPLANES -------------------------------------------------
INSERT INTO autoplanes (nom_plan        , cant_coutas, tasa_interes)
				VALUES ('Tu Primer 0KM' , 24         , 15),
					   ('Plan Ahorro'   , 12         , 10),
					   ('Plan 50 Cuotas', 50         , 25),
					   ('Plan Automovil', 16         , 17)

-------------------------- INSERT TIPOS_VEHICULO ---------------------------------------------
insert into Tipos_vehiculo (descripcion)
					values ('Sedan'),
						   ('Pick up'),
						   ('Utilitario'),
						   ('Suv'),
						   ('Cupe'),
						   ('Van'),
						   ('Hatchback')

-------------------------- INSERT AUTOPARTES ------------------------------------------------
set dateformat ymd

insert into Autopartes (autoparte,precio,fec_fabricacion)
				values	('Correa de distribucion',	19129	,'2022-02-15'),
						('Disco de frenos',	17861	,'2022-01-26'),
						('Amortiguadores',	17745	,'2022-08-12'),
						('Lamparas',	11986	,'2022-12-15'),
						('Bateria',	10862	,'2022-12-23'),
						('Bujias',	15332	,'2022-07-27'),
						('Inyector',	9327	,'2022-12-18'),
						('Limpiaparabrisas',	10872	,'2022-04-07'),
						('Cubiertas',	19313	,'2021-12-13'),
						('Burro de arranque',	16654	,'2022-09-04')

-------------------------- INSERT AUTOMOVILES -----------------------------------------------
insert into Automoviles (cod_modelo,cod_tipo_vehiculo,color,precio,fec_fabricacion)
				values (	5	,	1	,'blanco',	4934137	,'2022-04-30'),
					   (	1	,	2	,'rojo',	4534165	,'2022-08-12'),
					   (	7	,	1	,'negro',	4804518	,'2022-10-11'),
					   (	5	,	7	,'azul',	4408912	,'2022-05-16'),
					   (	2	,	6	,'blanco',	4173716	,'2022-11-30'),
					   (	7	,	1	,'gris',	4296281	,'2022-07-26')

-------------------------- INSERT FACTURAS --------------------------------------------------
insert into Facturas (cod_empleado,fecha,nom_cliente,cuit       ,  cod_plan,cod_tipo_cliente)
			values (	5	,'2022-05-24','Erick Rojas',	23483912043	,	3	   ,	1	),
					(	3	,'2022-10-19','Miguel Ramirez',	25776031906	,	4	   ,	1	),
					(	1	,'2022-08-17','Silvia Carrillo',	22602625065	,	3	   ,	3	),
					(	1	,'2022-06-06','Natalia Cabrera', 20295707565,	3	   ,	1	),
					(	3	,'2022-05-29','Jose Oliva', 24375348629   ,	null	,	3	),
					(	3	,'2022-01-22','Leandro Pavon',20622776951	,	null	,	1	),
					(	5	,'2022-07-06','Franco Montes',	24534263550	,	null	,	2	),
					(	2	,'2022-12-24','Alfredo Villalba',21784166754	,	null	,	1	)

-------------------------- INSERT DETALLES FACTURA ------------------------------------------
insert into Detalles_factura (cod_factura,patente ,nro_serie,cantidad,precio)
					  values (	1		,	1004  ,	null	,	1	,	4160072	),
							(	2		,	1000  ,	null	,	1	,	4684958	),
							(	3		,	1004  ,	null	,	1	,	4333919	),
							(	4		,	1004  ,	null	,	1	,	4980734	),
							(	7		,	null  ,	9		,	1	,	17046	),
							(	8		,	null  ,	4		,	2	,	18951	),
							(	5		,	null  ,	3		,	3	,	16407	),
							(	8		,	null  ,	10		,	3	,	7732	),
							(	7		,	null  ,	2		,	3	,	13195	),
							(	6		,	null  ,	1		,	2	,	18614	),
							(	6		,	null  ,	5		,	3	,	12737	),
							(	5		,	null  ,	6		,	3	,	15291	),
							(	7		,	null  ,	7		,	3	,	13666	),
							(	6		,	null  ,	8		,	2	,	9944	)


----------------------------------------------------------------------------------------------------------------------------------------
--		                  PROCEDIMIENTOS ALMACENADOS

create procedure pa_modelos
as
begin
	select * from Modelos
end

create procedure pa_tipos_vehiculos
as
begin
	select * from Tipos_vehiculo
end

create procedure pa_autopartes
as
begin
	select nro_serie, autoparte from Autopartes
end



/*
--SP PROXIMO ID

CREATE PROCEDURE SP_PROXIMO_ID
@next int OUTPUT
AS
BEGIN
	SET @next = (SELECT MAX(cod_factura)+1  FROM Facturas);
END

--SP CONSULTAR CLIENTES

CREATE PROCEDURE SP_CONSULTAR_CLIENTES
AS
BEGIN
	SELECT cuit 'CUIL/CUIT', nom_cliente 'CLIENTE' FROM Facturas
END

--SP CONSULTAR PLANES

CREATE PROC SP_CONSULTAR_PLANES
as
begin
select * from Autoplanes
end

--SP CONSULTAR EMPLEADOS

CREATE PROC SP_CONSULTAR_EMPLEADOS
AS
BEGIN
SELECT cod_empleado, nombre + ' ' + apellido empleado from Empleados e join Personas p on p.cod_persona = e.cod_persona
END

--SP CONSULTAR TIPOS DE PRODUCTOS

create proc sp_consultar_tiposprod
as
begin
select * from Tipos_productos
end

--SP CONSULTAR AUTOMOVILES

create proc sp_consultar_autopartes
as
begin
select * from Productos where cod_tipo_producto = 1
end

--SP CONSULTAR AUTOPARTES

create proc sp_consultar_automoviles
as
begin
select * from Productos where cod_tipo_producto = 2
end

--SP INSERTAR FACTURA

CREATE PROCEDURE SP_INSERTAR_FACTURA
	@cod_cliente int,
	@cod_plan int = null,
	@cod_empleado int,
	@cod_factura int OUTPUT
AS
BEGIN
	INSERT INTO Facturas(fecha, cod_cliente, cod_plan, cod_empleado)
    VALUES (GETDATE(), @cod_cliente, @cod_plan, @cod_empleado);
    --Asignamos el valor del último ID autogenerado (obtenido --  
    --mediante la función SCOPE_IDENTITY() de SQLServer)	
    SET @cod_factura = SCOPE_IDENTITY();
END

--SP INSERTAR DETALLE

CREATE PROC SP_INSERTAR_DETALLE
	@cod_factura int,
	@cantidad int,
	@precio money,
	@cod_producto int
as
begin
	INSERT INTO Detalles_factura (cod_factura, cantidad, precio, cod_producto)
	VALUES (@cod_factura, @cantidad, @precio, @cod_producto)
end
*/