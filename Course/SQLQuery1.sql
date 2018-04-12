CREATE TABLE [dbo].[Группа] (
    [Id]       INT          NOT NULL IDENTITY,
    [Год поступления]     INT   NULL,
    [Куратор]  INT  NULL,
    [Название] NVARCHAR (50) NULL,
    [Тип обучения] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Студент] (
    [Id]               INT          NOT NULL IDENTITY ,
    [Имя]              NVARCHAR(50) NULL,
    [Фамилия]          NVARCHAR(50) NULL,
    [Отчество]         NVARCHAR(50) NULL,
    [Дата поступления] DATE         NULL,
    [Дата рождения]	   DATE         NULL,
    [Группа]           INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Студент_Группа] FOREIGN KEY ([Группа]) REFERENCES [dbo].[Группа] ([Id])
);

CREATE TABLE [dbo].[Преподаватель] (
    [Id]       INT        NOT NULL IDENTITY,
    [Имя]      NVARCHAR (50) NULL,
    [Фамилия]  NVARCHAR (50) NULL,
    [Отчество] NVARCHAR (50) NULL,
    [Дата рождения]	   DATE         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Предмет] (
    [Id]                INT        NOT NULL IDENTITY,
    [Название]          NVARCHAR (50) NULL,
    [Способ оценивания] NVARCHAR (50) NULL,
    [Количество часов]  INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Преподаватель_предмет] (
    [Id]            INT        NOT NULL IDENTITY,
    [Преподаватель] INT NULL,
    [Предмет]       INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Преподаватель_предмет_Предмет] FOREIGN KEY ([Предмет]) REFERENCES [dbo].[Предмет] ([Id]),
    CONSTRAINT [FK_Преподаватель_предмет_Преподаватель] FOREIGN KEY ([Преподаватель]) REFERENCES [dbo].[Преподаватель] ([Id])
);

CREATE TABLE [dbo].[Студент_предмет] (
    [Id]              INT        NOT NULL IDENTITY,
    [Студент]         INT        NULL,
    [Предмет]         INT        NULL,
    [Время изучения]  INT        NULL,
    [Статус изучения] NCHAR (10) NULL,
    [Оценка] NCHAR (16) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Группа_предмет_Предмет] FOREIGN KEY ([Предмет]) REFERENCES [dbo].[Предмет] ([Id]),
    CONSTRAINT [FK_Группа_предмет_Студент] FOREIGN KEY ([Студент]) REFERENCES [dbo].[Студент] ([Id])
);
GO
/*
INSERT INTO [Преподаватель]
			   ([Имя]
			   ,[Фамилия]
			   ,[Отчество]
			   ,[Дата рождения])
     VALUES
           ('Евгений', 'Армейский', 'Андреевич', '20-01-1971')
           , ('Сергей', 'Еримеев', 'Андреевич', '20-01-1973')
           , ('Андрей', 'Авганский', 'Сергеевич', '11-02-1970')
           , ('Евгений', 'Еримеев', 'Андреевич', '02-01-1979')
           , ('Александр', 'Полянский', 'Сергеевич', '11-01-1972')
GO

INSERT INTO [Группа]
			   ([Год поступления]
			   ,[Куратор]
			   ,[Название]
			   ,[Тип обучения])
     VALUES
           (2011, 1, '15ТТ3', 'Бакалавриат')
           , (2011, 1, '15ТТ3', 'Бакалавриат')
           , (2012, 2, '16ТТ3', 'Бакалавриат')
           , (2012, 4, '16ТТ3', 'Бакалавриат')
GO

INSERT INTO [Студент]
			   ([Имя]
			   ,[Фамилия]
			   ,[Отчество]
			   ,[Дата рождения]
			   ,[Дата поступления]
			   ,[Группа])
     VALUES
           ('Сергей', 'Полянский', 'Андреевич', '20-01-2013', '10-03-1997', 1)
           , ('Сергей', 'Полянский', 'Андреевич', '20-01-2013', '23-03-1997', 1)
           , ('Андрей', 'Авганский', 'Сергеевич', '11-02-2013', '20-03-1997', 2)
           , ('Евгений', 'Еримеев', 'Андреевич', '02-01-2013', '15-03-1997', 2)
           , ('Александр', 'Армейский', 'Сергеевич', '11-01-2013', '10-03-1997', 1)
GO
*/