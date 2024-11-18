*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valid_checkout_information_label}      Checkout: Your Information
${continue_button_location}              id=continue

*** Keywords ***
verify correct checkout information page
    Wait Until Page Contains   ${valid_checkout_information_label}

insert frist name
    [Arguments]           ${frist_anme}
     Input Text            locator=id=first-name        text=${frist_anme}

insert last name
    [Arguments]           ${last_name}
     Input Text            locator=id=last-name         text=${last_name}


insert ZIP code
    [Arguments]           ${zip_code}
     Input Text            locator=id=postal-code       text=${zip_code}

insert checkout infomation
    [Arguments]            ${first_anme}    ${last_name}    ${post_code}
    insert frist name      ${first_anme} 
    insert last name       ${last_name} 
    insert ZIP code        ${post_code}

press continue button
    Click Button             ${continue_button_location}  