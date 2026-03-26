IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = 'hospital_bxb'
)
BEGIN
    CREATE DATABASE hospital_bxb;
END
GO

USE hospital_bxb;
GO

-- TABLE PATIENT
CREATE TABLE [dbo].[patient](
    [patient_id] INT IDENTITY(1,1) NOT NULL,
    [first_name] VARCHAR(200) NULL,
    [last_name] VARCHAR(200) NULL,
    [date_of_birth] DATE NOT NULL,
    [created_date] DATETIME DEFAULT GETUTCDATE(),
    [updated_date] DATETIME DEFAULT GETUTCDATE(),
    CONSTRAINT PK_patient PRIMARY KEY CLUSTERED ([patient_id] ASC)
);
GO

-- TABLE REFERRAL
CREATE TABLE [dbo].[referral](
    [referral_id] INT IDENTITY(1,1) NOT NULL,
    [patient_id] INT NULL,
    [referral_source] VARCHAR(200) NULL,
    [referral_type] VARCHAR(200) NULL,
    [referral_note] NVARCHAR(MAX) NULL,
    [created_date] DATETIME DEFAULT GETUTCDATE(),
    [updated_date] DATETIME DEFAULT GETUTCDATE(),
    CONSTRAINT PK_referral PRIMARY KEY CLUSTERED ([referral_id] ASC)
);
GO

-- FOREIGN KEY
ALTER TABLE [dbo].[referral]
ADD CONSTRAINT FK_referral_patient
FOREIGN KEY ([patient_id])
REFERENCES [dbo].[patient] ([patient_id]);
GO

-- INSERT DATA
INSERT INTO patient(first_name, last_name, date_of_birth)
VALUES ('Budiwahyu', 'Adita', '1992-12-13');

INSERT INTO referral (patient_id, referral_source, referral_type, referral_note)
VALUES 
(1, 'Hospital','Short Stay','Patient recently hospitalized with pneumonia.'),
(1, 'General Practitioner','Specialist Consultation','Referred to neurologist for persistent migraines.');