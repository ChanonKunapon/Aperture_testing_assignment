*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valid_checkout_overview_label}      Checkout: Overview
${finish_button_location}        xpath=//*[@id="finish"]
${cancel_butto_location}        xpath=//*[@id="cancel"]

*** Keywords ***
verify correct checkout overview page
    Wait Until Page Contains   ${valid_checkout_overview_label}

verify selected item was found
    [Arguments]    @{items}
    FOR    ${item}    IN    @{items}
        Element Text Should Be    xpath=//*[contains(@class, "inventory_item_name") and text()="${item}"]    ${item}
    END


press finish button
    Click Button  ${finish_button_location}

press cancel button
    Click Button  ${cancel_butto_location} 