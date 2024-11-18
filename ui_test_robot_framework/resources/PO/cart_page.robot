*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valid_cart_page_label}      Your Cart
${continue_button}        xpath=//*[@id="continue-shopping"]
${checkout_button}        xpath=//*[@id="checkout"]

*** Keywords ***
verify correct cart page
    Wait Until Page Contains   ${valid_cart_page_label}

verify selected product was found
    [Arguments]    @{items}
    FOR    ${item}    IN    @{items}
        Wait Until Page Contains   xpath://div[text()='${item}']/ancestor::div[@class='inventory_item']//button[text()='Add to cart']
    END


press continus shopping button
    Click Button  ${continue_button}


press checkout button
    Click Button      ${checkout_button}
