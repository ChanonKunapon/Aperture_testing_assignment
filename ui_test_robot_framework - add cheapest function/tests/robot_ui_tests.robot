*** Settings ***
Documentation     Apature QA testing Ui test
Library           SeleniumLibrary
Resource          ../resources/common.robot
Resource          ../resources/robot_ui_tests_app.robot
Test Setup        common.begin web test  ${BROWSER}    ${WEB_URL}
Test Teardown     common.end web test

*** Variables ***
${BROWSER}                    edge
${WEB_URL}                    https://www.saucedemo.com/
${MAIN_PASSWORD}                  secret_sauce
${VALID_USER}                     standard_user     
${LOCKED_OUT_USER}                locked_out_user   
${PROBLEM_USER}                   problem_user
${PERFORMANCEGLITCH_USER}         performance_glitch_user
${ERROR_USER}                     error_user
${VISUAL_USER}                    visual_user
${LOGIN_ERROR_MESSAGE}    Epic sadface: Username and password do not match any user in this service.

*** Test Cases ***
test to login and select cheapest item
    [Tags]  login_test_cheapest
    robot_ui_tests_app.access login page and login   ${VALID_USER}  ${MAIN_PASSWORD}
    robot_ui_tests_app.Add Cheapest Item To Cart
    sleep  2s


shoudd be able to login with valid credential 
    [Tags]  login_test
    robot_ui_tests_app.access login page and login   ${VALID_USER}  ${MAIN_PASSWORD}

logout button should direct to login page
    [Tags]  logout_test
    robot_ui_tests_app.access login page and login   ${VALID_USER}  ${MAIN_PASSWORD}
    robot_ui_tests_app.logout to login page
    login_page.verify correct login page


valid user should be ordering item
    [Tags]  valid_user_test    
    robot_ui_tests_app.access login page and login   ${VALID_USER}  ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page 
    robot_ui_tests_app.access to checkout overview page
    robot_ui_tests_app.access to finished order page

locked out user should cannot login
    [Tags]  locked_out_user
    robot_ui_tests_app.access login page and login   ${LOCKED_OUT_USER}   ${MAIN_PASSWORD}
    Element Text Should Be  locator=css=#login_button_container > div > form > div.error-message-container.error > h3   expected=Epic sadface: Sorry, this user has been locked out.

poblem user should not check out
    [Tags]  problem_user_test
    VAR  ${EMPTY_LASTNAME_MESSAGE}      Error: Last Name is required
    robot_ui_tests_app.access login page and login     ${PROBLEM_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page wiith problem user    ${EMPTY_LASTNAME_MESSAGE}

performance glitch user should able to ordering item
    [Tags]  glitch_user_test
    robot_ui_tests_app.access login page and login   ${PERFORMANCEGLITCH_USER}  ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page 
    robot_ui_tests_app.access to checkout overview page
    robot_ui_tests_app.access to finished order page

error user should not perform checkout
    [Tags]  error_user_test
    robot_ui_tests_app.access login page and login   ${ERROR_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page
    checkout_overview_page.press finish button
    robot_ui_tests_app.verify page should not change to finished order page

visual user should be able to order
    [Tags]  visual_user_test
    robot_ui_tests_app.access login page and login   ${VISUAL_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page 
    robot_ui_tests_app.access to checkout overview page
    robot_ui_tests_app.access to finished order page

empty username should not loggin
    [Tags]  empty_login_test
    VAR    ${EMPTY_USERNAME_MESSAGE}          Epic sadface: Username is required
    robot_ui_tests_app.Run Invalid Login Test    ${EMPTY}    ${MAIN_PASSWORD}     ${EMPTY_USERNAME_MESSAGE}

empty password should not loggin
    [Tags]  empty_login_test
    VAR  ${EMPTY_PASSWORD_MESSAGE}      Epic sadface: Password is required
    robot_ui_tests_app.Run Invalid Login Test     ${VALID_USER}     ${EMPTY}       ${EMPTY_PASSWORD_MESSAGE}

empty username and password should not loggin
    [Tags]  empty_login_test
    VAR  ${EMPTY_USER_PWD_MESSAGE}      Epic sadface: Username is required
    robot_ui_tests_app.Run Invalid Login Test    ${EMPTY}      ${EMPTY}        ${EMPTY_USER_PWD_MESSAGE} 

empty cart checkout should show error message
    [Tags]  empty_cart_checkout_test
    robot_ui_tests_app.access login page and login  ${VALID_USER}   ${MAIN_PASSWORD}   
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout Information page 
    checkout_overview_page.press finish button
    robot_ui_tests_app.access to incorrect finish order page

empty fristname information should not check out
    [Tags]  empty_information_checkout_test
    robot_ui_tests_app.access login page and login  ${VALID_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout empty Information page    first_anme=${EMPTY}   last_name=kunaponsudjalit   post_code=51000    expected_error=Error: First Name is required

empty lastname information should not check out
    [Tags]  empty_information_checkout_test
    robot_ui_tests_app.access login page and login  ${VALID_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout empty Information page   first_anme=chanon     last_name=${EMPTY}     post_code=51000   expected_error=Error: Last Name is required

empty postcode information should not check out
    [Tags]  empty_information_checkout_test
    robot_ui_tests_app.access login page and login  ${VALID_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout empty Information page   first_anme=chaon     last_name=kunaponsudjalit     post_code=${EMPTY}    expected_error=Error: Postal Code is required

empty information should not check out
    [Tags]  empty_information_checkout_test
    robot_ui_tests_app.access login page and login  ${VALID_USER}   ${MAIN_PASSWORD}
    robot_ui_tests_app.add product to cart
    robot_ui_tests_app.access to cart page
    robot_ui_tests_app.process to checkout empty Information page   first_anme=${EMPTY}     last_name=${EMPTY}     post_code=${EMPTY}   expected_error=Error: First Name is required