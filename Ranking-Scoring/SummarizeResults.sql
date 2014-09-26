IF OBJECT_ID('SC_SummarizePerformanceResults') IS NULL
    EXEC('CREATE PROCEDURE SC_SummarizePerformanceResults AS SELECT 1')
GO

-- =============================================
-- Author:		JOE C
-- Create date: 7/11/201?
-- Description:	Summarizes performance results and calculates the raw and weighted ranks/scores for each team member.
-- =============================================
ALTER PROCEDURE [dbo].[SC_SummarizePerformanceResults]
	@ResultType varchar(25),
	@FromDate DateTime,
	@ThroughDate DateTime,
	@WeightingPercentile decimal(3,3)
WITH RECOMPILE
AS
BEGIN

	SET NOCOUNT ON;
	SET ANSI_NULLS ON;

	SET @WeightingPercentile = 1 - @WeightingPercentile

	SELECT TOP 100 PERCENT
		-- General info
		 CTMI.ScNumber
		,CTMI.Active
		,CTMI.Store
		,CTMI.TeamMemberFirstName
		,CTMI.TeamMemberNickname
		,CTMI.TeamMemberLastName
		,CTMI.FlsaExempt
		,CTMI.ReportsToId
		,CTMI.ReportsToFirstName
		,CTMI.ReportsToNickname
		,CTMI.ReportsToLastName

		-- Raw Stats
		,CASE CTMI.FlsaExempt
			WHEN 0 THEN ISNULL(SUM(DHWS.HoursWorked),0)
			ELSE 0
		 END AS HoursWorked
		,ISNULL(SUM(DSCS.ScCogs),0) AS Raw_ScCogs
		,ISNULL(SUM(DSCS.ScItemCount),0) AS Raw_ScItemCount
		,ISNULL(SUM(DSCS.ScNetSales),0) AS Raw_ScNetSales
		,ISNULL(SUM(DSCS.ScTransactionCount),0) AS Raw_ScTransactionCount
		,ISNULL(SUM(DSOS.SoCogs),0) AS Raw_SoCogs
		,ISNULL(SUM(DSOS.SoItemCount),0) AS Raw_SoItemCount
		,ISNULL(SUM(DSOS.SoNetSales),0) AS Raw_SoNetSales
		,ISNULL(SUM(DSOS.SoTransactionCount),0) AS Raw_SoTransactionCount
		,ISNULL(SUM(DPS.PrAddonClassesPotential),0) AS Raw_PrAddonClassesPotential
		,ISNULL(SUM(DPS.PrAddonClassesSold),0) AS Raw_PrAddonClassesSold
		,dbo.SafeDivide(ISNULL(SUM(DPS.PrAddonClassesSold),0),
						ISNULL(SUM(DPS.PrAddonClassesPotential),0)) AS Raw_PrCompletionPercent
		,ISNULL(SUM(DPS.PrTransactionCogs),0) AS Raw_PrTransactionCogs
		,ISNULL(SUM(DPS.PrTransactionCount),0) AS Raw_PrTransactionCount
		,ISNULL(SUM(DPS.PrTransactionItemCount),0) AS Raw_PrTransactionItemCount
		,ISNULL(SUM(DPS.PrTransactionNetSales),0) AS Raw_PrTransactionNetSales

		-- Raw Ranks (across company)
		,RANK() OVER (ORDER BY ISNULL(SUM(DHWS.HoursWorked),0) DESC) AS Rank_Raw_HoursWorked
		,RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScCogs),0) DESC) AS Rank_Raw_ScCogs
		,RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScItemCount),0) DESC) AS Rank_Raw_ScItemCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScNetSales),0) DESC) AS Rank_Raw_ScNetSales
		,RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScTransactionCount),0) DESC) AS Rank_Raw_ScTransactionCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoCogs),0) DESC) AS Rank_Raw_SoCogs
		,RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoItemCount),0) DESC) AS Rank_Raw_SoItemCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoNetSales),0) DESC) AS Rank_Raw_SoNetSales
		,RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoTransactionCount),0) DESC) AS Rank_Raw_SoTransactionCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrAddonClassesPotential),0) DESC) AS Rank_Raw_PrAddonClassesPotential
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrAddonClassesSold),0) DESC) AS Rank_Raw_PrAddonClassesSold
		,RANK() OVER (ORDER BY dbo.SafeDivide(ISNULL(SUM(DPS.PrAddonClassesSold),0),
											  ISNULL(SUM(DPS.PrAddonClassesPotential),0)) DESC) AS Rank_Raw_PrCompletionPercent
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrTransactionCogs),0) DESC) AS Rank_Raw_PrTransactionCogs
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrTransactionCount),0) DESC) AS Rank_Raw_PrTransactionCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrTransactionItemCount),0) DESC) AS Rank_Raw_PrTransactionItemCount
		,RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrTransactionNetSales),0) DESC) AS Rank_Raw_PrTransactionNetSales

		-- Raw Scoring
		,CASE CTMI.FlsaExempt
			WHEN 1 THEN 0
			ELSE 
			(
			  RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScNetSales),0) DESC)
			+ RANK() OVER (ORDER BY ISNULL(SUM(DSCS.ScTransactionCount),0) DESC)
			+ RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoNetSales),0) DESC)
			+ RANK() OVER (ORDER BY ISNULL(SUM(DSOS.SoTransactionCount),0) DESC)
			+ RANK() OVER (ORDER BY dbo.SafeDivide(ISNULL(SUM(DPS.PrAddonClassesSold),0),
												   ISNULL(SUM(DPS.PrAddonClassesPotential),0)) DESC)
			+ RANK() OVER (ORDER BY ISNULL(SUM(DPS.PrTransactionCount),0) DESC)
			) / 6.0
		 END AS Raw_Score

		-- Splits
		,ISNULL(SUM(DSCS.Split_ScCogs),0) AS Split_Raw_ScCogs
		,ISNULL(SUM(DSCS.Split_ScItemCount),0) AS Split_Raw_ScItemCount
		,ISNULL(SUM(DSCS.Split_ScNetSales),0) AS Split_Raw_ScNetSales
		,ISNULL(SUM(DSOS.Split_SoCogs),0) AS Split_Raw_SoCogs
		,ISNULL(SUM(DSOS.Split_SoitemCount),0) AS Split_Raw_SoitemCount
		,ISNULL(SUM(DSOS.Split_SoNetSales),0) AS Split_Raw_SoNetSales
		,ISNULL(SUM(DPS.Split_PrAddonClassesPotential),0) AS Split_Raw_PrAddonClassesPotential
		,ISNULL(SUM(DPS.Split_PrAddonClassesSold),0) AS Split_Raw_PrAddonClassesSold
		,ISNULL(SUM(DPS.Split_PrTransactionCogs),0) AS Split_Raw_PrTransactionCogs
		,ISNULL(SUM(DPS.Split_PrTransactionItemCount),0) AS Split_Raw_PrTransactionItemCount
		,ISNULL(SUM(DPS.Split_PrTransactionNetSales),0) AS Split_Raw_PrTransactionNetSales
	INTO
	    #RawScores
	FROM
		SC_CurrentTeamMemberInfo AS CTMI
		LEFT JOIN SC_DailyHoursWorkedStats AS DHWS
			ON DHWS.ScNumber = CTMI.ScNumber
		LEFT JOIN SC_DailyPerformanceStats AS DSCS
			ON DHWS.ScNumber = DSCS.ScNumber
			AND DHWS.Date = DSCS.Date
		LEFT JOIN SC_DailyProjectStats AS DPS
			ON DHWS.ScNumber = DPS.ScNumber
			AND DHWS.Date = DPS.Date
		LEFT JOIN SC_DailySpecialOrderStats AS DSOS
			ON DHWS.ScNumber = DSOS.ScNumber
			AND DHWS.Date = DSOS.Date
	WHERE
			CTMI.Store IS NOT NULL
		AND DHWS.Date BETWEEN @FromDate AND @ThroughDate
		AND CTMI.FlsaExempt = 0
	GROUP BY
		 CTMI.ScNumber
		,CTMI.Active
		,CTMI.Store
		,CTMI.TeamMemberFirstName
		,CTMI.TeamMemberNickname
		,CTMI.TeamMemberLastName
		,CTMI.FlsaExempt
		,CTMI.ReportsToId
		,CTMI.ReportsToFirstName
		,CTMI.ReportsToNickname
		,CTMI.ReportsToLastName
	ORDER BY
		CTMI.Store,
		CTMI.ReportsToLastName,
		CTMI.ScNumber

	DECLARE @NTileHours decimal(12,3)
	DECLARE @MaxHoursRank decimal(12,3)
	SET @MaxHoursRank = (SELECT MAX(Rank_Raw_HoursWorked) FROM #RawScores)
	SET @NTileHours = (
		SELECT TOP 1
			HoursWorked
		FROM
			#RawScores RS
		ORDER BY
			ABS(@WeightingPercentile - dbo.SafeDivide(RS.Rank_Raw_HoursWorked, @MaxHoursRank)) ASC
		);

	WITH Averages
		(
		Avg_ScNetSales,
		Avg_ScTransactionCount,
		Avg_SoNetSales,
		Avg_SoTransactionCount,
		Avg_PrCompletionPercent,
		Avg_PrTransactionCount
		)
	AS (
		SELECT
			 AVG(RS.Raw_ScNetSales)
			,AVG(RS.Raw_ScTransactionCount)
			,AVG(RS.Raw_SoNetSales)
			,AVG(RS.Raw_SoTransactionCount)
			,AVG(RS.Raw_PrCompletionPercent)
			,AVG(RS.Raw_PrTransactionCount)
		FROM
			#RawScores RS
		)
	SELECT 
		 R.ScNumber
		,Weighted_ScNetSales = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_ScNetSales,A.Avg_ScNetSales,@NTileHours)
		,Weighted_ScTransactionCount = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_ScTransactionCount,A.Avg_ScTransactionCount,@NTileHours)
		,Weighted_SoNetSales_Weighted = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_SoNetSales,A.Avg_SoNetSales,@NTileHours)
		,Weighted_SoTransactionCount = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_SoTransactionCount,A.Avg_SoTransactionCount,@NTileHours)
		,Weighted_PrCompletionPercent = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_PrCompletionPercent,A.Avg_PrCompletionPercent,@NTileHours)
		,Weighted_PrTransactionCount = dbo.SC_BayesianWeight(R.HoursWorked,R.Raw_PrTransactionCount,A.Avg_PrTransactionCount,@NTileHours)
	INTO
		#WeightedScores
	FROM
		#RawScores R
		CROSS JOIN Averages A
	ORDER BY
		R.HoursWorked ASC

	SELECT
		 ScNumber
		,RANK() OVER (ORDER BY AVG(Weighted_ScNetSales) DESC) AS Rank_Weighted_ScNetSales
		,RANK() OVER (ORDER BY AVG(Weighted_ScTransactionCount) DESC) AS Rank_Weighted_ScTransactionCount
		,RANK() OVER (ORDER BY AVG(Weighted_SoNetSales_Weighted) DESC) AS Rank_Weighted_SoNetSales
		,RANK() OVER (ORDER BY AVG(Weighted_SoTransactionCount) DESC) AS Rank_Weighted_SoTransactionCount
		,RANK() OVER (ORDER BY AVG(Weighted_PrCompletionPercent) DESC) AS Rank_Weighted_PrCompletionPercent
		,RANK() OVER (ORDER BY AVG(Weighted_PrTransactionCount) DESC) AS Rank_Weighted_PrTransactionCount
	INTO
		#WeightedRanks
	FROM
		#WeightedScores
	GROUP BY
		ScNumber

	SELECT
		 RS.ScNumber
		,1 - dbo.SafeDivide(RS.Rank_Raw_HoursWorked,(SELECT MAX(Rank_Raw_HoursWorked) FROM #RawScores)) AS Norm_Raw_HoursWorked
		,1 - dbo.SafeDivide(RS.Rank_Raw_ScCogs,(SELECT MAX(Rank_Raw_ScCogs) FROM #RawScores)) AS Norm_Raw_ScCogs
		,1 - dbo.SafeDivide(RS.Rank_Raw_ScItemCount,(SELECT MAX(Rank_Raw_ScItemCount) FROM #RawScores)) AS Norm_Raw_ScItemCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_ScNetSales,(SELECT MAX(Rank_Raw_ScNetSales) FROM #RawScores)) AS Norm_Raw_ScNetSales
		,1 - dbo.SafeDivide(RS.Rank_Raw_ScTransactionCount,(SELECT MAX(Rank_Raw_ScTransactionCount) FROM #RawScores)) AS Norm_Raw_ScTransactionCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_SoCogs,(SELECT MAX(Rank_Raw_SoCogs) FROM #RawScores)) AS Norm_Raw_SoCogs
		,1 - dbo.SafeDivide(RS.Rank_Raw_SoItemCount,(SELECT MAX(Rank_Raw_SoItemCount) FROM #RawScores)) AS Norm_Raw_SoItemCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_SoNetSales,(SELECT MAX(Rank_Raw_SoNetSales) FROM #RawScores)) AS Norm_Raw_SoNetSales
		,1 - dbo.SafeDivide(RS.Rank_Raw_SoTransactionCount,(SELECT MAX(Rank_Raw_SoTransactionCount) FROM #RawScores)) AS Norm_Raw_SoTransactionCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrAddonClassesPotential,(SELECT MAX(Rank_Raw_PrAddonClassesPotential) FROM #RawScores)) AS Norm_Raw_PrAddonClassesPotential
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrAddonClassesSold,(SELECT MAX(Rank_Raw_PrAddonClassesSold) FROM #RawScores)) AS Norm_Raw_PrAddonClassesSold
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrCompletionPercent,(SELECT MAX(Rank_Raw_PrCompletionPercent) FROM #RawScores)) AS Norm_Raw_PrCompletionPercent
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrTransactionCogs,(SELECT MAX(Rank_Raw_PrTransactionCogs) FROM #RawScores)) AS Norm_Raw_PrTransactionCogs
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrTransactionCount,(SELECT MAX(Rank_Raw_PrTransactionCount) FROM #RawScores)) AS Norm_Raw_PrTransactionCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrTransactionItemCount,(SELECT MAX(Rank_Raw_PrTransactionItemCount) FROM #RawScores)) AS Norm_Raw_PrTransactionItemCount
		,1 - dbo.SafeDivide(RS.Rank_Raw_PrTransactionNetSales,(SELECT MAX(Rank_Raw_PrTransactionNetSales) FROM #RawScores)) AS Norm_Raw_PrTransactionNetSales
		,1 - dbo.SafeDivide(WR.Rank_Weighted_ScNetSales,(SELECT MAX(Rank_Weighted_ScNetSales) FROM #WeightedRanks)) AS Norm_Weighted_ScNetSales
		,1 - dbo.SafeDivide(WR.Rank_Weighted_ScTransactionCount,(SELECT MAX(Rank_Weighted_ScTransactionCount) FROM #WeightedRanks)) AS Norm_Weighted_ScTransactionCount
		,1 - dbo.SafeDivide(WR.Rank_Weighted_SoNetSales,(SELECT MAX(Rank_Weighted_SoNetSales) FROM #WeightedRanks)) AS Norm_Weighted_SoNetSales
		,1 - dbo.SafeDivide(WR.Rank_Weighted_SoTransactionCount,(SELECT MAX(Rank_Weighted_SoTransactionCount) FROM #WeightedRanks)) AS Norm_Weighted_SoTransactionCount
		,1 - dbo.SafeDivide(WR.Rank_Weighted_PrCompletionPercent,(SELECT MAX(Rank_Weighted_PrCompletionPercent) FROM #WeightedRanks)) AS Norm_Weighted_PrCompletionPercent
		,1 - dbo.SafeDivide(WR.Rank_Weighted_PrTransactionCount,(SELECT MAX(Rank_Weighted_PrTransactionCount) FROM #WeightedRanks)) AS Norm_Weighted_PrTransactionCount
	INTO
		#NormalizedScores
	FROM
		#RawScores RS
		JOIN #WeightedRanks WR
			ON RS.ScNumber = WR.ScNumber

	SELECT
		 @ResultType AS 'ResultType'
		,@FromDate AS 'FromDate'
		,@ThroughDate AS 'ThroughDate'
		,RS.ScNumber
		,RS.Active
		,RS.Store
		,RS.TeamMemberFirstName
		,RS.TeamMemberNickname
		,RS.TeamMemberLastName
		,RS.FlsaExempt
		,RS.ReportsToId
		,RS.ReportsToFirstName
		,RS.ReportsToNickname
		,RS.ReportsToLastName
		,RS.HoursWorked
		,RS.Raw_ScCogs
		,RS.Raw_ScItemCount
		,RS.Raw_ScNetSales
		,RS.Raw_ScTransactionCount
		,RS.Raw_SoCogs
		,RS.Raw_SoItemCount
		,RS.Raw_SoNetSales
		,RS.Raw_SoTransactionCount
		,RS.Raw_PrAddonClassesPotential
		,RS.Raw_PrAddonClassesSold
		,RS.Raw_PrCompletionPercent
		,RS.Raw_PrTransactionCogs
		,RS.Raw_PrTransactionCount
		,RS.Raw_PrTransactionItemCount
		,RS.Raw_PrTransactionNetSales
		,RS.Rank_Raw_HoursWorked
		,RS.Rank_Raw_ScCogs
		,RS.Rank_Raw_ScItemCount
		,RS.Rank_Raw_ScNetSales
		,RS.Rank_Raw_ScTransactionCount
		,RS.Rank_Raw_SoCogs
		,RS.Rank_Raw_SoItemCount
		,RS.Rank_Raw_SoNetSales
		,RS.Rank_Raw_SoTransactionCount
		,RS.Rank_Raw_PrAddonClassesPotential
		,RS.Rank_Raw_PrAddonClassesSold
		,RS.Rank_Raw_PrCompletionPercent
		,RS.Rank_Raw_PrTransactionCogs
		,RS.Rank_Raw_PrTransactionCount
		,RS.Rank_Raw_PrTransactionItemCount
		,RS.Rank_Raw_PrTransactionNetSales
		,RS.Raw_Score
		,RS.Split_Raw_ScCogs
		,RS.Split_Raw_ScItemCount
		,RS.Split_Raw_ScNetSales
		,RS.Split_Raw_SoCogs
		,RS.Split_Raw_SoitemCount
		,RS.Split_Raw_SoNetSales
		,RS.Split_Raw_PrAddonClassesPotential
		,RS.Split_Raw_PrAddonClassesSold
		,RS.Split_Raw_PrTransactionCogs
		,RS.Split_Raw_PrTransactionItemCount
		,RS.Split_Raw_PrTransactionNetSales
		,WS.Weighted_ScNetSales
		,WS.Weighted_ScTransactionCount
		,WS.Weighted_SoNetSales_Weighted
		,WS.Weighted_SoTransactionCount
		,WS.Weighted_PrCompletionPercent
		,WS.Weighted_PrTransactionCount
		,WR.Rank_Weighted_ScNetSales
		,WR.Rank_Weighted_ScTransactionCount
		,WR.Rank_Weighted_SoNetSales
		,WR.Rank_Weighted_SoTransactionCount
		,WR.Rank_Weighted_PrCompletionPercent
		,WR.Rank_Weighted_PrTransactionCount
		,NS.Norm_Raw_HoursWorked
		,NS.Norm_Raw_ScCogs
		,NS.Norm_Raw_ScItemCount
		,NS.Norm_Raw_ScNetSales	-- Scored
		,NS.Norm_Raw_ScTransactionCount	-- Scored
		,NS.Norm_Raw_SoCogs
		,NS.Norm_Raw_SoItemCount
		,NS.Norm_Raw_SoNetSales	-- Scored
		,NS.Norm_Raw_SoTransactionCount	-- Scored
		,NS.Norm_Raw_PrAddonClassesPotential
		,NS.Norm_Raw_PrAddonClassesSold
		,NS.Norm_Raw_PrCompletionPercent -- Scored
		,NS.Norm_Raw_PrTransactionCogs
		,NS.Norm_Raw_PrTransactionCount	-- Scored
		,NS.Norm_Raw_PrTransactionItemCount
		,NS.Norm_Raw_PrTransactionNetSales
		,NS.Norm_Weighted_ScNetSales
		,NS.Norm_Weighted_ScTransactionCount
		,NS.Norm_Weighted_SoNetSales
		,NS.Norm_Weighted_SoTransactionCount
		,NS.Norm_Weighted_PrCompletionPercent
		,NS.Norm_Weighted_PrTransactionCount
		,RawScore =  (NS.Norm_Raw_ScNetSales
						+ NS.Norm_Raw_ScTransactionCount
						+ NS.Norm_Raw_SoNetSales
						+ NS.Norm_Raw_SoTransactionCount
						+ NS.Norm_Raw_PrCompletionPercent
						+ NS.Norm_Raw_PrTransactionCount
						) / 6
		,WeightedScore = (NS.Norm_Weighted_ScNetSales
							+ NS.Norm_Weighted_ScTransactionCount
							+ NS.Norm_Weighted_SoNetSales
							+ NS.Norm_Weighted_SoTransactionCount
							+ NS.Norm_Weighted_PrCompletionPercent
							+ NS.Norm_Weighted_PrTransactionCount
							 ) / 6
		,GETDATE() AS DateUpdated
	FROM
		#RawScores RS
		JOIN #WeightedRanks WR
			ON RS.ScNumber = WR.ScNumber
		JOIN #WeightedScores WS
			ON RS.ScNumber = WS.ScNumber
		JOIN #NormalizedScores NS
			ON RS.ScNumber = NS.ScNumber

END

GO
