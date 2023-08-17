"""Python file to serve as the frontend"""
import streamlit as st
from streamlit_chat import message
from langchain.chains import ConversationChain, SimpleSequentialChain, SequentialChain, TransformChain, LLMChain
from langchain.llms import AzureOpenAI
from parking_app import get_parking_intent_chain
from sequential_chain_example import get_chain
from fetch_user_vechicle_details import fetch_user_vehicle_details_chain
from langchain.chat_models import AzureChatOpenAI
from langchain.prompts import PromptTemplate

import os
def set_env():
    pass

set_env()

def get_vehicle_details(inputs: dict) -> dict:
    text = inputs["intent"]
    return {"user_vehicle_details" : text}

def load_chain():
    """Logic for loading the chain you want to use should go here."""
    # chain = ConversationChain(llm=llm)
    # chain = get_chain()
    # chain = get_parking_intent_chain()
    # overall_chain = SequentialChain(input_variables = ["input"], 
    #                     output_variables=["user_vehicle_details"], 
    #                     chains=[get_parking_intent_chain, fetch_user_vehicle_details_chain], 
    #                     verbose=True
    # )
    # overall_chain = get_chain()
    

    llm = AzureChatOpenAI(
        deployment_name="completion-gpt-35-turbo",
        model_name="gpt-35-turbo",
        temperature=0.0
    )
    
    prompt = PromptTemplate(
        template="Write a joke on {input}",
        input_variables=["input"]
    )
    llm_chain = LLMChain(
        prompt=prompt,
        llm=llm,
        output_key="intent"
    )

    def parse_output(inputs: dict) -> dict:
        text = inputs["intent"]
        return {"user_vehicle_details": text}

    transform_chain = TransformChain(
        input_variables=["intent"],
        output_variables=["user_vehicle_details"],
        transform=parse_output
    )

    uvd_chain = TransformChain(
        input_variables=["intent"], 
        output_variables=["user_vehicle_details"], 
        transform=get_vehicle_details
    )
    # return uvd_chain

    chain = SequentialChain(
        input_variables=["input"],
        output_variables=["user_vehicle_details"],
        chains=[llm_chain, uvd_chain],
    )

    # chain.run(query="Tell me a joke.")
    return chain

chain = load_chain()

# From here down is all the StreamLit UI.
st.set_page_config(page_title="LangChain Demo", page_icon=":robot:")
st.header("LangChain Demo")

if "generated" not in st.session_state:
    st.session_state["generated"] = []

if "past" not in st.session_state:
    st.session_state["past"] = []


def get_text():
    input_text = st.text_input("You: ", "Hello, how are you?", key="input")
    return input_text


user_input = get_text()

if user_input:
    output = chain.run({"input": {user_input}})

    st.session_state.past.append(user_input)
    st.session_state.generated.append(output)

if st.session_state["generated"]:

    for i in range(len(st.session_state["generated"]) - 1, -1, -1):
        message(st.session_state["generated"][i], key=str(i))
        message(st.session_state["past"][i], is_user=True, key=str(i) + "_user")