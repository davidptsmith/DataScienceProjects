***Please Note: This report discusses acts of terrorism.***

This trigger warning for anyone who may be personally affected by this topic - specific details of attacks are not explicitly discussed however summaries of events are provided as well as other statistics

<br/>


# Global Terrorism Database 

This was a university project where we had to explore a dataset and then look at creating models. I worked with a friend and colleague studying Violent Extremism who asked if I could help him explore questions form the Global Terrorism Databases Global Terrorism Database (GTD) collated and maintained by the START institute in the United States. 


The data was so interesting and truly saddening, however it opened me up to a lot of history that I had never known about. 
Please feel free to explore the R notebooks and the outcomes, however note that the data set has significant bias and the modeling is not reliable but completed for the purpose of the assignment. 
 

<br/>

![image](https://user-images.githubusercontent.com/76982323/179906492-20283704-8ea0-4a1d-a694-31c7d135e53f.png)

<br/>

# Introduction

Acts of terrorism have a way of grabbing headlines. By their nature, these horrific acts of violence are used to instill fear and are enacted for a myriad of reasons and yet are seldom understood. Many aspects of society have fundamentally changed due to acts of terrorism. Air travel, urban design, financial systems, and diplomatic strategy have all had to adapt to the modern counter terrorism landscape. 

The importance of this topic to modern society cannot be understated. Hundreds of thousands of people have been killed in the name of countless causes globally. With the number of attacks significantly increasing due to various enabling factors, research, or at least publicly available research, seems to be lagging behind as information is difficult to define, attain, and collate. The lack of quantitative information available has meant that the majority of research on terrorism is primarily qualitative conjecture. 

A 2011 study performed in the United States by the National Consortium for the Study of Terrorism And Response to Terrorism (START) found that of 300 hypotheses formulated around terrorism in academia only 8 possessed both significant qualitative and quantitative supporting evidence.^[Influencing Violent Extremist Organizations: Planning Influence Activities While Accounting for Unintended Side Effects (I-VEO), National Consortium for the Study of Terrorism and Responses to Terrorism (START), University of Maryland. (2011).  Retrieved from http://start.foxtrotdev.com/] With conflicting sources, definitions, and unreliable data this is unsurprising however, there is considerable valuable to be gained through the exploration of data and ideas relating to terrorism.

The primary goal of the data exploration is to gain insights from historic terrorism data to better understand:

1) How is terrorism evolving over time?  
2) Who are the primary groups, how do they operate, and where do they operate?  

These high-level questions will provide the framework for further analysis questions that explore more specific aspects throughout the data exploration process. 


# The Global Terrorism Database 

The data chosen for this exploration is the Global Terrorism Database (GTD) collated and maintained by the START institute in the United States. The GTD collates information across multiple dimensions from unclassified media articles which form 100 structured variables that categorize key information pertaining to the attack and various unstructured variables that contain summaries and other general information that allows for contextualization. 

> The Global Terrorism Database (GTD) documents more than 200,000 international and domestic terrorist attacks that occurred worldwide since 1970. With details on various dimensions of each attack, the GTD familiarizes analysts, policymakers, scholars, and journalists with patterns of terrorism.^[National Consortium for the Study of Terrorism and Responses to Terrorism (START), University of Maryland. (2019). The Global Terrorism Database (GTD) Retrieved from "https://www.start.umd.edu/gtd"]
> `r tufte::quote_footer('Global Terrorism Database Code Book')`   

## Definition Of An Act Of Terrorism 

Defining what constitutes as an act of terrorism is a considerably difficult task to define and something that has yet to reach consensus in research. The definition that the START uses is a good all-round definition that has been implemented in order to capture more specifically what would traditionally be attributed to an act of terrorism as opposed to an act of violence. 

> The GTD defines a terrorist attack as the threatened or actual use of illegal force and violence by a non-state actor to attain a political, economic, religious, or social goal through fear, coercion, or intimidation.^[National Consortium for the Study of Terrorism and Responses to Terrorism (START), University of Maryland. (2019). The Global Terrorism Database (GTD) Retrieved from "https://www.start.umd.edu/gtd"]
> `r tufte::quote_footer('Global Terrorism Database Code Book')`
The definition is further extended in the filtering of information using a specific criteria to ensure a reduction in ambiguity   across those inputting information.

## Methodology of the GTD

>GTD Definition of Terrorism and Inclusion Criteria 
The GTD defines a terrorist attack as the threatened or actual use of illegal force and violence by a non-state actor to attain a political, economic, religious, or social goal through fear, coercion, or intimidation. In practice this means in order to consider an incident for inclusion in the GTD, all three of the following attributes must be present: 
>
>   - The incident must be intentional – the result of a conscious calculation on the part of a perpetrator. 
>   - The incident must entail some level of violence or immediate threat of violence -including property violence, as well as violence against people. 
>  
>   - The perpetrators of the incidents must be sub-national actors. The database does not include acts of state terrorism. 
In addition, at least two of the following three criteria must be present for an incident to be included in the GTD: 
>   - Criterion 1: The act must be aimed at attaining a political, economic, religious, or social goal. In terms of economic goals, the exclusive pursuit of profit does not satisfy this criterion. It must involve the pursuit of more profound, systemic economic change. 
>  
>    - Criterion 2: There must be evidence of an intention to coerce, intimidate, or convey some other message to a larger audience (or audiences) than the immediate victims. It is the act taken as a totality that is considered, irrespective if every individual involved in carrying out the act was aware of this intention. As long as any of the planners or decision-makers behind the attack intended to coerce, intimidate or publicize, the intentionality criterion is met.   
>  
>   - Criterion 3: The action must be outside the context of legitimate warfare activities. That is, the act must be outside the parameters permitted by international humanitarian law, insofar as it targets non-combatants   
>  
>Each of these latter three criteria filters can be applied to the database on the GTD website and the full data file.^[National Consortium for the Study of Terrorism and Responses to Terrorism (START), University of Maryland. (2019). The Global Terrorism Database (GTD) Retrieved from "https://www.start.umd.edu/gtd"]
> `r tufte::quote_footer('Global Terrorism Database Code Book')`  
As a note, given recent events in Afghanistan by the Taliban some of these criteria need to be considered further. For example, now that the Taliban are controlling the country and are the dominate political entity does this mean that they can no longer be classes as non-state actors? However this is an exploration for another project, for now we will be using the data as is.

 

## Known Data Issues & Biases   

**Data Reliability Issues**  

Collecting quality data is difficult in even the most controlled situations where acts of terrorism are not easily understood and are difficult to analyse as such many errors can occur and the GTD reflects this.   

Many attacks occur with unknown information or conflicting information. Whilst the START organisation does its best to collect quality information this task relies heavily on the third parties. The quality of data can be affected in many ways and it is important to consider these when analysing a dataset.  


**1993 missing data**   
The data collected from 1993 is not included in this data analysis due it being lost with recovery efforts only able to recover 15% of the data. The 1993 has not being incorporated into this data set as it can lead to skewing of results and visualizations.

>Users familiar with the GTD’s data collection methodology are aware that incidents of terrorism from 1993 are not present in the GTD because they were lost prior to START’s compilation of the GTD from multiple data collection efforts. Several efforts were made to re-collect these incidents from original news sources. Unfortunately, due to the challenges of retrospective data collection for events that happened more than 25 years ago, the number of 1993 cases for which sources were identified is only 15% of estimated attacks. As a consequence we exclude all 1993 attacks from the GTD data to prevent users from misinterpreting the low frequency in 1993 as an actual count. However, Appendix II provides country level statistics for 1993. These figures were obtained from an early report on the data compiled before the 1993 files were lost. In addition, the 1993 data we do have is available to download from the GTD website.^[National Consortium for the Study of Terrorism and Responses to Terrorism (START), University of Maryland. (2019). The Global Terrorism Database (GTD) Retrieved from "https://www.start.umd.edu/gtd"]
> `r tufte::quote_footer('Global Terrorism Database Code Book')` 

**Changing Processes & Materials**

The GTD has employed a single definition over the course of its data connection efforts however, there have been other changes made to the collection of materials and workflows employed to gather information. Intrinsic to the process of data collect and filtration of data this will lead to some leave of variation over time.  

Whilst reviewing that collection methodology there has been significant changes in processes in 1998, 2008, and 2012. As such it is important to understand that this process coupled with other enablers such as the pervasiveness of the internet and media focus that could explain some of the increase in the number of recorded attacks.  

**Legacy Issues**  

The GTD is an evolving database and collection effort that goes through some changes over time. As such new data fields are added to allow for the better understanding of events and terrorism landscape. With these new data fields an attempt is made to retroactively code historic events however, this is not always possible and therefore data might be skewed due to the biases in the encoding process. These changes are explicitly mentioned in the code book.  

**Remote Locations & Missing Information or Events **  

With some attacks occurring in remote and impoverished regions gathering accurate and reported information is challenging. The results of these events therefore can be inaccurate, unknown, or missed. It is important to understand that the data collected here comes from only open, non-classified channels – primarily media reporting.   

**Definitions**  

Difficult to determine if that attack is an act of terrorism or an act of violence. The distinction and subtleties around the definitions are often hard to discern. In most of these cases, they are included into the GTD.   

**Media Bias**  

Media as the primary source makes it very difficult to ensure data reliability as it has its own biases. These can include: 

  -	Political agendas of media  

  -	Only reporting on what sells or is deemed important to be broadcast 

  -	Can be restricted by governments, military, and police 

  -	May wish not to report information that might lead to some form of economic, social, governmental, or terrorist organisation backlash

If the database is relying on media coverage it is it can be assumed that only those attacks big enough to have some leave of reports will be collected. This might cause a skew of the data towards larger, more impactful events. 


**Conflicting Information **

Terrorist acts can often be shrouded in a level of ambiguity or confusion, and this can lead to conflicting information. The START institute does its best to filter through sources to gain the most accurate information however sometimes this is not possible. Therefore, there are many unknowns evident in the database – these can arise to conflicting information that means they cannot be properly attributed. 

There are more biases and issues with regards to specific data fields and variables. These comments will be made along with the analysis. 


# Note: Graphics & Formatting Comments 


This report was created to be displayed using interactive charts through a web browser.

Where available, please use the HTML Features to your advantage: 

  - Hover to see further information   
  - Click the legend items to hide it
  - Double click legend items to isolate it  
  - Zoom in on the map and click a single event to see further information & event summary  


Some of the colour schemes are chosen to aesthetically represent the data being discussed however they do not have the correct level of contrast to be viewed in a printed or PDF format. 
