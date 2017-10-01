
$(document).ready(function() {
    debugger;
    var url = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/DemoService.svc/GetResponse";
    console.log("Testing WS on URL: " +  url);

    //Shortcut to JSON Format and HTTP-GET
    $.getJSON(url, function(data) {
        console.log("Response from WS: " + data);
    });

    //Allows more control and other HTTP-Verbs than GET
    console.log("Testing WS on URL using $.ajax(): " + url);

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        processdata: true,
        success: function (msg) {
            console.log(msg);
        },
        error: function (msg) {
            console.log(msg);
        }
    });

    //Using Parameters
    
    url = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/DemoService.svc/GetResponseByValue/TestMe";
    console.log("Testing WS on URL: " + url);

    $.getJSON(url, function (data) {
        console.log("Response from WS: " + data);
    });


    var data = { "Value": "Sent using POST" }
    url = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/DemoService.svc/SetResponse?Param=" + JSON.stringify(data) ;

    $.ajax({
        type: "POST",
        url: url,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        processdata: true,
        success: function (msg) {
            if (msg!=null) {
                var result = JSON.parse(msg);
                console.log(result.Value);
            }            
        },
        error: function (msg) {
            console.log(msg);
        }
    });
    
    //Complex Type Params

    var data = JSON.stringify({ "Id": 0, "Name": "Demo Complex Payload" });
    url = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/DemoService.svc/SetPayload";

    $.ajax({
        type: "POST",
        url: url,
        xhrFields: {
            withCredentials: true
        },
        data: data,
        crossDomain: true,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        processdata: true,
        success: function (msg) {
            console.log(msg);
        },
        error: function (msg) {
            console.log(msg);
        }
    });  
})