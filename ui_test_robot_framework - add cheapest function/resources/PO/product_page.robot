*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valid_product_page_label}      Products
${cart_button}  xpath=//*[@id="shopping_cart_container"]/a

*** Keywords ***
verify correct product page
    Wait Until Page Contains   ${valid_product_page_label}

add product to cart
    [Arguments]    @{items}
    FOR    ${item}    IN    @{items}
        Click Button    xpath://div[text()='${item}']/ancestor::div[@class='inventory_item']//button[text()='Add to cart']
    END

go to cart page
    Click Link  ${cart_button}