from langchain.prompts import FewShotChatMessagePromptTemplate
# Import Azure OpenAI
from langchain.llms import AzureOpenAI
from langchain.chains import LLMChain
from langchain.prompts import FewShotChatMessagePromptTemplate

import os

def set_env():
    pass

set_env()

from langchain.prompts.chat import (
    ChatPromptTemplate,
    SystemMessagePromptTemplate,
    HumanMessagePromptTemplate
)
from langchain.chat_models import AzureChatOpenAI


def get_parking_intent_chain():
    intent_identification_examples = [
        {"input": "Find me the nearest parking spot.", "output": "GET PARKING SPOT"},
        {"input": "Do you have the registration details of my vehicle?", "output": "GET REGISTRATION DETAILS"},
        {"input": "Where can I park my vehicle?", "output": "GET PARKING SPOT"},
        {"input": "Give me the registration details.", "output": "GET REGISTRATION DETAILS"}
    ]

    # This is a prompt template used to format each individual example.
    example_prompt = ChatPromptTemplate.from_messages(
        [
            ("human", "{input}"),
            ("ai", "{output}"),
        ]
    )
    few_shot_prompt = FewShotChatMessagePromptTemplate(
        example_prompt=example_prompt,
        examples=intent_identification_examples,
    )

    final_prompt = ChatPromptTemplate.from_messages(
        [
            ("system", "You are a helpful assistant that identifies the intent of a user's query. There are 2 possible intents - GET PARKING SPOT, GET REGISTRATION DETAILS. Choose the most appropriate intent based on the human's query. Here are some examples for you to understand how to output the intent."),
            few_shot_prompt,
            ("human", "{input}\n AI: "),
        ]
    )

    llm = AzureChatOpenAI(
        deployment_name="completion-gpt-35-turbo",
        model_name="gpt-35-turbo",
        temperature=0.0
    )
    intent_chain = LLMChain(llm=llm, prompt=final_prompt, output_key="intent")
    return intent_chain

def display_parking_response():
    pass
