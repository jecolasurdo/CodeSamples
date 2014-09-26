Ranking and Scoring
===================

I had a fun opportunity to design, develop, and oversee a system that provides quantitative feedback regarding the interaction of several hundred sales team members and their customers. Information from this system was used to provide individual sales team members with feedback about their performance as well as provide very high level information to executives about things such as a quantification of customer service in terms of sales and profit lift.

Due to an interesting set of constraints, almost the entire system was engineered using tSQL.

What I've included here is a rather massive query that is responsible for one of the final steps in the ranking process.

There are several interesting things to note about this procedure:
 - It is written in an almost entirely set-based and declarative fashion, which allows SQL Server to process the procedure rather efficiently in spite of its size.
 - Much of the calculations were able to be performed through the use of Common Table Expressions and Partitioning clauses, which allow the queries to remain set-based, while retaining legibility and performance.
 - This particular procedure has been written with the WITH RECOMPILE option set. https://github.com/jecolasurdo/CodeSamples/blob/master/Ranking-Scoring/SummarizeResults.sql#L15 This is one of the few times I've needed to use this, as I discovered, in this case, if the statistics for this procedure are cached, it tends to run very poorly. 
 - In this procedure I am making a number of calls to various functions. One such example is the function that applies the Bayesian weigting to the scores (sorry, I couldn't include that code). https://github.com/jecolasurdo/CodeSamples/blob/master/Ranking-Scoring/SummarizeResults.sql#L176 Calling functions in this way is generally a bad idea since it prevents parallelism. However, I found that the procedure still performs well, and I believe the increase in legibility is worth it. Were performance to be an issue, I would probably consider changing that design.
