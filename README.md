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
