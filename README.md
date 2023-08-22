# Semantic Kernel
Semantic Kernel is an SDK that integrates Large Language Models (LLMs) like OpenAI, Azure OpenAI, and Hugging Face with conventional programming languages like C#, Python, and Java. Semantic Kernel achieves this by allowing you to define plugins that can be chained together in just a few lines of code.

## Installation
To install Semantic Kernel, you can use one of the following methods:

For C#, use the NuGet package manager and run the command Install-Package Microsoft.SemanticKernel
For Python, use the pip package manager and run the command pip install semantic-kernel
For Java, use the Maven dependency manager and add the following dependency to your pom.xml file:
<dependency>
  <groupId>com.microsoft</groupId>
  <artifactId>semantic-kernel</artifactId>
  <version>1.0.0</version>
</dependency>

## Usecase
![image](https://github.com/jogendrasinghgurjar/Semantic-Kernel-Workshop/assets/31069041/f1eb1f99-5f62-4ce6-b27d-22c078bea523)

### Problem Statement:

Problem: Difficulty in finding available parking spaces leads to frustration and annoyance for employees, causing a negative shift in their mood and experience as they arrive at work.

### Context: 
Despite starting the day with a positive outlook and excitement, the struggle to locate an empty parking spot upon reaching the office premises results in a gradual deterioration of emotions.

### Observation:
The recurrence of parking-related hack ideas submitted to Hackbox in recent hackathons highlights the widespread nature of this problem and its relatability to a larger audience.

### Objective:
Develop AI applications utilizing Language Model (LLM) technology to effectively address the issue of parking space scarcity and enhance the overall experience for employees.

### Scope of Solution:

Provide a solution for employees to effortlessly find available parking spots upon their arrival at the workplace.
Enable employees to use conversational queries to retrieve details about their vehicle's registration.
Create a user-centric experience by facilitating natural language interactions with the AI application.
Implementation Approach:

####  Identifying User Intent:

Analyze user queries to discern their intent regarding parking spot availability.
Example intent: Assistance in locating a free parking space.

#### Gathering Vehicle Information:

Acquire information about the user's vehicle type, which informs the search for appropriate parking spots.
Understand factors like vehicle size, permit requirements, etc.

#### Accessing Live Database:

Query a real-time database containing information on parking space availability.
Retrieve data on vacant parking spots, taking into account different categories (e.g., reserved, general).

#### Suggesting Parking Spaces:

Utilize AI to process gathered data and recommend suitable parking spots to the user.
Consider factors like proximity to the office, user preferences, and availability.

#### Enhancing User Experience:

Enable users to engage in natural language conversations with the AI application.
Create a seamless and user-friendly interface for queries and responses.

### Outcome: 
By implementing this solution, employees will experience a smoother transition from their morning positivity to their arrival at work, as the AI application aids them in finding available parking spaces effortlessly. The frustration associated with circling parking lots and the resulting negative emotions can be mitigated through the conversational AI-driven approach, thereby improving overall workplace satisfaction.


## Usage
To use Semantic Kernel, you need to create a semantic function that defines the logic of your AI app. A semantic function is composed of one or more plugins that can be connected with the | operator. For example, the following semantic function takes a user input and generates a response using an LLM:

from semantic_kernel import SemanticFunction

sf = SemanticFunction("user_input | openai('davinci') | response")
response = sf.run(user_input="Hello")
print(response)

You can also use variables to make your semantic function dynamic and reusable. For example, the following semantic function takes a topic and generates a summary using an LLM:

using Microsoft.SemanticKernel;

SemanticFunction sf = new SemanticFunction("topic | openai('davinci', 'summarize {topic}') | summary");
string summary = sf.Run(topic: "Semantic Kernel");
Console.WriteLine(summary);

You can find more examples and documentation on how to use Semantic Kernel on the official website.

## Contributing
Semantic Kernel is an open source project and we welcome contributions from the community. If you want to contribute, please follow these steps:

Fork the repository on GitHub
Clone your forked repository to your local machine
Create a new branch for your feature or bug fix
Make your changes and commit them with a descriptive message
Push your changes to your forked repository
Create a pull request from your forked repository to the original repository
Wait for feedback from the maintainers

### Please make sure to follow the code of conduct and the coding guidelines when contributing.

## presentation deck 
https://microsoftapc-my.sharepoint.com/:p:/g/personal/jogurjar_microsoft_com/Eek2CQxew09JhCOPPrEfjhoBDQy9Upl8blHC1SaPNNf9kg?e=fvDFa7
