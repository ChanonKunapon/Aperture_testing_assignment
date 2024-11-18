*** Settings ***
Library         SeleniumLibrary
Resource        ../resources/PO/login_page.robot
Resource        ../resources/PO/product_page.robot
Resource        ../resources/PO/cart_page.robot
Resource        ../resources/PO/checkout_Information_page.robot
Resource        ../resources/PO/checkout_overview_page.robot
Resource        ../resources/PO/finished_order_page.robot

*** Variables ***
@{ITEMS_TO_ADD}  Sauce Labs Backpack  Sauce Labs Bike Light  Sauce Labs Bolt T-Shirt
@{CHECKOUT_INFOMATIONS}        chanon        Kunaponsudjalit        51000


*** Keywords ***    
access login page and login
    [Arguments]  ${USERNAME}   ${PASSWORD}
    login_page.verify correct login page    
    login_page.login useing credential  ${USERNAME}   ${PASSWORD}
    
    

Run Invalid Login Test
    [Arguments]    ${username}    ${password}    ${expected_error}
    access login page and login  ${username}   ${password}
    Element Text Should Be        locator=css=#login_button_container > div > form > div.error-message-container.error   expected=${expected_error}


add product to cart
    product_page.verify correct product page
    product_page.add product to cart  @{ITEMS_TO_ADD}     

access to cart page    
    product_page.go to cart page
    cart_page.verify correct cart page
    cart_page.verify selected product was found
    cart_page.press checkout button
    checkout_Information_page.verify correct checkout information page

process to checkout Information page    
    checkout_Information_page.insert checkout infomation  @{CHECKOUT_INFOMATIONS}
    checkout_Information_page.press continue button
    checkout_overview_page.verify correct checkout overview page

process to checkout empty Information page
    [Arguments]   ${first_anme}    ${last_name}    ${post_code}    ${expected_error}
    checkout_Information_page.insert checkout infomation   ${first_anme}    ${last_name}    ${post_code}
    checkout_Information_page.press continue button
    Element Text Should Be        locator=xpath=//*[@id="checkout_info_container"]/div/form/div[1]/div[4]/h3   expected=${expected_error}                                           

process to checkout Information page wiith problem user
    [Arguments]  ${EMPTY_LASTNAME_MESSAGE}
    checkout_Information_page.insert checkout infomation  @{CHECKOUT_INFOMATIONS}
    checkout_Information_page.press continue button
    Element Text Should Be        locator=xpath=//*[@id="checkout_info_container"]/div/form/div[1]/div[4]/h3   expected=${EMPTY_LASTNAME_MESSAGE}

access to checkout overview page
    checkout_overview_page.verify selected item was found  @{ITEMS_TO_ADD}
    checkout_overview_page.press finish button

access to finished order page    
    finished_order_page.verify correct checkout complete page

access to incorrect finish order page
    finished_order_page.verify incorrect checkout page


logout to login page
    login_page.press burger menu button
    login_page.press logout button
    
verify page should not change to finished order page
    Element Should Be Visible   xpath=//*[@id="finish"]   
    Element Text Should Not Be    xpath=//*[@id="header_container"]/div[2]/span   Checkout: Complete!
