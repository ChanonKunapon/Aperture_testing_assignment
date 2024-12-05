*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valod_login_page_element}      xpath=//*[@id="login-button"] 

*** Keywords ***
verify correct login page
    Wait Until Page Contains Element  ${valod_login_page_element}


login useing credential
    [Arguments]        ${USERNAME}  ${PASSWORD}
    Input Text    locator=id=user-name   text=${USERNAME}
    Input Password  locator=id=password   password=${PASSWORD}
    Click Button  locator=id=login-button

press burger menu button
    Wait Until Element Is Visible    xpath=//*[@id="react-burger-menu-btn"]    timeout=5s
    Wait Until Element Is Enabled    xpath=//*[@id="react-burger-menu-btn"]    timeout=5s
    Click Element                    xpath=//*[@id="react-burger-menu-btn"]
    Wait Until Element Is Visible    xpath=//*[@id="logout_sidebar_link"]    timeout=5s
    Log                              Opened the burger menu


press logout button
    Wait Until Element Is Visible    xpath=//*[@id="logout_sidebar_link"]    timeout=5s
    Wait Until Element Is Enabled    xpath=//*[@id="logout_sidebar_link"]    timeout=5s
    Click Element                    xpath=//*[@id="logout_sidebar_link"]
    Log                              Clicked the logout button
