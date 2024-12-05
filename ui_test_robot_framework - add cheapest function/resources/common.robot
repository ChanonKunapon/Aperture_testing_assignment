*** Settings ***
Library        SeleniumLibrary



*** Keywords ***
begin web test
    [Arguments]  ${BROWSER}  ${WEB_URL}
    Log  begin testing
    Open Browser    ${WEB_URL}   ${BROWSER}


end web test
    Close All Browsers




  