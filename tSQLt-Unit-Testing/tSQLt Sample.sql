EXEC tSQLt.NewTestClass 'EligibilityTests'
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 1]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 1000.01,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('E', 1)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 2]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 999.99,
		@ProjectedPreEnrollmentDateHours = 1000.01,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('P', 2)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 3]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 999.99,
		@ProjectedPreEnrollmentDateHours = 999.99,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 3)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 4]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 0,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 4)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 5]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 0,
		@ProjectedHoursByAnniveraryDate = 999.99,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 5)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 6]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 0,
		@ProjectedHoursByAnniveraryDate = 1000.01,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('P', 6)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 7]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 1,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = 999.99

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 7)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 8]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 1,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = 1000.01

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('E', 8)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 9]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 0,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 9)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 10]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 19,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 10)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 11]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 0,
		@CalDaysOfServiceByEnrollmentDate = 364,
		@AgeByEnrollmentDate = NULL,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 11)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 12]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 1,
		@PreviouslyEnrolled = 1,
		@CalDaysOfServiceByEnrollmentDate = NULL,
		@AgeByEnrollmentDate = NULL,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('I', 12)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 13]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 1000.01,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('E', 13)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 14]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 999.99,
		@ProjectedPreEnrollmentDateHours = 1000.01,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('P', 14)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 15]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 0,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = 999.99,
		@ProjectedPreEnrollmentDateHours = 999.99,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 15)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 16]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 0,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 16)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 17]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 0,
		@ProjectedHoursByAnniveraryDate = 999.99,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 17)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 18]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 0,
		@ProjectedHoursByAnniveraryDate = 1000.01,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('P', 18)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 19]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 1,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = 999.99

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 19)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 20]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 1,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = 1,
		@AnniversaryDateHasPassed = 1,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = 1000.01

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('E', 20)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 21]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 21,
		@TargettedEnrollmentDate = 1,
		@AnniversaryBetweenJan1AndJul1 = 0,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 21)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 22]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 366,
		@AgeByEnrollmentDate = 19,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 22)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 23]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 0,
		@Rehire = 0,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = 364,
		@AgeByEnrollmentDate = NULL,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 23)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 24]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 1,
		@CurrentlyEnrolled = 1,
		@Rehire = NULL,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = NULL,
		@AgeByEnrollmentDate = NULL,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 24)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

CREATE PROCEDURE EligibilityTests.[Test Scenario 25]
AS
BEGIN
    DECLARE @ActiveEmployee BIT,
		@CurrentlyEnrolled BIT,
		@Rehire BIT,
		@PreviouslyEnrolled BIT,
		@CalDaysOfServiceByEnrollmentDate INT,
		@AgeByEnrollmentDate INT,
		@TargettedEnrollmentDate BIT,
		@AnniversaryBetweenJan1AndJul1 BIT,
		@HoursWorkedSinceJan1 DECIMAL(6,2),
		@ProjectedPreEnrollmentDateHours DECIMAL(6,2),
		@FirstAnniversaryIsThisYear BIT,
		@AnniversaryDateHasPassed BIT,
		@ProjectedHoursByAnniveraryDate DECIMAL(6,2),
		@HoursWorkedPriorToAnniversary DECIMAL(6,2)

	SELECT @ActiveEmployee = 0,
		@CurrentlyEnrolled = NULL,
		@Rehire = NULL,
		@PreviouslyEnrolled = NULL,
		@CalDaysOfServiceByEnrollmentDate = NULL,
		@AgeByEnrollmentDate = NULL,
		@TargettedEnrollmentDate = NULL,
		@AnniversaryBetweenJan1AndJul1 = NULL,
		@HoursWorkedSinceJan1 = NULL,
		@ProjectedPreEnrollmentDateHours = NULL,
		@FirstAnniversaryIsThisYear = NULL,
		@AnniversaryDateHasPassed = NULL,
		@ProjectedHoursByAnniveraryDate = NULL,
		@HoursWorkedPriorToAnniversary = NULL

	CREATE TABLE #ExpectedResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ExpectedResult
		VALUES ('N', 25)

	CREATE TABLE #ActualResult (
		EligibilityResult CHAR(1),
		Scenario INT
		)

	INSERT INTO #ActualResult
	SELECT
		EligibilityResult,
		Scenario 
	FROM [dbo].[Corp_Acct_CheckEligibility] (
		  @ActiveEmployee,
		  @CurrentlyEnrolled,
		  @Rehire,
		  @PreviouslyEnrolled,
		  @CalDaysOfServiceByEnrollmentDate,
		  @AgeByEnrollmentDate,
		  @TargettedEnrollmentDate,
		  @AnniversaryBetweenJan1AndJul1,
		  @HoursWorkedSinceJan1,
		  @ProjectedPreEnrollmentDateHours,
		  @FirstAnniversaryIsThisYear,
		  @AnniversaryDateHasPassed,
		  @ProjectedHoursByAnniveraryDate,
		  @HoursWorkedPriorToAnniversary)

	EXEC tSQLt.AssertEqualsTable '#ExpectedResult', '#ActualResult'
END
GO

EXEC tSQLt.RunAll
GO