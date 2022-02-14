
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/24/2021 14:22:19
-- Generated from EDMX file: F:\5032\ass\FIT5032_Ass\MyWarmPetWeb\MyWarmPetWeb\Models\MyWarmPetWebModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-MyWarmPetWeb-20210913081814];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUsersReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReviewSet] DROP CONSTRAINT [FK_AspNetUsersReview];
GO
IF OBJECT_ID(N'[dbo].[FK_VetReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReviewSet] DROP CONSTRAINT [FK_VetReview];
GO
IF OBJECT_ID(N'[dbo].[FK_VetConsultationTime]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConsultationTimeSet] DROP CONSTRAINT [FK_VetConsultationTime];
GO
IF OBJECT_ID(N'[dbo].[FK_ConsultationTimeBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingSet] DROP CONSTRAINT [FK_ConsultationTimeBooking];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUsersBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingSet] DROP CONSTRAINT [FK_AspNetUsersBooking];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[ServiceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceSet];
GO
IF OBJECT_ID(N'[dbo].[VetSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VetSet];
GO
IF OBJECT_ID(N'[dbo].[ReviewSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReviewSet];
GO
IF OBJECT_ID(N'[dbo].[ConsultationTimeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConsultationTimeSet];
GO
IF OBJECT_ID(N'[dbo].[BookingSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookingSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'ServiceSet'
CREATE TABLE [dbo].[ServiceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ServiceName] nvarchar(max)  NOT NULL,
    [ServiceCharacteristic] nvarchar(max)  NOT NULL,
    [ServiceDetail] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'VetSet'
CREATE TABLE [dbo].[VetSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VetName] nvarchar(max)  NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [PhoneNumber] int  NOT NULL,
    [Rate] float  NOT NULL
);
GO

-- Creating table 'ReviewSet'
CREATE TABLE [dbo].[ReviewSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ReviewTitle] nvarchar(max)  NOT NULL,
    [ReviewContent] nvarchar(max)  NOT NULL,
    [RateOfVet] float  NOT NULL,
    [AspNetUsersId] nvarchar(128)  NOT NULL,
    [VetId] int  NOT NULL
);
GO

-- Creating table 'ConsultationTimeSet'
CREATE TABLE [dbo].[ConsultationTimeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateAndTime] datetime  NOT NULL,
    [BookingStatus] nvarchar(max)  NOT NULL,
    [VetId] int  NOT NULL
);
GO

-- Creating table 'BookingSet'
CREATE TABLE [dbo].[BookingSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerName] nvarchar(max)  NOT NULL,
    [PetName] nvarchar(max)  NOT NULL,
    [PetType] nvarchar(max)  NOT NULL,
    [CustomerPhoneNumber] int  NOT NULL,
    [DateAndTime] datetime  NOT NULL,
    [VetId] int  NOT NULL,
    [AspNetUsersId] nvarchar(128)  NOT NULL,
    [ConsultationTime_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceSet'
ALTER TABLE [dbo].[ServiceSet]
ADD CONSTRAINT [PK_ServiceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VetSet'
ALTER TABLE [dbo].[VetSet]
ADD CONSTRAINT [PK_VetSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [PK_ReviewSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ConsultationTimeSet'
ALTER TABLE [dbo].[ConsultationTimeSet]
ADD CONSTRAINT [PK_ConsultationTimeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [PK_BookingSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AspNetUsersId] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [FK_AspNetUsersReview]
    FOREIGN KEY ([AspNetUsersId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUsersReview'
CREATE INDEX [IX_FK_AspNetUsersReview]
ON [dbo].[ReviewSet]
    ([AspNetUsersId]);
GO

-- Creating foreign key on [VetId] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [FK_VetReview]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[VetSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VetReview'
CREATE INDEX [IX_FK_VetReview]
ON [dbo].[ReviewSet]
    ([VetId]);
GO

-- Creating foreign key on [VetId] in table 'ConsultationTimeSet'
ALTER TABLE [dbo].[ConsultationTimeSet]
ADD CONSTRAINT [FK_VetConsultationTime]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[VetSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VetConsultationTime'
CREATE INDEX [IX_FK_VetConsultationTime]
ON [dbo].[ConsultationTimeSet]
    ([VetId]);
GO

-- Creating foreign key on [ConsultationTime_Id] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [FK_ConsultationTimeBooking]
    FOREIGN KEY ([ConsultationTime_Id])
    REFERENCES [dbo].[ConsultationTimeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConsultationTimeBooking'
CREATE INDEX [IX_FK_ConsultationTimeBooking]
ON [dbo].[BookingSet]
    ([ConsultationTime_Id]);
GO

-- Creating foreign key on [AspNetUsersId] in table 'BookingSet'
ALTER TABLE [dbo].[BookingSet]
ADD CONSTRAINT [FK_AspNetUsersBooking]
    FOREIGN KEY ([AspNetUsersId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUsersBooking'
CREATE INDEX [IX_FK_AspNetUsersBooking]
ON [dbo].[BookingSet]
    ([AspNetUsersId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------