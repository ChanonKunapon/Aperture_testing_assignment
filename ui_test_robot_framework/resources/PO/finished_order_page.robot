*** Settings ***
Library      SeleniumLibrary


*** Variables ***
${valid_finished_order_label}      Checkout: Complete!

*** Keywords ***
verify correct checkout complete page
    Wait Until Page Contains   ${valid_finished_order_label}


verify incorrect checkout page
    Page Should Not Contain   ${valid_finished_order_label}


