Sorting
=======

What's interesting about the project that these samples came from is not the algorithm that is used, but is the purpose of its use.

In the project that these samples are coming from, I was to design a system that would allow an IT department to more effectively prioritize its trouble tickets.

The problem was that they were having trouble seeing the forest through the trees. They had no high level framework or rubric through which they could vet tickets en mass. They needed a way of developing this framework. The tool that I developed here was used for the first step in developing that framework.

I had the IT team come up with a number of general categories, issues, and common occurences that affected tickets. The only rule in the development of this list was that the items in the list be mutually exclusive. I then wrote a program that would take that list of items as input. It would then randomize the list and push the list through merge sort. Since the list contains discrete, non-numeric values, the sort algorithm would ask a team member to choose which of the two items being compared is more important.

Because team members were being used to make the comparisons, it was important to keep the number of comparisons to a minimum (to minimize test fatigue). This is the primary reason merge sort was used for this application. Compared to other sort algorythms it has a relatively low and stable number of iterations required to complete a set of comparisons.

Several team members would use the program to come up each with a prioritized list. These lists were then compared and a final priority order was agreed upon. This then because the basis for further development of a ticket prioritization rubric.

I could go on about this for hours, so I suppose I'll just cut it off there.
